using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.DTO;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Persistence.Abstract
{
    public interface IVillaRepository : IBaseRepository<Villa>
    {
        Task<VillaDTO> GetVilla(int villaId);
        Task<IEnumerable<VillaDTO>> GetAvailable();
        Task<IEnumerable<VillaDTO>> GetAllVilla();
    }
}
