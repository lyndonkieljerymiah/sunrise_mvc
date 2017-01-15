using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.VillaManagement.Model
{

    public class Villa
    {

        public static Villa Map(string villaNo,string elecNo,string waterNo,
            string qtelNo,string type,int capacity,string description,decimal ratePerMonth)
        {
            return new Villa(villaNo, elecNo, waterNo, qtelNo, type, capacity, description,ratePerMonth);
        }

        internal Villa(string villaNo, string elecNo, string waterNo, string qtelNo, 
            string type, int capacity, string description,decimal ratePerMonth) : this()
        {

            this.VillaNo = villaNo;
            this.ElecNo = elecNo;
            this.WaterNo = waterNo;
            this.QtelNo = qtelNo;
            this.Type = type;
            this.Capacity = capacity;
            this.Description = description;
            this.RatePerMonth = ratePerMonth;
        }

        public Villa()
        {
            DateStamp = DateTime.Now;
            Id = Guid.NewGuid().ToString();
            this.Status = "vsav";
        }

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
        public byte[] Picture { get; set; }
        public decimal RatePerMonth { get; set; }

        public virtual ICollection<VillaGallery> Galleries { get; set; }
        
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

        public void AddPrimaryPicture()
        {

        }
    }
}
