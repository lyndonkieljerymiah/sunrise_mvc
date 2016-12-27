using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Domains.ViewModels
{
    public class VillaViewModel
    {

        public static IEnumerable<VillaViewModel> CreateRange(IEnumerable<Villa> villas)
        {
            var vms = Mapper.Map<IEnumerable<VillaViewModel>>(villas);
            
            return vms.ToList();
        }

        public static VillaViewModel Create(Villa villa)
        {
            return Mapper.Map<VillaViewModel>(villa);
        }

        public VillaViewModel()
        {
            
        }
        public int Id { get; set; }

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

        

    }
}
