using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.DynamicData;

namespace Sunrise.Client.Persistence.Abstract
{
    public interface IBaseRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);

        Task<T> FindAsync(object Id);


    }
}
