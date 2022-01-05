using System;
using Newtonsoft.Json;

namespace RentReady.Common.Entity
{
    public class DateRangeEntity
    {
        private DateTime _startOn;
        private DateTime _endOn;


        [JsonProperty(Required = Required.Always)]
        public DateTime StartOn
        {
            get => _startOn;
            set => _startOn = value.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(value, DateTimeKind.Utc) : value;
        }
        
        [JsonProperty(Required = Required.Always)]
        public DateTime EndOn
        {
            get => _endOn;
            set => _endOn = value.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(value, DateTimeKind.Utc) : value;
        }
    }
}