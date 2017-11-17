using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hospital.Models;
using System.Diagnostics;

namespace Hospital.Implementation
{
    public class HospitalResource : IHospitalResource
    {
        /// <summary>
        /// Get list of registred patients
        /// </summary>
        public IEnumerable<Patient> Patients
        {
            get
            {
                return _patients.Values.ToArray();
            }
        }

        /// <summary>
        /// Get list of consulations
        /// </summary>
        public IEnumerable<Consultation> Consultations
        {
            get
            {
                return _consultations.Values.ToArray();
            }
        }

        /// <summary>
        /// Get patient
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Patient GetPatient(string name)
        {
            return _patients.ContainsKey(name) ? _patients[name] : null;
        }

        /// <summary>
        /// Register a patient
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public Patient AddPatient(Patient patient)
        {
            if (_patients.ContainsKey(patient.Name))
            {
                return null;
            }
                       
            _patients.Add(patient.Name, patient);
            AddConsulation(patient);
            
            return patient;
        }

        /// <summary>
        /// Delete a patient
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Patient DeletePatient(string name)
        {
            if (!_patients.ContainsKey(name))
            {
                return null;
            }
            
            Patient p = _patients[name];
            _patients.Remove(name);

            DeleteConsulation(p);

            return p;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public HospitalResource()
        {
            _rooms = new Dictionary<string, TreatmentRoom>()
            {
                { "One", new TreatmentRoom {Name="One", TreatmentMachine =_machines["Elekta"]}},
                { "Two", new TreatmentRoom {Name="Two", TreatmentMachine =_machines["Varian"]}},
                { "Three", new TreatmentRoom {Name="Three", TreatmentMachine =_machines["MM50"]}},
                { "Four", new TreatmentRoom {Name="Four"}},
                { "Five", new TreatmentRoom {Name="Five"}}
            };
        }

        /// <summary>
        /// Schedule the earliest consultation for the patient
        /// WARNING: right now there's not resource optimization technique implemented, e.g., flu patients don't need room with machine,  
        /// but the current implementation does not take this into account
        /// </summary>
        /// <param name="p"></param>
        public void AddConsulation(Patient p)
        {
            // get the list of available doctors and the list of avaiblable rooms
            var doctorIntervalList = new LinkedList<Tuple<DateRange, object>>();

            var roomIntervalList = new LinkedList<Tuple<DateRange, object>>();

            if (p.Condition == Patient.PatientCondition.Cancer)
            {
                foreach (var d in _doctors)
                {
                    if (d.Value.Roles.Contains(Doctor.Role.Oncologist))
                    {
                        MergeIntervalList(doctorIntervalList, d.Value.Scheduler.GetAvailableIntervals());
                    }
                }

                if (p.Topology == Patient.CancerTopology.HeadAndNeck)
                {
                    foreach (var r in _rooms)
                    {
                        if (r.Value.TreatmentMachine != null &&
                            r.Value.TreatmentMachine.Capability == TreatmentMachine.MachineCapability.Advanced)
                        {
                            MergeIntervalList(roomIntervalList, r.Value.Scheduler.GetAvailableIntervals());
                        }
                    }
                }
                else if (p.Topology == Patient.CancerTopology.Breast)
                {
                    foreach (var r in _rooms)
                    {
                        if (r.Value.TreatmentMachine != null)
                        {
                            MergeIntervalList(roomIntervalList, r.Value.Scheduler.GetAvailableIntervals());
                        }
                    }
                }
            }
            else if (p.Condition == Patient.PatientCondition.Flu)
            {
                foreach (var d in _doctors)
                {
                    if (d.Value.Roles.Contains(Doctor.Role.GeneralPractitioner))
                    {
                        MergeIntervalList(doctorIntervalList, d.Value.Scheduler.GetAvailableIntervals());
                    }
                }

                foreach (var r in _rooms)
                {
                    MergeIntervalList(roomIntervalList, r.Value.Scheduler.GetAvailableIntervals());                    
                }
            }

            // retrieve the earliest time
            var appointment = BookTime(doctorIntervalList, roomIntervalList);

            // book the doctor
            var doctor = appointment.Item2 as Doctor;
            Debug.Assert(doctor != null);
            bool bookedDoctor = doctor.Scheduler.Book(appointment.Item1);
            Debug.Assert(bookedDoctor);

            // book the room
            var room = appointment.Item3 as TreatmentRoom;
            Debug.Assert(room != null);
            bool bookedRoom = room.Scheduler.Book(appointment.Item1);
            Debug.Assert(bookedRoom);

            // set the patient appointment info
            p.SetAppointment(appointment.Item1, doctor.Name, room.Name);

            // add the consulation to the list
            _consultations.Add( new Tuple<DateTime, string>(appointment.Item1, p.Name), 
                                new Consultation { Patient = p, Doctor = doctor, TreatmentRoom = room, AppointmentDate = appointment.Item1 });

        }

        /// <summary>
        /// remove the consulation
        /// </summary>
        /// <param name="p"></param>
        public void DeleteConsulation(Patient p)
        {
            var key = new Tuple<DateTime, string>(p.AppointmentTime, p.Name);
            if (_consultations.ContainsKey(key))
            { 
                var value = _consultations[key];
                value.Doctor.Scheduler.Cancel(key.Item1);
                value.TreatmentRoom.Scheduler.Cancel(key.Item1);
                _consultations.Remove(key);
            }
        }

        /// <summary>
        /// merge the sorted addedlist to the sorted base list. The two lists are sorted on ascending order based on start date. o(n), n being the combined list size
        /// </summary>
        /// <param name="baseList"></param>
        /// <param name="addedList"></param>
        private void MergeIntervalList(LinkedList<Tuple<DateRange, object>> baseList, LinkedList<Tuple<DateRange, object>> addedList)
        {
            var baseNode = baseList.First;
            var addedNode = addedList.First;

            // merge
            while (addedNode != null && baseNode != null)
            {
                if (baseNode.Value.Item1.CompareTo(addedNode.Value.Item1) > 0)
                {
                    baseList.AddBefore(baseNode, addedNode.Value);
                    addedNode = addedNode.Next;
                }
                else
                {
                    baseNode = baseNode.Next;
                }
            }

            // add the remaining nodes
            while (addedNode != null)
            {
                baseList.AddLast(addedNode.Value);
                addedNode = addedNode.Next;
            }
        }

        /// <summary>
        /// Find the earliest DateRange that overlaps and return the earliest date
        /// </summary>
        /// <param name="list1">sorted list</param>
        /// <param name="list2">sorted list</param>
        /// <returns>earliest date that the two interval lists overlap</returns>
        private Tuple<Date, object, object> BookTime(LinkedList<Tuple<DateRange, object>> list1, LinkedList<Tuple<DateRange, object>> list2)
        {
            var result = new Tuple<Date, object, object>(DateTime.MinValue, null, null);

            var node1 = list1.First;
            var node2 = list2.First;
            while (node1 != null && node2 != null)
            {
                var overlap = node1.Value.Item1.Overlap(node2.Value.Item1);
                if (overlap.Item1)
                {
                    return new Tuple<Date, object, object>(overlap.Item2, node1.Value.Item2, node2.Value.Item2);
                }

                if (node1.Value.Item1.CompareTo(node2.Value.Item1) < 0)
                {
                    node1 = node1.Next;
                }
                else
                {
                    Debug.Assert(node1.Value.Item1.CompareTo(node2.Value.Item1) > 0);
                    node2 = node2.Next;
                }
            }

            return new Tuple<Date, object, object>(DateTime.MaxValue, null, null);
        }

        private Dictionary<string, Patient> _patients = new Dictionary<string, Patient>();

        private Dictionary<string, Doctor> _doctors = new Dictionary<string, Doctor>()
        {
            { "John", new Doctor {Name="John", Roles = { Doctor.Role.Oncologist} }},
            { "Anna", new Doctor {Name="Anna", Roles = { Doctor.Role.GeneralPractitioner} }},
            { "Peter", new Doctor {Name="Peter", Roles = { Doctor.Role.Oncologist, Doctor.Role.GeneralPractitioner} }},
        };

        private Dictionary<string, TreatmentMachine> _machines = new Dictionary<string, TreatmentMachine>()
        {
            { "Elekta", new TreatmentMachine {Name="Elekta", Capability = TreatmentMachine.MachineCapability.Advanced}},
            { "Varian", new TreatmentMachine {Name="Varian", Capability = TreatmentMachine.MachineCapability.Advanced}},
            { "MM50", new TreatmentMachine {Name="MM50", Capability = TreatmentMachine.MachineCapability.Simple}}
        };

        private Dictionary<string, TreatmentRoom> _rooms = new Dictionary<string, TreatmentRoom>();

        private SortedDictionary<Tuple<DateTime, string>, Consultation> _consultations = new SortedDictionary<Tuple<DateTime, string>, Consultation>();

    }
}