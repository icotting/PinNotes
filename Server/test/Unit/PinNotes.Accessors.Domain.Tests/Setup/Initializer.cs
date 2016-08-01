using Microsoft.Extensions.DependencyInjection;
using System;
using PinNotes.Accessors.Domain.Config;
using PinNotes.Accessors.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace PinNotes.Accessors.Domain.Tests.Setup
{
    public static class Initializer
    {
        public static IServiceProvider Init()
        {
            IServiceProvider provider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase()
            .AddPersistence(options => options.UseInMemoryDatabase()).BuildServiceProvider();
            provider.GetService<PersistenceContext>().Database.EnsureDeleted();

            // seed some data
            _UserSeed(provider.GetService<PersistenceContext>());

            return provider;
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
                UserId = 100
            });

            context.Users.Add(new Models.User()
            {
                AvatarUrl = @"https://avatars1.githubusercontent.com/u/4513686?v=3&s=400",
                Email = "fakezach@unl.edu",
                FirstName = "Zach",
                LastName = "Christensen",
                Location = "Lincoln, NE",
                UserId = 200
            });

            context.Users.Add(new Models.User()
            {
                AvatarUrl = @"http://cdn2.insidermonkey.com/blog/wp-content/uploads/2013/01/01-Bill-Gates.jpg",
                Email = "fakebill@microsoft.com",
                FirstName = "Bill",
                LastName = "Gates",
                Location = "Seattle, WA", 
                UserId = 300
            });

            context.SaveChanges();
        }
    }
}
