using PinNotes.Accessors.Domain.Contracts;
using PinNotes.Accessors.Domain.Tests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;
using PinNotes.Accessors.Domain.Core;
using PinNotes.Accessors.Domain.Config;
using Microsoft.EntityFrameworkCore;

namespace PinNotes.Accessors.Domain.Tests.Tests
{
    public class NoteAccessorTests : IDisposable
    {
        private IServiceProvider _provider;

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
        public void TestFindNotes()
        {
            INoteAccessor accessor = this._provider.GetService<INoteAccessor>();
            IUserAccessor userAccessor = this._provider.GetService<IUserAccessor>();

            var ian = userAccessor.FindUser("ian");
            var ianNotes = accessor.FindNotes(ian);

            Assert.Equal(5, ianNotes.Count);
        }
    }
}
