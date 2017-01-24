using Sunrise.VillaManagement.Abstract;
using Sunrise.VillaManagement.DTO;
using Sunrise.VillaManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.VillaManagement.Data.Villas
{
    public interface IVillaDataService
    {
        Task<CustomResult> CreateVilla(Villa villa);
        Task<CustomResult> UpdateVilla(Villa villa);
        Task<CustomResult> RemoveVilla(string id);
        Task<IEnumerable<VillaListDTO>> GetVillasForDisplay(string villaNo = "", VillaStatusEnum status = VillaStatusEnum.All, int pageNumber = 1, int pageSize = 20);

        
        Task<Villa> FindVilla(string id);
        Task<Villa> GetVillaByNo(string villaNo);
        Task<Villa> GetVillaById(string Id);



    }
}
