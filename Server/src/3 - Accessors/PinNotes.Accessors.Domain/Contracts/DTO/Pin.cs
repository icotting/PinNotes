using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PinNotes.Accessors.Domain.Contracts.DTO
{
    [DataContract]
    public class Pin
    {
        [DataMember]
        public string PinId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double? Latitude { get; set; }

        [DataMember]
        public double? Longitude { get; set; }

        [DataMember]
        public string BelongsTo { get; set; }
    }
}
