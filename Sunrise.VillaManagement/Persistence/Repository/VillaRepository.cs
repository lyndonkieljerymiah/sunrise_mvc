using Sunrise.VillaManagement.Abstract;
using Sunrise.VillaManagement.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.GeneralRepository;
using System.Data.Entity;
using PagedList.EntityFramework;
using Sunrise.VillaManagement.DTO;

namespace Sunrise.VillaManagement.Persistence.Repository
{
    public class VillaRepository : BaseRepository<Villa, AppDbContext>, IVillaRepository
    {
        private ReferenceDbContext _referenceDbContext;

        public VillaRepository(AppDbContext context,ReferenceDbContext referenceDbContext) 
            : base(context)
        {
            _referenceDbContext = referenceDbContext;
        }

        public async Task<IEnumerable<VillaView>> GetAllVilla(int currentPage, int pageSize)
        {
            var villas = await _referenceDbContext.Villas
                            .OrderBy(v => v.VillaNo)
                            .ToPagedListAsync(currentPage, pageSize);
            return villas;
        }

    }
}
