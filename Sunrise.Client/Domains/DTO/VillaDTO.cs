using System;
using System.ComponentModel.DataAnnotations;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Domains.DTO
{
    public class VillaDTO
    {
      
        public VillaDTO()
        {
            
        }

        [Key]
        public int Id { get; set; }

        public DateTime DateStamp { get; set; }

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

        public byte[] Picture { get; set; }

    }
}
