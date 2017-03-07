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
        public byte[] Blob { get; set; }
        public string FileFormat { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public int Size { get; set; }

        public virtual VillaView Villa { get; set; }
    }
}
