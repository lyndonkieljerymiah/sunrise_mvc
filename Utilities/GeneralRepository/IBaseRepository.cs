using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.GeneralRepository
{
    public interface IBaseRepository<T> where T : class
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);

        Task<T> FindQueryAsync(object id);
        Task<T> FindQueryAsync(Expression<Func<T,bool>> clause, params Expression<Func<T,object>>[] includeExpressions);
        Task<T> FindQueryAsync(Expression<Func<T, bool>> clause);
        T FindQuery(object id);

        Task<IEnumerable<T>> GetQueryAsync(Expression<Func<T,bool>> where = null);

        IEnumerable<T> GetQuery();
    }
}
