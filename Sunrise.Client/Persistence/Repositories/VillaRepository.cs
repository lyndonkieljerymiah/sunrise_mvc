using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Persistence.Abstract;

namespace Sunrise.Client.Persistence.Repositories
{
    public class VillaRepository : BaseRepository<Villa>, IVillaRepository
    {
        public VillaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Villa> GetVilla(string villaNo)
        {
            var villa = await  _context.Villas.SingleOrDefaultAsync(v => v.VillaNo == villaNo);
            return villa;
        }


        public async Task<IEnumerable<Villa>> GetAllVilla()
        {
            var villas = await _context.Villas
                       .ToListAsync();

            return villas;
        }

        public async Task<IEnumerable<Villa>> GetAvailable()
        {
            var villas = await _context.Villas
                        .Where(v => v.Status.Contains("available"))
                        .ToListAsync();

            return villas;
        }


    }
}
