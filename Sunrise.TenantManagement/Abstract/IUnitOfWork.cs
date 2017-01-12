using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TenantManagement.Abstract
{
    public interface IUnitOfWork : IDisposable
    {

        ITenantRepository Tenants { get; }

        Task SaveChanges();
        void SaveChangesNonAsync();
    }
}
