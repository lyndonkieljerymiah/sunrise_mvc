using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.GeneralRepository
{
    public abstract class BaseRepository<T, TContext> : 
        IBaseRepository<T> 
            where T : class
            where TContext : DbContext
    {
        protected readonly TContext _context;
        protected DbSet<T> _set;

        protected BaseRepository(TContext context)
        {
            _context = context;
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

        
        public virtual void Remove(T entity)
        {
            _set.Remove(entity);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public async Task<T> FindQueryAsync(object id)
        {
            return await _set.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetQueryAsync(Expression<Func<T,bool>> where = null)
        {
            return (where != null) ? await _set.Where(where).ToListAsync() : await _set.ToListAsync();
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _set.AddRange(entities);
        }
    }
}
