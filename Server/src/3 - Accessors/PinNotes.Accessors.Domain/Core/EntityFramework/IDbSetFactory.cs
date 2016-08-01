using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinNotes.Accessors.Domain.Core.EntityFramework
{
    public interface IDbSetFactory : IDisposable
    {
        DbSet<T> CreateDbSet<T>() where T : class;
        void ChangeObjectState(object entity, EntityState state);
    }
}
