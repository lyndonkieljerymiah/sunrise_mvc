using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Persistence.Abstract;
using Sunrise.Client.Persistence.Context;

namespace Sunrise.Client.Persistence.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly ReferenceDbContext _referenceDbContext;
        

        protected  DbSet<T> _set;

        protected BaseRepository(AppDbContext context,ReferenceDbContext referenceDbContext)
        {
            _context = context;
            _referenceDbContext = referenceDbContext;
            _set = _context.Set<T>();
        }

        public virtual void Add(T entity)
        {
            _set.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _set.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task<T> FindAsync(object id)
        {
            return await _set.FindAsync(id);
        }
        public virtual void Remove(T entity)
        {
            _set.Remove(entity);
        }

     
    }
}
