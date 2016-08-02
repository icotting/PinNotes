using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinNotes.Accessors.Domain.Core.EntityFramework
{
    public interface IDbContext : IDisposable
    {
        void SaveChanges();
    }
}
