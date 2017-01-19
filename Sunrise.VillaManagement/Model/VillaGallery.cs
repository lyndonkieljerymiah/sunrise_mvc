using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.VillaManagement.Model
{
    public class VillaGallery
    {
        public VillaGallery(ImageBlob blob)
        {
            this.Blob = blob;
        }

        internal VillaGallery()
        {

        }
        public int Id { get; set; }
        public string VillaId { get; set; }
        public ImageBlob Blob { get; set; }
        public virtual Villa Villa { get; set; }
    }
}
