using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.VillaManagement.Model
{
    public class VillaGallery
    {
        public int Id { get; set; }
        public string VillaId { get; set; }
        public byte[] ImageData { get; set; }
        public string ImagePath { get; set; }
        public virtual Villa Villa { get; set; }
    }
}
