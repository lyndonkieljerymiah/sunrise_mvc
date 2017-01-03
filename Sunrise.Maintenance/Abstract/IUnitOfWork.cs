using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Maintenance.Abstract
{
    public interface IUnitOfWork : IDisposable
    {

        ISelectionRepository Selections { get; }

        Task SaveChanges();
    }
}
