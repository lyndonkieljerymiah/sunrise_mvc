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
        T FindQuery(object id);

        Task<IEnumerable<T>> GetQueryAsync(Expression<Func<T,bool>> where = null);
        IEnumerable<T> GetQuery();
    }
}
