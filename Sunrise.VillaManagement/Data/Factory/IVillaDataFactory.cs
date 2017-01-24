using Sunrise.VillaManagement.Data.Villas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.VillaManagement.Data.Factory
{
    public interface IVillaDataFactory : IDisposable
    {
        IVillaDataService Villas { get; }
    }
}
