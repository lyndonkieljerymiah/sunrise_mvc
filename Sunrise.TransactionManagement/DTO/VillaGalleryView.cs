using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.TransactionManagement.DTO
{
    public class VillaGalleryView
    {
        public int Id { get; set; }
        public string VillaId { get; set; }
        public ImageBlob Blob { get; set; }
        public virtual VillaView Villa { get; set; }
    }
}
