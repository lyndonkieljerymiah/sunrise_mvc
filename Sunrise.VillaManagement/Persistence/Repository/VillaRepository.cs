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

        public async Task<IEnumerable<VillaDTO>> GetAllVilla(string villaNo="",string statusCode = "", int currentPage = 1, int pageSize = 20)
        {

            var villas = _context.Villas.Select(v => new VillaDTO
            {
                Villa = v,
                Profile = _context.Galleries.FirstOrDefault(g => g.Id == v.ProfileIndex)
            });

            if (statusCode != "" && villaNo != "")
                villas = villas.Where(v => v.Villa.Status == statusCode && v.Villa.VillaNo.Contains(villaNo));
            else if(statusCode != "")
                villas = villas.Where(v => v.Villa.Status == statusCode);
            else if(villaNo != "")
                villas = villas.Where(v => v.Villa.VillaNo == villaNo);

            return (await villas.OrderBy(v => v.Villa.VillaNo)
                        .ToPagedListAsync(currentPage, pageSize)).ToList();
        }

        public async Task<IEnumerable<VillaView>> GetVillaByNo(string no)
        {
            return await _referenceDbContext.Villas
                .Where(v => v.VillaNo.Contains(no) && v.StatusCode == "vsav")
                .OrderBy(v => v.VillaNo)
                .ToListAsync();
        }
     
        public void RemoveGallery(Villa parent,IEnumerable<VillaGallery> galleries)
        {
            foreach(var gallery in galleries)
            {
                parent.Galleries.Remove(gallery);
                _context.Galleries.Remove(gallery);
            }
        }
    }
}
