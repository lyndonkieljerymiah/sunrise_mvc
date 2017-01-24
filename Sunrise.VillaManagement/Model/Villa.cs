using Sunrise.VillaManagement.Infrastructure.State;
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

        private IVillaState State { get; set; }
        private ICollection<VillaGallery> ForDeletion { get; set; }

        #region Factory
        public static Villa Map(string villaNo,string elecNo,string waterNo,
            string qtelNo,string type,int capacity,string description,decimal ratePerMonth)
        {
           return new Villa(villaNo, elecNo, waterNo, qtelNo, type, capacity, description,ratePerMonth);
        }
        #endregion


        #region Constructor
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
            this.ProfileIndex = 0;
        }

        public Villa()
        {
            DateStamp = DateTime.Now;
            Id = Guid.NewGuid().ToString();
            this.Galleries = new HashSet<VillaGallery>();

            State = new VillaState();
            MakeAvailable();
            ForDeletion = new HashSet<VillaGallery>();
        }
        #endregion


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
        public virtual ICollection<VillaGallery> Galleries { get; set; }
        public int ProfileIndex { get; set; }

        public void Update(string villaNo, string elecNo, string waterNo,
            string qtelNo, string type, int capacity, 
            string description, decimal ratePerMonth)
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


        #region ver 1.1 - new approach pattern
        public void MakeAvailable()
        {
            this.Status = this.State.Vacant();
        }
        
        public void MakeReserved()
        {
            this.Status = this.State.Reserved();
        }
        
        public void MakeOccupied()
        {
            this.Status = this.State.Occupied();
        }
        
        public void MarkGalleryForDeletion(int galleryId)
        {
            var gallery = Galleries.SingleOrDefault(g => g.Id == galleryId);
            ForDeletion.Add(gallery);

        }

        public ICollection<VillaGallery> GetForDeletion()
        {
            return ForDeletion;
        }
        #endregion


        #region ver 1.0 - deprecated soon
        /// <summary>
        /// ver 1.0 - become deprecated
        /// </summary>
        /// <param name="status"></param>
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

        /// <summary>
        /// ver 1.0 - uses to store and manage images by storing it's blob data
        /// </summary>
        /// <param name="blob"></param>
        public void AddGallery(ImageBlob blob)
        {
            var gallery = new VillaGallery(blob);
            Galleries.Add(gallery);
        }
        #endregion
    }
}
