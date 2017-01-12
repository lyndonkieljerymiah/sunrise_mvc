using AutoMapper;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.VillaManagement.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.Client.Persistence.Manager
{
    public class VillaDataManager
    {
        private readonly IUnitOfWork _uow;

        public VillaDataManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<VillaViewModel>> GetVillas(string search="")
        {
            var villas = string.IsNullOrEmpty(search) ? 
                (await _uow.Villas.GetAllVilla(1, 100)) : 
                (await _uow.Villas.GetVillaByNo(search)); 

            var vm = Mapper.Map<IEnumerable<VillaViewModel>>(villas);
            return vm;
        }

        public async Task<VillaViewModel> GetVilla(string villaId)
        {
            var villa = await _uow.Villas.FindQueryAsync(villaId);
            var vm = Mapper.Map<VillaViewModel>(villa);
            return vm;
        }

        public async Task UpdateVillaStatus(string id,VillaStatusEnum status)
        {
            var villa = await _uow.Villas.FindQueryAsync(id);
            villa.SetStatus(status);
            _uow.Villas.Update(villa);
            await _uow.SaveChanges();
        }

        public void UpdateVillaStatusNonAsync(string id, VillaStatusEnum status)
        {
            var villa = _uow.Villas.FindQuery(id);
            villa.SetStatus(status);
            _uow.Villas.Update(villa);
            _uow.SaveChangesNonAsync();
        }
    }
}
