using Sunrise.VillaManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.VillaManagement.DTO
{
    public class VillaDTO
    {

        public VillaDTO()
        {
            
        }

        public Villa Villa { get; set; }
        public string StatusCode { get; set; }
        public string TypeCode { get; set; }
        public int ProfileIndex { get; set; }
        public VillaGallery Profile { get; set; }
        public IEnumerable<VillaGallery> Galleries { get; set; }
    }

    public class VillaListDTO
    {
        public string Id { get; set; }
        public DateTime DateStamp { get; set; }
        public string VillaNo { get; set; }
        public string ElecNo { get; set; }
        public string WaterNo { get; set; }
        public string QTelNo { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public decimal RatePerMonth { get; set; }
        public int ProfileIndex { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string Type { get; set; }
        public VillaGalleryView Gallery { get; set; }

    }
}
