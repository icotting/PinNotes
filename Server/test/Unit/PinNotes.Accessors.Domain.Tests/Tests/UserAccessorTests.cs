using PinNotes.Accessors.Domain.Contracts;
using PinNotes.Accessors.Domain.Tests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using PinNotes.Accessors.Domain.Core.EntityFramework;
using PinNotes.Accessors.Domain.Core;
using System.Linq;
using System;

namespace PinNotes.Accessors.Domain.Tests.Tests
{
    public class UserAccessorTests
    {
        [Fact]
        public void TestAllUsers()
        {
            var provider = Initializer.Init();
            IUserAccessor accessor = provider.GetService<IUserAccessor>();

            Assert.Equal(3, accessor.AllUsers().Count);
        }

        [Fact]
        public void TestFindUser()
        {
            var provider = Initializer.Init();
            IUserAccessor accessor = provider.GetService<IUserAccessor>();

            var ian = accessor.FindUser(100);
            var bill = accessor.FindUser(300);

            Assert.NotEqual(ian, bill);

            Assert.Equal("Ian", ian.FirstName);
            Assert.Equal("Cottingham", ian.LastName);
            Assert.Equal("Seattle, WA", bill.Location);
            Assert.Equal("Lincoln, NE", ian.Location);
            Assert.Equal("Gates", bill.LastName);

            var otherian = accessor.FindUser("fakeian@unl.edu");
            Assert.NotNull(otherian);
            Assert.Equal(ian, otherian);
            Assert.NotEqual(bill, otherian);
        }

        [Fact]
        public void TestAddUser()
        {
            var provider = Initializer.Init();
            IUserAccessor accessor = provider.GetService<IUserAccessor>();
            IUnitOfWork uow = provider.GetService<IUnitOfWork>();
            PersistenceContext context = provider.GetService<PersistenceContext>();

            Assert.Equal(3, context.Users.Count());

            var user = new Contracts.DTO.User()
            {
                UserId = 1,
                AvatarUrl = @"http://www.google.com",
                Email = "test@user.com",
                FirstName = "Test",
                LastName = "User",
                Location = "Any Place, USA"
            };

            accessor.AddUser(user);
            uow.Commit();
            Assert.Equal(4, context.Users.Count());

            Assert.Equal("Any Place, USA", context.Users.Where(u => u.UserId == 1)
                .FirstOrDefault().Location);

            Assert.Equal(user, accessor.FindUser(1));

            /* Test some bad data */
            Assert.Throws<ArgumentException>(() =>
            {
                accessor.AddUser(null);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                accessor.AddUser(new Contracts.DTO.User()
                {
                    FirstName = "Bad",
                    LastName = "User"
                });
            });

            /* Test entry of a user with an already existing ID */
            Assert.Throws<ArgumentException>(() =>
            {
                accessor.AddUser(new Contracts.DTO.User()
                {
                    FirstName = "Bad",
                    LastName = "User",
                    Location = "Death Valley",
                    Email = "bad@user.com",
                    UserId = 300
                });
            });
        }

        [Fact]
        public void TestRemoveUser()
        {
            var provider = Initializer.Init();
            IUserAccessor accessor = provider.GetService<IUserAccessor>();
            IUnitOfWork uow = provider.GetService<IUnitOfWork>();
            PersistenceContext context = provider.GetService<PersistenceContext>();

            Assert.Equal(3, context.Users.Count());

            var user = new Contracts.DTO.User() { UserId = 300 };

            accessor.RemoveUser(user);
            uow.Commit();

            Assert.Equal(2, context.Users.Count());

            Assert.Throws<ArgumentException>(() => 
            {
                accessor.RemoveUser(new Contracts.DTO.User() { UserId = 1 });
            });


            Assert.Throws<InvalidOperationException>(() =>
            {
                accessor.RemoveUser(null);
            });
        }
    }
}
