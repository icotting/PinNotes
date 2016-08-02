using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinNotes.Accessors.Domain.Core.EntityFramework
{
    public class PersistenceContextAdapter : IDbSetFactory, IDbContext
    {
        private readonly DbContext _context;

        public PersistenceContextAdapter(DbContext context)
        {
            _context = context;
        }

        #region IObjectContext Members

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion

        #region IObjectSetFactory Members

        public DbSet<T> CreateDbSet<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void ChangeObjectState(object entity, EntityState state)
        {
            _context.Entry(entity).State = state;
        }
        #endregion
    }
}
