using Sunrise.VillaManagement.Abstract;
using Sunrise.VillaManagement.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.GeneralRepository;
using System.Data.Entity;
using PagedList.EntityFramework;

namespace Sunrise.VillaManagement.Persistence.Repository
{
    public class VillaRepository : BaseRepository<Villa, AppContextDb>, IVillaRepository
    {
        public VillaRepository(AppContextDb context) : base(context)
        {

        }

        public async Task<IEnumerable<Villa>> GetAllVilla(int currentPage, int pageSize)
        {
            var villas = await _set
                            .OrderBy(v => v.VillaNo)
                            .ToPagedListAsync(currentPage, pageSize);
            return villas;
        }

        
    }
}
