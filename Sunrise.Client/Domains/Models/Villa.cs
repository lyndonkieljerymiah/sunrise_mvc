using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Enum;

namespace Sunrise.Client.Domains.Models
{
    
    public class Villa
    {

        public Villa()
        {
            DateStamp = DateTime.Now;
        }

        public int Id { get; private set; }
        public DateTime DateStamp { get; private set; } 
        public string VillaNo { get; set; }
        public string ElecNo { get; set; }
        public string WaterNo { get; set; }
        public string QtelNo { get; set; }
        public string Status { get; private set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public decimal RatePerMonth { get; set; }


        public void SetStatus(VillaStatusEnum status)
        {
            var strStatus = "";
            if (status == VillaStatusEnum.Available)
            {
                strStatus = "vsav";
            }
            else if (status == VillaStatusEnum.NotAvailable)
            {
                strStatus = "vsna";
            }
            else
            {
                strStatus = "vsres";
            }

            this.Status = strStatus;
        }
        


    }
}
