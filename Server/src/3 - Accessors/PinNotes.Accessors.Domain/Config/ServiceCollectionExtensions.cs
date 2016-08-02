using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PinNotes.Accessors.Domain.Contracts;
using PinNotes.Accessors.Domain.Core;
using PinNotes.Accessors.Domain.Core.EntityFramework;
using System;

namespace PinNotes.Accessors.Domain.Config
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection serviceCollection, 
            Action<DbContextOptionsBuilder> options)
        {
            serviceCollection.AddDbContext<PersistenceContext>(options);

            serviceCollection.AddTransient<IUserAccessor, UserAccessor>();
            serviceCollection.AddTransient<INoteAccessor, NoteAccessor>();

            /* Setup Infrastructure Bindings */
            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped(p => new PersistenceContextAdapter(p.GetService<PersistenceContext>()));

            serviceCollection.AddTransient(typeof(IDbContext), p => p.GetService<PersistenceContextAdapter>());
            serviceCollection.AddTransient(typeof(IDbSetFactory), p => p.GetService<PersistenceContextAdapter>());

            return serviceCollection;
        }
    }
}
