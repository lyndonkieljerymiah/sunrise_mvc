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
        public async Task<T> FindQueryAsync(Expression<Func<T, bool>> clause, params Expression<Func<T, object>>[] includeExpressions)
        {   
            return await includeExpressions
                .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
                (_set, (current, expression) => current.Include(expression))
                .SingleOrDefaultAsync(clause);
        }
        public async Task<T> FindQueryAsync(Expression<Func<T, bool>> clause)
        {
            return await _set.SingleOrDefaultAsync(clause);
        }

        public T FindQuery(object id)
        {
            return _set.Find(id);
        }
        public T FindQuery(Expression<Func<T, bool>> clause)
        {
            return _set.SingleOrDefault(clause);
        }

        public async Task<IEnumerable<T>> GetQueryAsync(Expression<Func<T,bool>> where = null)
        {
            return (where != null) ? await _set.Where(where).ToListAsync() : await _set.ToListAsync();
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _set.AddRange(entities);
        }
       
        public IEnumerable<T> GetQuery(Expression<Func<T, bool>> where = null)
        {
            return (where != null) ? _set.Where(where).ToList() : _set.ToList();
        }
    }
}
