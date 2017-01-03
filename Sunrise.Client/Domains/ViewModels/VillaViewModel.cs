using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Utilities.Enum;

namespace Sunrise.Client.Domains.ViewModels
{
    public class VillaViewModel
    {

        
        public VillaViewModel()
        {
            Images = new HashSet<ViewImages>();
        }

        public string Id { get; set; }
        public DateTime DateStamp { get; private set; }
        public string VillaNo { get; set; }
        public string ElecNo { get; set; }
        public string WaterNo { get; set; }
        public string QtelNo { get; set; }
        public string Status { get; set; }
        public string VillaStatus { get; set; }
        public string Type { get; set; }
        public string VillaType { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public decimal RatePerMonth { get; set; }

        public string Label { get { return this.VillaNo + " - " + this.VillaStatus; } }
        public ICollection<ViewImages> Images { get; private set; }
        
    }
}
