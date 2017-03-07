using Sunrise.VillaManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.ViewModels
{

    public class VillaListMasterFile
    {
        public VillaListMasterFile()
        {
            Boards = new HashSet<VillaStatusBoard>();
        }
        public IEnumerable<VillaListViewModel> ListView { get; set; }
        public ICollection<VillaStatusBoard> Boards { get; set; }
    }

    public class VillaListViewModel
    {
        public VillaListViewModel()
        {
            ImageGallery = new ImageGalleryReaderViewModel();
        }

        public string Id { get; set; }
        public DateTime DateStamp { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public string VillaNo { get; set; }
        public string ElecNo { get; set; }
        public string WaterNo { get; set; }
        public string QtelNo { get; set; }
        public string StatusDescription { get; set; }
        public string StatusCode { get; set; }
        public string Type { get; set; }
        public string DefaultImageUrl { get; set; }
        public decimal RatePerMonth { get; set; }
        public ImageGalleryReaderViewModel ImageGallery { get; set; }
    }

    public class VillaStatusBoard
    {
        public string Header { get; set; }
        public int Total { get; set; }
    }
}
