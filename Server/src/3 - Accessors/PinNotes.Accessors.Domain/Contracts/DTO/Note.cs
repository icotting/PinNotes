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
        public long NoteId { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public DateTime Added { get; set; }
        [DataMember]
        public long BelongsTo { get; set; }
    }
}
