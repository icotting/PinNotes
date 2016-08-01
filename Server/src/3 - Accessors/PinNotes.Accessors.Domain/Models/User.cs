using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PinNotes.Accessors.Domain.Models
{
    public class User
    {
        [Key]
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string AvatarUrl { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Pin> Pins { get; set; }
    }
}
