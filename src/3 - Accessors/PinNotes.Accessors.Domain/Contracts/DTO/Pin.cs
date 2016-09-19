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

        public override bool Equals(object obj)
        {
            if (obj is DTO.Pin)
            {
                return ((DTO.Pin)obj).GetHashCode() == this.GetHashCode();
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return this.PinId;
        }
    }
}
