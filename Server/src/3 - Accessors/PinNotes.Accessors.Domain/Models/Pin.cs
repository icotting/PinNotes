using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PinNotes.Accessors.Domain.Models
{
    public class Pin
    {
        [Key]
        public long PinId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public User User { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
