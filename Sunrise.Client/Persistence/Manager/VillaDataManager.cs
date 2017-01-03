using AutoMapper;
using Sunrise.Client.Domains.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sunrise.VillaManagement;
using Utilities.Enum;
using Utilities.GeneralRepository;
using Sunrise.VillaManagement.Abstract;


namespace Sunrise.Client.Persistence.Manager
{
    public class VillaDataManager
    {
        private readonly IUnitOfWork _uow;

        public VillaDataManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<VillaViewModel>> GetVillas()
        {
            var villas = await _uow.Villas.GetAllVilla(1, 20);
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
    }
}
