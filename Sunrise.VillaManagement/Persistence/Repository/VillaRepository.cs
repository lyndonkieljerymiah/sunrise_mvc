using Sunrise.VillaManagement.Abstract;
using Sunrise.VillaManagement.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.GeneralRepository;
using System.Data.Entity;
using PagedList.EntityFramework;
using Sunrise.VillaManagement.DTO;
using System;

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
                            .Where(v => v.StatusCode == "vsav")
                            .OrderBy(v => v.VillaNo)
                            .ToPagedListAsync(currentPage, pageSize);
            return villas;
        }

        public async Task<IEnumerable<VillaView>> GetVillaByNo(string no)
        {
            return await _referenceDbContext.Villas
                .Where(v => v.VillaNo.Contains(no) && v.StatusCode == "vsav")
                .OrderBy(v => v.VillaNo)
                .ToListAsync();
        }

       

        public void RemoveGallery(Villa parent,int childId)
        {
            var gallery = parent.Galleries.SingleOrDefault(c => c.Id == childId);
            parent.Galleries.Remove(gallery);
            _context.Galleries.Remove(gallery);
        }
    }
}
