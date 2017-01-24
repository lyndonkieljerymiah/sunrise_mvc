using Sunrise.Maintenance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Maintenance.Data.MasterFile
{
    public interface ISelectionDataService
    {
        Task<IEnumerable<Selection>> GetLookup(string[] categoryCode);
    }
}
