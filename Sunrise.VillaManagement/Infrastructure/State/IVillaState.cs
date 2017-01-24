using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.VillaManagement.Infrastructure.State
{
    public interface IVillaState
    {
        string Reserved();
        string Occupied();
        string Vacant();
    }
}
