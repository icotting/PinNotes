using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PinNotes.Accessors.Domain.Models
{
    public class Note
    {
        [Key]
        public string NoteId { get; set; }
        public string Content { get; set; }
        public DateTime Added { get; set; }
        public Pin Pin { get; set; }
    }
}
