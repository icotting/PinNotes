using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinNotes.Accessors.Domain.Core.EntityFramework
{
    public abstract class EntityAccessor
    {
        protected readonly IDbSetFactory _dbSetFactory;

        public EntityAccessor(IDbSetFactory dbSetFactory)
        {
            _dbSetFactory = dbSetFactory;
        }
    }
}
