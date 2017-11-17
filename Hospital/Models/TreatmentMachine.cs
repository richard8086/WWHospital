using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hospital.Models
{
    public class TreatmentMachine
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum MachineCapability
        {
            Simple,
            Advanced
        }

        public string Name { get; set; }

        public MachineCapability Capability { get; set; }
    }
}