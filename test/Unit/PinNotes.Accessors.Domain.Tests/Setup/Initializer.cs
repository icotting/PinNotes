
using System;
using PinNotes.Accessors.Domain.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace PinNotes.Accessors.Domain.Tests.Setup
{
    public static class Initializer
    {
        public static void Seed(PersistenceContext context)
        {
            // seed some data
            _UserSeed(context);
            _NoteSeed(context);
        }

        private static void _UserSeed(PersistenceContext context)
        {
            context.Users.Add(new Models.User()
            {
                AvatarUrl = @"https://avatars0.githubusercontent.com/u/425362?v=3&s=460",
                Email = "fakeian@unl.edu",
                FirstName = "Ian",
                LastName = "Cottingham",
                Location = "Lincoln, NE",
                UserId = "ian"
            });

            context.Users.Add(new Models.User()
            {
                AvatarUrl = @"https://avatars1.githubusercontent.com/u/4513686?v=3&s=400",
                Email = "fakezach@unl.edu",
                FirstName = "Zach",
                LastName = "Christensen",
                Location = "Lincoln, NE",
                UserId = "zach"
            });

            context.Users.Add(new Models.User()
            {
                AvatarUrl = @"http://cdn2.insidermonkey.com/blog/wp-content/uploads/2013/01/01-Bill-Gates.jpg",
                Email = "fakebill@microsoft.com",
                FirstName = "Bill",
                LastName = "Gates",
                Location = "Seattle, WA", 
                UserId = "bill"
            });

            context.SaveChanges();
        }

        private static void _NoteSeed(PersistenceContext context)
        {
            var ian = context.Users.Where(u => u.UserId == "ian").FirstOrDefault();
            var zach = context.Users.Where(u => u.UserId == "zach").FirstOrDefault();

            var kauffman = new Models.Pin()
            {
                Latitude = 40.819661,
                Longitude = -96.70041,
                Name = "Kauffman",
                PinId = "kauf"
            };

            var fuse = new Models.Pin()
            {
                Latitude = 40.81442,
                Longitude = -96.710235,
                Name = "FUSE",
                PinId = "fuse"
            };

            var destinations = new Models.Pin()
            {
                Latitude = 40.826233,
                Longitude = -96.70155,
                Name = "Destinations",
                PinId = "dest",
            };

            kauffman.Notes = new List<Models.Note>
            {
                new Models.Note()
                {
                    Added = DateTime.Now,
                    Content = "Make sure to teach class",
                    Pin = kauffman,
                    NoteId = "TN1"
                },
                new Models.Note()
                {
                    Added = DateTime.Now,
                    Content = "Attend the weekly standup on Thursday",
                    Pin = kauffman,
                    NoteId = "TN2"
                },
                new Models.Note()
                {
                    Added = DateTime.Now,
                    Content = "Don't eat the donuts",
                    Pin = kauffman,
                    NoteId = "TN3"
                },
            };

            fuse.Notes = new List<Models.Note>
            {
                new Models.Note()
                {
                    Added = DateTime.Now,
                    Content = "Use the FUSE wifi, it is faster than DPL",
                    Pin = fuse,
                    NoteId = "TN4"
                },
                new Models.Note()
                {
                    Added = DateTime.Now,
                    Content = "Avoid the elevator",
                    Pin = fuse,
                    NoteId = Guid.NewGuid().ToString()
                }
            };

            destinations.Notes = new List<Models.Note>
            {
                new Models.Note()
                {
                    Added = DateTime.Now,
                    Content = "Grab a coffee before heading to Kauffman, you'll thank yourself later",
                    Pin = destinations,
                    NoteId = "TN5"
                },
                new Models.Note()
                {
                    Added = DateTime.Now,
                    Content = "It's a longer walk than you think",
                    Pin = destinations,
                    NoteId = "TN6"
                }
            };

            ian.Pins = new List<Models.Pin> { kauffman, fuse };
            zach.Pins = new List<Models.Pin> { destinations };

            context.Entry(ian).State = EntityState.Modified;
            context.Entry(zach).State = EntityState.Modified;

            context.SaveChanges();
        }
    }
}
