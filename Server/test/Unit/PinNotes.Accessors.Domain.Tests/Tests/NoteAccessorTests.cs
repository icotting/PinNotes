using PinNotes.Accessors.Domain.Contracts;
using PinNotes.Accessors.Domain.Tests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;
using PinNotes.Accessors.Domain.Core;
using PinNotes.Accessors.Domain.Config;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PinNotes.Accessors.Domain.Core.EntityFramework;

namespace PinNotes.Accessors.Domain.Tests.Tests
{
    public class NoteAccessorTests : IDisposable
    {
        private IServiceProvider _provider;

        private Contracts.DTO.BoundingBox _campus = new Contracts.DTO.BoundingBox()
        {
            North = 40.824544,
            South = 40.816879,
            East = -96.697025,
            West = -96.708355
        };

        private Contracts.DTO.BoundingBox _bottoms = new Contracts.DTO.BoundingBox()
        {
            North = 40.830441,
            South = 40.825262,
            East = -96.701188,
            West = -96.707239
        };

        private Contracts.DTO.BoundingBox _lincoln = new Contracts.DTO.BoundingBox()
        {
            North = 40.899568,
            South = 40.690338,
            East = -96.597633,
            West = -96.781311
        };

        private Contracts.DTO.BoundingBox _south = new Contracts.DTO.BoundingBox()
        {
            North = 40.75689,
            South = 40.695805,
            East = -96.58493,
            West = -96.704063
        };

        public NoteAccessorTests()
        {
            _provider = new ServiceCollection().AddEntityFrameworkSqlite()
            .AddPersistence(options => options.UseSqlite(
                string.Format("Filename=../../../../../{0}.db", Guid.NewGuid().ToString())))
                .BuildServiceProvider();

            var context = _provider.GetService<PersistenceContext>();

            context.Database.EnsureCreated();
            Initializer.Seed(context);
        }

        public void Dispose()
        {
            this._provider.GetService<PersistenceContext>().Database.EnsureDeleted();
        }

        [Fact]
        public void TestFindNotesByUser()
        {
            INoteAccessor accessor = this._provider.GetService<INoteAccessor>();
            IUserAccessor userAccessor = this._provider.GetService<IUserAccessor>();

            var ian = userAccessor.FindUser("ian");
            var ianNotes = accessor.FindNotes(ian);

            var zach = userAccessor.FindUser("zach");
            var zachNotes = accessor.FindNotes(zach);

            Assert.Equal(5, ianNotes.Count);
            Assert.Equal(2, zachNotes.Count);

            Assert.Throws<ArgumentException>(() => { accessor.FindNotes((Contracts.DTO.User)null); });
        }

        [Fact]
        public void TestFindNote()
        {
            INoteAccessor accessor = this._provider.GetService<INoteAccessor>();
            Assert.Equal("Don't eat the donuts", accessor.FindNote("TN3").Content);
            Assert.Null(accessor.FindNote("TN100"));
            Assert.Null(accessor.FindNote(null));
        }

        [Fact]
        public void TestFindNotesByRegion()
        {
            INoteAccessor accessor = this._provider.GetService<INoteAccessor>();
            IUserAccessor userAccessor = this._provider.GetService<IUserAccessor>();

            var ian = userAccessor.FindUser("ian");
            var zach = userAccessor.FindUser("zach");

            Assert.Equal(3, accessor.FindNotes(_campus, ian).Count);
            Assert.Equal(0, accessor.FindNotes(_bottoms, ian).Count);
            Assert.Equal(5, accessor.FindNotes(_lincoln, ian).Count);
            Assert.Equal(0, accessor.FindNotes(_south, ian).Count);

            Assert.Equal(0, accessor.FindNotes(_campus, zach).Count);
            Assert.Equal(2, accessor.FindNotes(_bottoms, zach).Count);
            Assert.Equal(2, accessor.FindNotes(_lincoln, zach).Count);
            Assert.Equal(0, accessor.FindNotes(_south, zach).Count);

            Assert.Throws<ArgumentException>(() => 
            {
                accessor.FindNotes(null, ian);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.FindNotes(_campus, null);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.FindNotes(null, null);
            });
        }

        [Fact]
        public void TestFindNotesByPin()
        {
            INoteAccessor accessor = this._provider.GetService<INoteAccessor>();

            var kaufNotes = accessor.FindNotes(new Contracts.DTO.Pin() { PinId = "kauf" });
            Assert.Equal(3, kaufNotes.Count);

            var destinationNotes = accessor.FindNotes(new Contracts.DTO.Pin() { PinId = "dest" });
            Assert.Equal(2, destinationNotes.Count);

            Assert.Throws<ArgumentException>(() => { accessor.FindNotes((Contracts.DTO.Pin)null); });

        }

        public void TestFindPin()
        {
            INoteAccessor accessor = this._provider.GetService<INoteAccessor>();
            var pin = accessor.FindPin("dest");

            Assert.Equal("Destinations", pin.Name);
            Assert.Null(accessor.FindPin(null));

        }
        public void TestFindPinsByUser()
        {
            INoteAccessor accessor = this._provider.GetService<INoteAccessor>();
            IUserAccessor userAccessor = this._provider.GetService<IUserAccessor>();

            var ian = userAccessor.FindUser("ian");
            var zach = userAccessor.FindUser("zach");

            var kauf = accessor.FindPin("kauf");
            var destinations = accessor.FindPin("dest");
            var fuse = accessor.FindPin("fuse");

            var ianPins = accessor.FindPins(ian);
            var zachPins = accessor.FindPins(zach);

            Assert.Equal(2, ianPins.Count);
            Assert.True(ianPins.Contains(kauf));
            Assert.True(ianPins.Contains(fuse));
            Assert.True(zachPins.Contains(destinations));

            Assert.Throws<ArgumentException>(() => { accessor.FindPins(null); });
        }

        public void TestFindPinsByBox()
        {
            INoteAccessor accessor = this._provider.GetService<INoteAccessor>();
            IUserAccessor userAccessor = this._provider.GetService<IUserAccessor>();

            var ian = userAccessor.FindUser("ian");
            var zach = userAccessor.FindUser("zach");

            var kauf = accessor.FindPin("kauf");
            var destinations = accessor.FindPin("dest");
            var fuse = accessor.FindPin("fuse");

            Assert.Equal(1, accessor.FindPins(_campus, ian).Count);
            Assert.Equal(0, accessor.FindPins(_bottoms, ian).Count);
            Assert.Equal(2, accessor.FindPins(_lincoln, ian).Count);
            Assert.Equal(0, accessor.FindPins(_south, ian).Count);

            Assert.Equal(0, accessor.FindPins(_campus, zach).Count);
            Assert.Equal(1, accessor.FindPins(_bottoms, zach).Count);
            Assert.Equal(1, accessor.FindPins(_lincoln, zach).Count);
            Assert.Equal(0, accessor.FindPins(_south, zach).Count);

            var ianPins = accessor.FindPins(_lincoln, ian);
            Assert.True(ianPins.Contains(kauf));
            Assert.True(ianPins.Contains(fuse));

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.FindPins(null, ian);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.FindPins(_campus, null);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.FindPins(null, null);
            });
        }

        [Fact]
        public void TestAddPin()
        {
            INoteAccessor accessor = this._provider.GetService<INoteAccessor>();
            IUserAccessor userAccessor = this._provider.GetService<IUserAccessor>();
            PersistenceContext context = this._provider.GetService<PersistenceContext>();
            IUnitOfWork unit = this._provider.GetService<IUnitOfWork>();

            Assert.Equal(3, context.Pins.Count());

            var zach = userAccessor.FindUser("zach");
            accessor.AddPin(new Contracts.DTO.Pin()
            {
                BelongsTo = zach.UserId,
                Latitude = _lincoln.North,
                Longitude = _lincoln.East,
                Name ="Lincoln NE"
            });
            unit.Commit();

            Assert.Equal(4, context.Pins.Count());

            var zachPins = accessor.FindPins(zach);
            Assert.Equal(2, zachPins.Count);

            Assert.Throws<ArgumentException>(() => 
            {
                accessor.AddPin(null);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.AddPin(new Contracts.DTO.Pin()
                {
                    PinId = "kauf",
                    BelongsTo = zach.UserId,
                    Latitude = 0,
                    Longitude = 0,
                    Name = "Bad pin - duplicate"
                });
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.AddPin(new Contracts.DTO.Pin()
                {
                    Latitude = 0,
                    Longitude = 0,
                    Name = "Bad pin - no owner"
                });
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.AddPin(new Contracts.DTO.Pin()
                {
                    BelongsTo = zach.UserId,
                    Name = "Bad pin - no coordinates"
                });
            });
        }

        [Fact]
        public void TestAddNote()
        {
            INoteAccessor accessor = this._provider.GetService<INoteAccessor>();
            IUserAccessor userAccessor = this._provider.GetService<IUserAccessor>();
            PersistenceContext context = this._provider.GetService<PersistenceContext>();
            IUnitOfWork unit = this._provider.GetService<IUnitOfWork>();

            var pin = accessor.FindPin("kauf");
            var ian = userAccessor.FindUser("ian");

            Assert.Equal(7, context.Notes.Count());
            Assert.Equal(5, accessor.FindNotes(ian).Count);

            accessor.AddNote(new Contracts.DTO.Note()
            {
                Added = DateTime.Now, 
                BelongsTo = pin.PinId,
                Content = "Testing software late at night, stupid Raikes School curriculum"
            });

            unit.Commit();

            Assert.Equal(8, context.Notes.Count());

            Assert.Throws<ArgumentException>(() => 
            {
                accessor.AddNote(null);
                unit.Commit();
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.AddNote(new Contracts.DTO.Note()
                {
                    NoteId = "TN4",
                    Content = "Bad note - duplicate ID",
                    BelongsTo = pin.PinId
                });
                unit.Commit();
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.AddNote(new Contracts.DTO.Note()
                {
                    Content = "Bad note - no pin"
                });
                unit.Commit();
            });
        }

        [Fact]
        public void TestRemovePin()
        {
            INoteAccessor accessor = this._provider.GetService<INoteAccessor>();
            IUserAccessor userAccessor = this._provider.GetService<IUserAccessor>();
            PersistenceContext context = this._provider.GetService<PersistenceContext>();
            IUnitOfWork unit = this._provider.GetService<IUnitOfWork>();

            var ian = userAccessor.FindUser("ian");

            var ianPins = accessor.FindPins(ian);
            Assert.Equal(2, ianPins.Count);
            Assert.Equal(3, context.Pins.Count());

            accessor.RemovePin(ianPins.First());
            unit.Commit();

            Assert.Equal(2, context.Pins.Count());
            ianPins = accessor.FindPins(ian);

            Assert.Equal(1, ianPins.Count);

            Assert.Throws<ArgumentException>(() => 
            {
                accessor.RemovePin(null);
                unit.Commit();
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.RemovePin(new Contracts.DTO.Pin());
                unit.Commit();
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.RemovePin(new Contracts.DTO.Pin() { PinId = "NOTAREALPIN" } );
                unit.Commit();
            });
        }
        
        [Fact]
        public void TestRemoveNote()
        {
            INoteAccessor accessor = this._provider.GetService<INoteAccessor>();
            IUserAccessor userAccessor = this._provider.GetService<IUserAccessor>();
            PersistenceContext context = this._provider.GetService<PersistenceContext>();
            IUnitOfWork unit = this._provider.GetService<IUnitOfWork>();

            var kauf = accessor.FindPin("kauf");
            var kaufNotes = accessor.FindNotes(kauf);

            Assert.Equal(3, kaufNotes.Count);
            Assert.Equal(7, context.Notes.Count());

            accessor.RemoveNote(kaufNotes.First());
            unit.Commit();

            kaufNotes = accessor.FindNotes(kauf);

            Assert.Equal(2, kaufNotes.Count);
            Assert.Equal(6, context.Notes.Count());

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.RemoveNote(null);
                unit.Commit();
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.RemoveNote(new Contracts.DTO.Note());
                unit.Commit();
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.RemoveNote(new Contracts.DTO.Note() { NoteId = "NOTAREALNOTE" });
                unit.Commit();
            });
        }
    }
}
