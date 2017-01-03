using Sunrise.VillaManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralRepository;

namespace Sunrise.VillaManagement.Abstract
{
    public interface IVillaRepository : IBaseRepository<Villa>
    {
        Task<IEnumerable<Villa>> GetAllVilla(int currentPage,int pageSize);
    }
}
