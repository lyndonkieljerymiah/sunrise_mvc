using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.DTO;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Persistence.Abstract;
using Sunrise.Client.Persistence.Context;

namespace Sunrise.Client.Persistence.Repositories
{
    public class VillaRepository : BaseRepository<Villa>, IVillaRepository
    {
           
        public VillaRepository(AppDbContext context,ReferenceDbContext referenceDb) : base(context,referenceDb)
        {
        }

        public async Task<VillaDTO> GetVilla(int villaId)
        {  
            var villa = await _referenceDbContext.VillaDtos.SingleOrDefaultAsync(v => v.Id == villaId);
            return villa;
        }
        
        public async Task<IEnumerable<VillaDTO>> GetAllVilla()
        {
            var villas = await _referenceDbContext.VillaDtos.ToListAsync();
            return villas;
        }

        public async Task<IEnumerable<VillaDTO>> GetAvailable()
        {
            var villas = await _referenceDbContext.VillaDtos
                        .Where(v => v.Status.Contains("available"))
                        .ToListAsync();

            return villas;
        }


    }
}
