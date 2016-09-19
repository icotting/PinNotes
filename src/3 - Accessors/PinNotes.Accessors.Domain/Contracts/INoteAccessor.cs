using System.Collections.Generic;

namespace PinNotes.Accessors.Domain.Contracts
{
    public interface INoteAccessor
    {
        ICollection<DTO.Note> FindNotes(DTO.BoundingBox box, DTO.User user);
        ICollection<DTO.Note> FindNotes(DTO.User user);
        ICollection<DTO.Note> FindNotes(DTO.Pin pin);
        DTO.Note FindNote(string id);

        DTO.Pin FindPin(string id);
        ICollection<DTO.Pin> FindPins(DTO.User user);
        ICollection<DTO.Pin> FindPins(DTO.BoundingBox box, DTO.User user);

        DTO.Pin AddPin(DTO.Pin pin);
        DTO.Note AddNote(DTO.Note note);

        void RemovePin(DTO.Pin pin);
        void RemoveNote(DTO.Note note);
    }
}
