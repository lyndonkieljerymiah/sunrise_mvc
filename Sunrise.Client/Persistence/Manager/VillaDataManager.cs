using AutoMapper;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.VillaManagement.Data.Factory;
using Sunrise.VillaManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.Client.Persistence.Manager
{
    public class VillaDataManager : IDisposable
    {
        private IVillaDataFactory Factory { get; }

        public VillaDataManager(IVillaDataFactory factory)
        {
            Factory = factory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<CustomResult> UpdateVilla(VillaViewModel vm)
        {
            var result = new CustomResult();
            Villa villa;

            //fetch existing villa
            villa = await Factory.Villas.GetVillaById(vm.Id);

            villa.Update(vm.VillaNo, vm.ElecNo,
            vm.WaterNo, vm.QtelNo, vm.Type,
            vm.Capacity, vm.Description, vm.RatePerMonth);
            villa.ProfileIndex = vm.ProfileIndex;

            if (vm.ImageGalleries.Count > 0)
            {
                foreach (var gallery in vm.ImageGalleries)
                {
                    if (gallery.MarkDeleted)
                    {
                        villa.MarkGalleryForDeletion(gallery.Id);
                    }
                    else
                    {
                        villa.AddGallery(gallery.Blob);
                    }
                }
            }

            if (!string.IsNullOrEmpty(vm.Id))
            {
                result = await Factory.Villas.UpdateVilla(villa);
            }
            else
            {
                result = await Factory.Villas.CreateVilla(villa);
            }
            return result;
        }

        /// <summary>
        /// TODO: Insert Villa along with Galleries
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<CustomResult> CreateVilla(VillaViewModel vm)
        {
            var result = new CustomResult();
            Villa villa;

            villa = Villa.Map(vm.VillaNo, vm.ElecNo,
            vm.WaterNo, vm.QtelNo, vm.Type,
            vm.Capacity, vm.Description, vm.RatePerMonth);
            villa.ProfileIndex = vm.ProfileIndex;

            if (vm.ImageGalleries.Count > 0)
            {
                foreach (var gallery in vm.ImageGalleries)
                {
                    if (gallery.MarkDeleted)
                    {
                        villa.MarkGalleryForDeletion(gallery.Id);
                    }
                    else
                    {
                        villa.AddGallery(gallery.Blob);
                    }
                }
            }

            if (!string.IsNullOrEmpty(vm.Id))
            {
                result = await Factory.Villas.UpdateVilla(villa);
            }
            else
            {
                result = await Factory.Villas.CreateVilla(villa);
            }
            return result;
        }

        public async Task<IEnumerable<VillaListViewModel>> GetVacantVillas(string villaNo = "", int pageNumber = 1, int pageSize = 20, string noImageUrl = "")
        {
            var villas = await Factory.Villas.GetVillasForDisplay(villaNo, VillaStatusEnum.Available, pageNumber, pageSize);

            ICollection<VillaListViewModel> viewModels = new List<VillaListViewModel>();
            foreach (var villa in villas)
            {
                VillaListViewModel viewModel = Mapper.Map<VillaListViewModel>(villa);
                viewModel.ImageGallery.Id = viewModel.ImageGallery.Id;
                viewModel.ImageGallery.FileName = viewModel.ImageGallery.FileName;
                if (villa.Gallery != null)
                {
                    viewModel.ImageGallery.SetImageBlob(villa.Gallery.Blob.Blob);
                }
                else
                {
                    viewModel.ImageGallery.SetImageBlob(null, noImageUrl);
                }

                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public async Task<IEnumerable<VillaListViewModel>> GetVillaMasterList(
            string villaNo = "",
            VillaStatusEnum status = VillaStatusEnum.All,
            int pageNo = 1,
            int pageSize = 20, string noImageUrl = "")
        {
            var villas = await Factory.Villas.GetVillasForDisplay(villaNo, status, pageNo, pageSize);

            ICollection<VillaListViewModel> viewModels = new List<VillaListViewModel>();
            foreach (var villa in villas)
            {
                VillaListViewModel viewModel = Mapper.Map<VillaListViewModel>(villa);
                viewModel.ImageGallery.Id = viewModel.ImageGallery.Id;
                viewModel.ImageGallery.FileName = viewModel.ImageGallery.FileName;
                if (villa.Gallery != null)
                {
                    viewModel.ImageGallery.SetImageBlob(villa.Gallery.Blob.Blob);
                }
                else
                {
                    viewModel.ImageGallery.SetImageBlob(null, noImageUrl);
                }
                viewModels.Add(viewModel);
            }
            return viewModels;
        }

        public async Task<VillaViewModel> GetVilla(string id)
        {
            var villa = await Factory.Villas.GetVillaById(id);
            var vm = Mapper.Map<VillaViewModel>(villa);
            return vm;
        }

        public async Task UpdateVillaStatus(string id, VillaStatusEnum status)
        {
            var villa = await Factory.Villas.FindVilla(id);
            if (villa != null)
            {
                switch (status)
                {
                    case VillaStatusEnum.Available:
                        villa.MakeAvailable();
                        break;
                    case VillaStatusEnum.NotAvailable:
                        villa.MakeOccupied();
                        break;
                    case VillaStatusEnum.Reserved:
                        villa.MakeReserved();
                        break;
                    default:
                        break;
                }
            }
            await Factory.Villas.UpdateVilla(villa);
        }

        public void Dispose()
        {
            Factory.Dispose();
        }
    }
}
