using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.DTO
{
    public class VillaView
    {   
        public string Id { get; private set; }
        public DateTime DateStamp { get; private set; }
        public string VillaNo { get; private set; }
        public string ElecNo { get; private set; }
        public string WaterNo { get; private set; }
        public string QtelNo { get; private set; }
        public string Status { get; private set; }
        public string StatusCode { get; private set; }
        public string Type { get; private set; }
        public int Capacity { get; private set; }
        public string Description { get; private set; }
        public decimal RatePerMonth { get; private set; }

        public ICollection<VillaGalleryView> Galleries { get; set; }
    }
}
