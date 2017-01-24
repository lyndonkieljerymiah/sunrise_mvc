using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Shared.PubSub
{
    public interface ISubscriber<T> where T : class
    {
        void Update(T entity);
        Task UpdateAsync(T entity);
    }
}
