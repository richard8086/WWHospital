using Hospital.Implementation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class TreatmentRoom
    {
        public string Name { get; set; }

        public TreatmentMachine TreatmentMachine { get; set; }

        [JsonIgnore]
        internal List<DateTime> ConsulationTimes
        {
            get{ return _scheduler.GetBookings(); }
        }

        [JsonIgnore]
        internal IAppointmentScheduler Scheduler { get { return _scheduler; } }

        public TreatmentRoom()
        {
            _scheduler.Initialize(this);
        }

        private IAppointmentScheduler _scheduler = new AppointmentScheduler();
    }
}