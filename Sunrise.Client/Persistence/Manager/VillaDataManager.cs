using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Abstract;

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
            var villas = await _uow.Villas.GetAllVilla();
            var vm = Mapper.Map<IEnumerable<VillaViewModel>>(villas);

            return vm;

        }

        public async Task<VillaViewModel> GetVilla(int villaId)
        {
            var villa = await _uow.Villas.FindAsync(villaId);
            var vm = Mapper.Map<VillaViewModel>(villa);

            return vm;
        }
    }
}
