using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.Models
{
    public class Villa
    {

        public Villa()
        {
            DateStamp = DateTime.Now;
        }

        public int Id { get; set; }

        public DateTime DateStamp { get; private set; } 

        public string VillaNo { get; set; }

        public string ElecNo { get; set; }

        public string WaterNo { get; set; }

        public string QtelNo { get; set; }

        public int Status { get; set; }
        
        public string Type { get; set; }

        public int Capacity { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

    }
}
