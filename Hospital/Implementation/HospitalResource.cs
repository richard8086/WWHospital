using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hospital.Models;
using System.Diagnostics;
using Hospital.Interfaces;

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
        public HospitalResource(IUtility utility, IAppointmentScheduler scheduler)
        {
            _utility = utility;
            _schedulerSeed = scheduler;

            _machines = new Dictionary<string, TreatmentMachine>()
            {
                { "Elekta", new TreatmentMachine {Name="Elekta", Capability = TreatmentMachine.MachineCapability.Advanced}},
                { "Varian", new TreatmentMachine {Name="Varian", Capability = TreatmentMachine.MachineCapability.Advanced}},
                { "MM50", new TreatmentMachine {Name="MM50", Capability = TreatmentMachine.MachineCapability.Simple}}
            };

            _rooms = new Dictionary<string, TreatmentRoom>()
            {
                { "One", new TreatmentRoom(_schedulerSeed.Create()) {Name="One", TreatmentMachine =_machines["Elekta"]}},
                { "Two", new TreatmentRoom(_schedulerSeed.Create()) {Name="Two", TreatmentMachine =_machines["Varian"]}},
                { "Three", new TreatmentRoom (_schedulerSeed.Create()){Name="Three", TreatmentMachine =_machines["MM50"]}},
                { "Four", new TreatmentRoom(_schedulerSeed.Create()) {Name="Four"}},
                { "Five", new TreatmentRoom(_schedulerSeed.Create()) {Name="Five"}}
            };

             _doctors = new Dictionary<string, Doctor>()
            {
                { "John", new Doctor(_schedulerSeed.Create()) {Name="John", Roles = { Doctor.Role.Oncologist} }},
                { "Anna", new Doctor(_schedulerSeed.Create()) {Name="Anna", Roles = { Doctor.Role.GeneralPractitioner} }},
                { "Peter", new Doctor(_schedulerSeed.Create()) {Name="Peter", Roles = { Doctor.Role.Oncologist, Doctor.Role.GeneralPractitioner} }},
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
                        _utility.MergeIntervalList(doctorIntervalList, d.Value.Scheduler.GetAvailableIntervals());
                    }
                }

                if (p.Topology == Patient.CancerTopology.HeadAndNeck)
                {
                    foreach (var r in _rooms)
                    {
                        if (r.Value.TreatmentMachine != null &&
                            r.Value.TreatmentMachine.Capability == TreatmentMachine.MachineCapability.Advanced)
                        {
                            _utility.MergeIntervalList(roomIntervalList, r.Value.Scheduler.GetAvailableIntervals());
                        }
                    }
                }
                else if (p.Topology == Patient.CancerTopology.Breast)
                {
                    foreach (var r in _rooms)
                    {
                        if (r.Value.TreatmentMachine != null)
                        {
                            _utility.MergeIntervalList(roomIntervalList, r.Value.Scheduler.GetAvailableIntervals());
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
                        _utility.MergeIntervalList(doctorIntervalList, d.Value.Scheduler.GetAvailableIntervals());
                    }
                }

                foreach (var r in _rooms)
                {
                    _utility.MergeIntervalList(roomIntervalList, r.Value.Scheduler.GetAvailableIntervals());                    
                }
            }

            // retrieve the earliest time
            var appointment = _utility.FindFirstOverlappingDate(doctorIntervalList, roomIntervalList);
            Debug.Assert(appointment.Item1);

            // book the doctor
            var doctor = appointment.Item3 as Doctor;
            Debug.Assert(doctor != null);
            bool bookedDoctor = doctor.Scheduler.Book(appointment.Item2);
            Debug.Assert(bookedDoctor);

            // book the room
            var room = appointment.Item4 as TreatmentRoom;
            Debug.Assert(room != null);
            bool bookedRoom = room.Scheduler.Book(appointment.Item2);
            Debug.Assert(bookedRoom);

            // set the patient appointment info
            p.SetAppointment(appointment.Item2, doctor.Name, room.Name);

            // add the consulation to the list
            _consultations.Add( new Tuple<DateTime, string>(appointment.Item2, p.Name), 
                                new Consultation { Patient = p, Doctor = doctor, TreatmentRoom = room, AppointmentDate = appointment.Item2 });

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

        private Dictionary<string, Patient> _patients = new Dictionary<string, Patient>();

        private Dictionary<string, Doctor> _doctors;

        private Dictionary<string, TreatmentMachine> _machines;

        private Dictionary<string, TreatmentRoom> _rooms;

        private SortedDictionary<Tuple<DateTime, string>, Consultation> _consultations = new SortedDictionary<Tuple<DateTime, string>, Consultation>();

        private IUtility _utility;

        private IAppointmentScheduler _schedulerSeed;

    }
}