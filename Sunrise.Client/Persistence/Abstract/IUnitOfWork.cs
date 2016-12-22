using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Persistence.Abstract
{
    public interface IUnitOfWork : IDisposable
    {

        ITenantRepository Tenants { get; }
        ISelectionRepository Selections { get; }
        IVillaRepository Villas { get; }

        void SaveChanges();

        Task SaveChangesAsync();


    }
}
