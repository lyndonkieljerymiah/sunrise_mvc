using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Persistence.Abstract;

namespace Sunrise.Client.Persistence.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected  DbSet<T> _set;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _set.Add(entity);
        }
        public async Task<T> FindAsync(object id)
        {
            return await _set.FindAsync(id);
        }
        public void Remove(T entity)
        {
            _set.Remove(entity);
        }
    }
}
