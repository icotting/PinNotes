using PinNotes.Accessors.Domain.Contracts;
using PinNotes.Accessors.Domain.Core.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PinNotes.Accessors.Domain.Contracts.DTO;

namespace PinNotes.Accessors.Domain.Core
{
    public class NoteAccessor : EntityAccessor, INoteAccessor
    {
        private DbSet<Models.Pin> _pins;
        private DbSet<Models.Note> _notes;
        private DbSet<Models.User> _users;
        
        public NoteAccessor(IDbSetFactory dbSetFactory) : base(dbSetFactory)
        {
            this._pins = dbSetFactory.CreateDbSet<Models.Pin>();
            this._notes = dbSetFactory.CreateDbSet<Models.Note>();
            this._users = dbSetFactory.CreateDbSet<Models.User>();
        }

        public Contracts.DTO.Note AddNote(Contracts.DTO.Note note)
        {
            if (note == null)
            {
                throw new ArgumentException("Note must be non-null"); 
            }
            else
            {

                var pin = note.BelongsTo == null ? null :
                    this._pins.Where(p => p.PinId == note.BelongsTo).FirstOrDefault();

                if (pin == null)
                {
                    throw new ArgumentException(string.Format("Pin {0} is not valid", note.BelongsTo));
                }
                else if (note.NoteId != null)
                {
                    throw new ArgumentException("Note ID is not null, ID values cannot be specified");
                }
                else
                {
                    string id = Guid.NewGuid().ToString();
                    this._notes.Add(new Models.Note()
                    {
                        NoteId = id,
                        Added = DateTime.Now,
                        Content = note.Content,
                        Pin = pin
                    });

                    note.NoteId = id;
                    return note;
                }
            }
        }

        public Contracts.DTO.Pin AddPin(Contracts.DTO.Pin pin)
        {
            if (pin == null)
            {
                throw new ArgumentException("Pin must be non-null");
            }
            else if (pin.PinId != null)
            {
                throw new ArgumentException("Pin ID is not null, ID values cannot be specified");
            }
            else if ( pin.Latitude == null || pin.Longitude == null || pin.Name == null)
            {
                throw new ArgumentException("A pin must have a valid latitude, longitude, and name");
            }
            else
            {
                var user = this._users.Where(u => u.UserId == pin.BelongsTo).FirstOrDefault();
                if (user == null)
                {
                    throw new ArgumentException(string.Format("User {0} is invalid", pin.BelongsTo));
                }
                else
                {
                    var id = Guid.NewGuid().ToString();
                    this._pins.Add(new Models.Pin()
                    {
                        PinId = id,
                        Latitude = pin.Latitude.Value,
                        Longitude = pin.Longitude.Value,
                        Name = pin.Name,
                        User = user
                    });

                    pin.PinId = id;
                    return pin;
                }
            }
        }

        public Contracts.DTO.Note FindNote(string id)
        {
            var note = this._notes.Where(n => n.NoteId == id).FirstOrDefault();
            return note == null ? null : new Contracts.DTO.Note()
            {
                NoteId = note.NoteId,
                Added = note.Added,
                BelongsTo = note.Pin.User.UserId,
                Content = note.Content
            };
        }

        public ICollection<Contracts.DTO.Note> FindNotes(Contracts.DTO.Pin pin)
        {
            if (pin == null)
            {
                throw new ArgumentException("The pin cannot be null");
            }
            var pentity = this._pins.Where(p => p.PinId == pin.PinId).FirstOrDefault();
            if ( pentity != null )
            {
                if ( pentity.Notes != null && pentity.Notes.Count > 0)
                {
                    return pentity.Notes.Select(n => new Contracts.DTO.Note()
                    {
                        NoteId = n.NoteId,
                        Added = n.Added, 
                        BelongsTo = n.Pin.User.UserId,
                        Content = n.Content
                    }).ToList<Contracts.DTO.Note>();
                }
            }

            return new List<Contracts.DTO.Note>();
        }

        public ICollection<Contracts.DTO.Note> FindNotes(Contracts.DTO.User user)
        {
            if (user == null)
            {
                throw new ArgumentException("The user cannot be null");
            }

            var notes = this._notes.Where(n => n.Pin.User.UserId == user.UserId);

            if (notes != null || notes.Count() > 0)
            {
                return notes.Select(n => new Contracts.DTO.Note()
                {
                    NoteId = n.NoteId,
                    Added = n.Added,
                    BelongsTo = n.Pin.User.UserId,
                    Content = n.Content
                }).ToList<Contracts.DTO.Note>();
            }
            else
            {
                return new List<Contracts.DTO.Note>();
            }
        }

        public ICollection<Contracts.DTO.Note> FindNotes(Contracts.DTO.BoundingBox box, 
            Contracts.DTO.User user)
        {
            if ( user == null || box == null )
            {
                throw new ArgumentException("The user and bounding box must be non-null");
            }
            
            var notes = this._notes.Where(n => n.Pin.Latitude >= box.South
                && n.Pin.Latitude <= box.North
                && n.Pin.Longitude <= box.East
                && n.Pin.Longitude >= box.West
                && n.Pin.User.UserId == user.UserId);

            if (notes != null && notes.Count() > 0)
            {
                return notes.Select(n => new Contracts.DTO.Note()
                {
                    NoteId = n.NoteId,
                    Added = n.Added,
                    Content = n.Content,
                    BelongsTo = n.Pin.PinId
                }).ToList<Contracts.DTO.Note>();
            }
            else
            {
                return new List<Contracts.DTO.Note>();
            }
        }

        public ICollection<Contracts.DTO.Pin> FindPins(Contracts.DTO.User user)
        {
            if (user==null)
            {
                throw new ArgumentException("The pin cannot be null");
            }

            var uentity = this._users.Where(u => u.UserId == user.UserId).FirstOrDefault();
            if ( uentity != null && uentity.Pins != null && uentity.Pins.Count > 0)
            {
                return uentity.Pins.Select(p => new Contracts.DTO.Pin()
                {
                    BelongsTo = uentity.UserId,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Name = p.Name, 
                    PinId = p.PinId
                }).ToList<Contracts.DTO.Pin>();
            }

            return new List<Contracts.DTO.Pin>();
        }

        public ICollection<Contracts.DTO.Pin> FindPins(Contracts.DTO.BoundingBox box, 
            Contracts.DTO.User user)
        {
            if ( box == null || user == null)
            {
                throw new ArgumentException("The user and bounding box must be non-null");
            }

            var pins = this._pins.Where(p => p.Latitude >= box.South
                && p.Latitude <= box.North
                && p.Longitude <= box.East
                && p.Longitude >= box.West
                && p.User.UserId == user.UserId);

            if (pins != null && pins.Count() > 0)
            {
                return pins.Select(p => new Contracts.DTO.Pin()
                {
                    BelongsTo = p.User.UserId,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Name = p.Name,
                    PinId = p.PinId
                }).ToList<Contracts.DTO.Pin>();
            }
            else
            {
                return new List<Contracts.DTO.Pin>();
            }
        }

        public void RemoveNote(Contracts.DTO.Note note)
        {
            if (note == null)
            {
                throw new ArgumentException("Note must be non-null");
            }
            else
            {
                var entity = this._notes.Where(n => n.NoteId == note.NoteId).FirstOrDefault();
                if (entity == null)
                {
                    throw new ArgumentException(string.Format("{0} was not found", note.NoteId));
                }
                else
                {
                    this._notes.Remove(entity);
                }
            }
        }

        public void RemovePin(Contracts.DTO.Pin pin)
        {
            if (pin == null)
            {
                throw new ArgumentException("Pin must be non-null");
            }
            else
            {
                var entity = this._pins.Where(p => p.PinId == pin.PinId).FirstOrDefault();
                if ( entity == null )
                {
                    throw new ArgumentException(string.Format("{0} was not found", pin.PinId));
                } 
                else
                {
                    this._pins.Remove(entity);
                }
            }
        }

        public Contracts.DTO.Pin FindPin(string id)
        {
            var pin = this._pins.Where(p => p.PinId == id).FirstOrDefault();
            return pin == null ? null : new Contracts.DTO.Pin()
            {
                BelongsTo = pin.User.UserId, 
                Latitude = pin.Latitude, 
                Longitude = pin.Longitude, 
                Name = pin.Name, 
                PinId = pin.PinId  
            };
        }
    }
}
