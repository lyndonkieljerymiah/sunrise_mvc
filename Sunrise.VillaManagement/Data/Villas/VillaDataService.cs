using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.VillaManagement.DTO;
using Sunrise.VillaManagement.Model;
using Utilities.Enum;
using Sunrise.VillaManagement.Abstract;
using Sunrise.VillaManagement.Infrastructure.State;
using Sunrise.VillaManagement.Persistence;
using System.Data.Entity;
using PagedList.EntityFramework;


namespace Sunrise.VillaManagement.Data.Villas
{
    public class VillaDataService : IVillaDataService
    {

        private IVillaState State { get; }
        private IUnitOfWork UnitOfWork { get; set; }
        private AppDbContext Context { get; set; }
        private ReferenceDbContext ReferenceContext { get; set; }

        public VillaDataService(
            AppDbContext context,
            ReferenceDbContext referenceContext)
        {
            State = new VillaState();
            Context = context;
            ReferenceContext = referenceContext;
        }

        public async Task<CustomResult> CreateVilla(Villa villa)
        {
            var result = new CustomResult();
            try
            {
                Context.Villas.Add(villa);
                await Context.SaveChangesAsync();
                result.Success = true;
                result.ReturnObject = villa.Id;
            }
            catch (Exception e)
            {
                result.AddError("VillaInternalException", e.Message);
                result.Success = false;
            }

            return result;
        }
        public async Task<CustomResult> UpdateVilla(Villa villa)
        {
            var result = new CustomResult();
            try
            {
                //check if the galleries mark for deletion
                var galleries = villa.GetForDeletion();
                if (galleries.Count() > 0)
                {
                    foreach (var gallery in galleries)
                    {
                        villa.Galleries.Remove(gallery);
                        Context.Galleries.Remove(gallery);
                    }
                }

                Context.Villas.Attach(villa);
                Context.Entry(villa).State = System.Data.Entity.EntityState.Modified;
                await Context.SaveChangesAsync();

                result.Success = true;
                result.ReturnObject = villa.Id;
            }
            catch (Exception e)
            {
                result.AddError("InternalException", e.Message);
                result.Success = false;
            }
            return result;
        }
        public async Task<CustomResult> RemoveVilla(string id)
        {
            var result = new CustomResult();
            try
            {
                var villa = await Context.Villas.FindAsync(id);
                Context.Villas.Remove(villa);
                await Context.SaveChangesAsync();
                result.Success = true;

            }
            catch (Exception e)
            {
                result.AddError("VillaInternalErrorException", e.Message);
                result.Success = false;
            }

            return result;
        }
        public async Task<IEnumerable<VillaListDTO>> GetVillasForDisplay(string villaNo = "", VillaStatusEnum status = VillaStatusEnum.All, int pageNumber = 1, int pageSize = 20)
        {
            var villas = ReferenceContext
                .Villas
                .Include(v => v.Galleries)
                .Select(v => new VillaListDTO
                {
                    Id = v.Id,
                    VillaNo = v.VillaNo,
                    DateStamp = v.DateStamp,
                    ElecNo = v.ElecNo,
                    WaterNo = v.WaterNo,
                    QTelNo = v.QtelNo,
                    Status = v.Status,
                    StatusCode = v.StatusCode,
                    Type = v.Type,
                    Capacity = v.Capacity,
                    Description = v.Description,
                    ProfileIndex = v.ProfileIndex,
                    RatePerMonth = v.RatePerMonth,
                    Gallery = ReferenceContext.Galleries.FirstOrDefault(g => g.Id == v.ProfileIndex)
                });
            string state = "";

            if (!string.IsNullOrEmpty(villaNo) && status != VillaStatusEnum.All)
            {
                
                switch (status)
                {
                    case VillaStatusEnum.Available:
                        state = State.Vacant();
                        villas = villas.Where(v => v.VillaNo == villaNo && v.StatusCode == state);
                        break;
                    case VillaStatusEnum.NotAvailable:
                        state = State.Occupied();
                        villas = villas.Where(v => v.VillaNo == villaNo && v.StatusCode == state);
                        break;
                    case VillaStatusEnum.Reserved:
                        state = State.Reserved();
                        villas = villas.Where(v => v.VillaNo == villaNo && v.StatusCode == state);
                        break;
                    default:
                        break;
                }
            }
            else if (!string.IsNullOrEmpty(villaNo))
            {
                villas = villas.Where(v => v.VillaNo == villaNo);
            }
            else if (status != VillaStatusEnum.All)
            {
                switch (status)
                {
                    case VillaStatusEnum.Available:
                        state = State.Vacant();
                        villas = villas.Where(v => v.StatusCode == state);
                        break;
                    case VillaStatusEnum.NotAvailable:
                        state = State.Occupied();
                        villas = villas.Where(v => v.StatusCode == state);
                        break;
                    case VillaStatusEnum.Reserved:
                        state = State.Reserved();
                        villas = villas.Where(v => v.StatusCode == state);
                        break;
                    default:
                        break;
                }
            }

            return (await villas
                        .OrderBy(v => v.VillaNo)
                        .ToPagedListAsync(pageNumber, pageSize)
                        ).ToList();
        }
        public async Task<Villa> GetVillaByNo(string villaNo)
        {
            var villa = await Context
                .Villas
                .Include(v => v.Galleries).SingleOrDefaultAsync(v => v.VillaNo == villaNo);

            return villa;
        }
        public async Task<Villa> FindVilla(string id)
        {
            return await Context.Villas.FindAsync(id);
        }

        public async Task<Villa> GetVillaById(string Id)
        {
            var villa = await Context
               .Villas
               .Include(v => v.Galleries).SingleOrDefaultAsync(v => v.Id == Id);

            return villa;
        }
    }
}
