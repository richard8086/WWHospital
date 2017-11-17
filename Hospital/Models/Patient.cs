using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Hospital.Models
{
    public class Patient
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PatientCondition
        {
            Cancer,
            Flu
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum CancerTopology
        {
            HeadAndNeck,
            Breast
        }

        public string Name { get; set; }

        public PatientCondition Condition { get; set; }

        public CancerTopology Topology { get; set; }

        public string RegistrationTime { get { return _registrationTime.ToString("MM/dd/yyyy"); } }

        public string ConsultationTime { get { return _consulationTime.ToString("MM/dd/yyyy"); } }

        public string Doctor { get { return _doctor; } }

        public string TreatmentRoom { get { return _room; } }

        [JsonIgnore]
        internal DateTime AppointmentTime { get { return _consulationTime; } }

        internal void SetAppointment(DateTime date, string doctor, string room)
        {
            _consulationTime = date;
            _doctor = doctor;
            _room = room;
        }

        private readonly DateTime _registrationTime = DateTime.Now;

        private DateTime _consulationTime = DateTime.MinValue;

        private string _doctor;

        private string _room;
    }
}