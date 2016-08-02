using PinNotes.Accessors.Domain.Contracts;
using PinNotes.Accessors.Domain.Tests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using PinNotes.Accessors.Domain.Core.EntityFramework;
using PinNotes.Accessors.Domain.Core;
using System.Linq;
using System;
using PinNotes.Accessors.Domain.Config;
using Microsoft.EntityFrameworkCore;

namespace PinNotes.Accessors.Domain.Tests.Tests
{
    public class UserAccessorTests : IDisposable
    {
        private IServiceProvider _provider;

        public UserAccessorTests()
        {
            _provider = new ServiceCollection().AddEntityFrameworkSqlite()
            .AddPersistence(options => options.UseSqlite(
                string.Format("Filename=../../../../../{0}.db", Guid.NewGuid().ToString())))
                .BuildServiceProvider();

            var context = _provider.GetService<PersistenceContext>();

            context.Database.EnsureCreated();
            Initializer.Seed(context);
        }

        [Fact]
        public void TestAllUsers()
        {
            IUserAccessor accessor = this._provider.GetService<IUserAccessor>();

            Assert.Equal(3, accessor.AllUsers().Count);
        }

        [Fact]
        public void TestFindUser()
        {
            IUserAccessor accessor = this._provider.GetService<IUserAccessor>();

            var ian = accessor.FindUser("ian");
            var bill = accessor.FindUser("bill");

            Assert.NotEqual(ian, bill);

            Assert.Equal("Ian", ian.FirstName);
            Assert.Equal("Cottingham", ian.LastName);
            Assert.Equal("Seattle, WA", bill.Location);
            Assert.Equal("Lincoln, NE", ian.Location);
            Assert.Equal("Gates", bill.LastName);

            var otherian = accessor.FindUserByEmail("fakeian@unl.edu");
            Assert.NotNull(otherian);
            Assert.Equal(ian, otherian);
            Assert.NotEqual(bill, otherian);
        }

        [Fact]
        public void TestAddUser()
        {
            IUserAccessor accessor = this._provider.GetService<IUserAccessor>();
            IUnitOfWork uow = this._provider.GetService<IUnitOfWork>();
            PersistenceContext context = this._provider.GetService<PersistenceContext>();

            Assert.Equal(3, context.Users.Count());

            var user = new Contracts.DTO.User()
            {
                AvatarUrl = @"http://www.google.com",
                Email = "test@user.com",
                FirstName = "Test",
                LastName = "User",
                Location = "Any Place, USA"
            };

            user = accessor.AddUser(user);
            uow.Commit();

            Assert.Equal(4, context.Users.Count());
            var users = context.Users.ToList();

            Assert.Equal("Any Place, USA", context.Users.Where(u => u.UserId == user.UserId)
                .FirstOrDefault().Location);

            Assert.Equal(user, accessor.FindUser(user.UserId));

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
                    UserId = "bill"
                });
            });
        }

        [Fact]
        public void TestRemoveUser()
        {
            IUserAccessor accessor = this._provider.GetService<IUserAccessor>();
            IUnitOfWork uow = this._provider.GetService<IUnitOfWork>();
            PersistenceContext context = this._provider.GetService<PersistenceContext>();

            Assert.Equal(3, context.Users.Count());

            var user = new Contracts.DTO.User() { UserId = "bill" };

            accessor.RemoveUser(user);
            uow.Commit();

            Assert.Equal(2, context.Users.Count());

            Assert.Throws<ArgumentException>(() => 
            {
                accessor.RemoveUser(new Contracts.DTO.User() { UserId = user.UserId });
            });


            Assert.Throws<InvalidOperationException>(() =>
            {
                accessor.RemoveUser(null);
            });
        }

        public void Dispose()
        {
            this._provider.GetService<PersistenceContext>().Database.EnsureDeleted();
        }
    }
}
