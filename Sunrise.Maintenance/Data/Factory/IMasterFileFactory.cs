using Sunrise.Maintenance.Data.MasterFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Maintenance.Data.Factory
{
    public interface IMasterFileFactory : IDisposable
    {
        ISelectionDataService Selections { get; }
    }
}
