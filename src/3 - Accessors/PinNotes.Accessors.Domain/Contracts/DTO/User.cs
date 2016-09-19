using System.Runtime.Serialization;

namespace PinNotes.Accessors.Domain.Contracts.DTO
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string AvatarUrl { get; set; }

        [DataMember]
        public string Email { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DTO.User)
            {
                return ((DTO.User)obj).GetHashCode() == this.GetHashCode();
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
            return this.Email;
        }
    }
}
