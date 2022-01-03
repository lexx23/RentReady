using System;
using Newtonsoft.Json;

namespace RentReady.Common.Entity
{
    public class DateRange
    {
        [JsonProperty(Required = Required.Always)]
        public DateTime StartOn { get; set; }
        
        [JsonProperty(Required = Required.Always)]
        public DateTime EndOn { get; set; }
    }
}