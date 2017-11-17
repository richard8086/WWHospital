using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Hospital.Implementation;
using Hospital.Interfaces;


namespace Hospital.Models
{
    public class Doctor
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Role
        {
            Oncologist,
            GeneralPractitioner
        }

        public string Name { get; set; }

        public HashSet<Role> Roles { get; set; }

        [JsonIgnore]
        internal List<DateTime> ConsulationTimes
        {
            get { return _scheduler.GetBookings(); }
        }

        [JsonIgnore]
        internal IAppointmentScheduler Scheduler { get { return _scheduler; } }

        public Doctor(IAppointmentScheduler scheduler)
        {
            _scheduler = scheduler;
            _scheduler.Initialize(this);
            Roles = new HashSet<Role>();
        }

        private IAppointmentScheduler _scheduler;
    }
}