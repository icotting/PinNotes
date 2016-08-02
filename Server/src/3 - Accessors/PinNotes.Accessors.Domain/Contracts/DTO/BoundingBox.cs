using System.Runtime.Serialization;

namespace PinNotes.Accessors.Domain.Contracts.DTO
{
    [DataContract]
    public class BoundingBox
    {
        [DataMember]
        public double North { get; set; }

        [DataMember]
        public double East { get; set; }

        [DataMember]
        public double South { get; set; }

        [DataMember]
        public double West { get; set; }
    }
}