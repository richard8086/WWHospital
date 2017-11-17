using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class Consultation
    {
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public TreatmentRoom TreatmentRoom { get; set; }
        public string ConsultationDate { get { return _date.ToString("MM/dd/yyyy"); } }

        [JsonIgnore]
        internal DateTime AppointmentDate { get { return _date; } set { _date = value; } }
        private DateTime _date = DateTime.MinValue;
    }
}