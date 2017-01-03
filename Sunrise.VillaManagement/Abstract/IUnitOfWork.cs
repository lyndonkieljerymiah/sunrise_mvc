using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.VillaManagement.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IVillaRepository Villas { get; }

        Task SaveChanges();
    }
}
