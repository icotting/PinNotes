using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PinNotes.Accessors.Domain.Contracts.DTO
{
    [DataContract]
    public class Note
    {
        [DataMember]
        public string NoteId { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public DateTime Added { get; set; }
        [DataMember]
        public string BelongsTo { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DTO.Note)
            {
                return ((DTO.Note)obj).GetHashCode() == this.GetHashCode();
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
            return this.NoteId;
        }
    }
}
