using Sunrise.VillaManagement.DTO;
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
        Task<IEnumerable<VillaDTO>> GetAllVilla(string villaNo, string statusCode = "", int currentPage = 1, int pageSize = 20);
        Task<IEnumerable<VillaView>> GetVillaByNo(string no);
        void RemoveGallery(Villa parent, IEnumerable<VillaGallery> galleries);
    }
}
