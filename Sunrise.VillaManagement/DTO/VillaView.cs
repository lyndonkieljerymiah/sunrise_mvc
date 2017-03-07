using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.VillaManagement.DTO
{
    public class VillaView
    {

        public string Id { get; set; }
        public DateTime DateStamp { get; set; }
        public string VillaNo { get; set; }
        public string ElecNo { get; set; }
        public string WaterNo { get; set; }
        public string QtelNo { get; set; }
        public string StatusDescription { get; set; }
        public string StatusCode { get; set; }
        public string Type { get; set; }
        public string TypeCode { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public decimal RatePerMonth { get; set; }
        public int ProfileIndex { get; set; }
        public VillaGalleryView Gallery { get; set; }

        public ICollection<VillaGalleryView> Galleries { get; set; }
    }
}
