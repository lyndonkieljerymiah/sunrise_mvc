using AutoMapper;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.VillaManagement.Abstract;
using Sunrise.VillaManagement.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Enum;
using System.Linq;

namespace Sunrise.Client.Persistence.Manager
{
    public class VillaDataManager
    {
        private readonly IUnitOfWork _uow;

        public VillaDataManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// TODO: Insert Villa along with Galleries
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<CustomResult> CreateAsync(VillaViewModel vm)
        {
            var result = new CustomResult();
            try
            {
                Villa villa;
                if (!string.IsNullOrEmpty(vm.Id))
                {
                    villa = await _uow.Villas.FindQueryAsync(v => v.Id == vm.Id, v => v.Galleries);
                    villa.Update(vm.VillaNo, vm.ElecNo,
                    vm.WaterNo, vm.QtelNo, vm.Type,
                    vm.Capacity, vm.Description, vm.RatePerMonth);
                }
                else
                {
                    villa = Villa.Map(vm.VillaNo, vm.ElecNo,
                    vm.WaterNo, vm.QtelNo, vm.Type,
                    vm.Capacity, vm.Description, vm.RatePerMonth);
                }
                if (vm.ImageGalleries.Count > 0)
                {
                    foreach (var gallery in vm.ImageGalleries)
                    {
                        if (gallery.MarkDeleted)
                        {
                            _uow.Villas.RemoveGallery(villa, gallery.Id);
                        }
                        else
                        {
                            villa.AddGallery(gallery.Blob);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(vm.Id))
                {
                    _uow.Villas.Update(villa);
                }
                else
                {
                    _uow.Villas.Add(villa);
                }
                result.ReturnObject = (string)villa.Id;
                await _uow.SaveChanges();
                result.Success = true;
            }
            catch (Exception e)
            {
                result.AddError("InternalErrorException", e.Message);
            }

            return result;
        }

        public async Task<IEnumerable<VillaViewModel>> GetVillas(string search = "")
        {
            var villas = string.IsNullOrEmpty(search) ?
                (await _uow.Villas.GetAllVilla(1, 100)) :
                (await _uow.Villas.GetVillaByNo(search));
            var vm = Mapper.Map<IEnumerable<VillaViewModel>>(villas);
            return vm;
        }

        public async Task<VillaViewModel> GetVilla(string villaId)
        {
            var villa = await _uow.Villas.FindQueryAsync(v => v.Id == villaId, v => v.Galleries);
            var vm = Mapper.Map<VillaViewModel>(villa);
            return vm;
        }

        public async Task UpdateVillaStatus(string id, VillaStatusEnum status)
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
