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

        public override bool Equals(object obj)
        {
            if (obj is DTO.BoundingBox)
            {
                return ((DTO.BoundingBox)obj).GetHashCode() == this.GetHashCode();
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
            return string.Format("({0}, {1}), ({2}, {3})", this.North, this.East, this.South, this.West);
        }
    }
}