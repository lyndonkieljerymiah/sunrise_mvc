using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Utilities.Enum;
using System.Web.Mvc;
using Sunrise.Maintenance.Model;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Sunrise.Client.Domains.ViewModels
{
    public class VillaViewModel
    {
        
        public VillaViewModel(IEnumerable<Selection> selections) : this()
        {
            SetLookup(selections);
        }
        public VillaViewModel()
        {
            Images = new HashSet<ViewImages>();
            DateStamp = DateTime.Today;
            this.ImageGalleries = new List<ImageGalleryViewModel>();
        }

        public string Id { get; set; }

        public DateTime DateStamp { get; private set; }

        [Required]
        public string VillaNo { get; set; }

        [Required]
        public string ElecNo { get; set; }

        public string WaterNo { get; set; }

        public string QtelNo { get; set; }

        public string Status { get; set; }

        public string VillaStatus { get; set; }
        public string Type { get; set; }
        public string VillaType { get; set; }
        [Required]
        public int Capacity { get; set; }
        public string Description { get; set; }

        [Required]
        [RegularExpression(@"\d+",ErrorMessage ="Value must be numeric")]
        public decimal RatePerMonth { get; set; }

        public string Label {
            get { return this.VillaNo + " - " + this.VillaStatus; }
        }

        public IEnumerable<SelectListItem> Types { get; set; }
        public ICollection<ViewImages> Images { get; private set; }
        public ICollection<ImageGalleryViewModel> ImageGalleries { get; set; }

        public void SetLookup(IEnumerable<Selection> selections)
        {
            this.Types = selections.Select(s => new SelectListItem { Value = s.Code, Text = s.Description });
        }
    }

    public class ImageGalleryViewModel
    {
        public ImageGalleryViewModel()
        {
            Blob = new ImageBlob();
        }
        public int Id { get; set; }
        public ImageBlob Blob { get; set; }
        public string ImageConverted { get { return "data:image/png;base64," + Convert.ToBase64String(Blob.Blob); } }
        public bool MarkDeleted { get; set; }
    }
    
}
