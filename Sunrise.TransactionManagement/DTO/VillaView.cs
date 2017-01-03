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
        [Key]
        public string Id { get; private set; }

        public DateTime DateStamp { get; private set; }

        public string VillaNo { get; set; }

        public string ElecNo { get; set; }

        public string WaterNo { get; set; }

        public string QtelNo { get; set; }

        public string Status { get; private set; }

        public string Type { get; set; }

        public int Capacity { get; set; }

        public string Description { get; set; }
        
        public decimal RatePerMonth { get; set; }
    }
}
