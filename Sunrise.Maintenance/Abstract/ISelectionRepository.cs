using Sunrise.Maintenance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralRepository;

namespace Sunrise.Maintenance.Abstract
{
    public interface ISelectionRepository : IBaseRepository<Selection>
    {
        Task<IEnumerable<Selection>> GetSelectionByType(string[] type);
    }
}
