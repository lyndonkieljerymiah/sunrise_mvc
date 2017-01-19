using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Enum
{
    public enum VillaStatusEnum
    {
        Available,
        NotAvailable,
        Reserved
    }

    public enum SearchByEnum
    {
        Code,
        Id
    }

    public enum GenderEnum
    {
        Male,
        Female
    }

    public struct CustomResult
    {
        public bool Success { get; set; }
        public IDictionary<string,string> Errors { get;private set; }
        public object ReturnObject { get; set; }
        public ICollection<object> ReturnObjects { get; set; }
        
        public CustomResult(IDictionary<string,string> errors)
        {
            Success = false;
            Errors = errors;
            ReturnObject = new object();
            ReturnObjects = new HashSet<object>();
        }

        public void AddError(string key, string error)
        {
            if (Errors == null)
                Errors = new Dictionary<string, string>();
            Errors.Add(key,error);
        }





       
    }


    public struct ViewImages
    {
        public int Index { get; private set; }
        public string ImageUrl { get; private set; }
        public string Title { get; private set; }

        public ViewImages(int index, string imageUrl, string title)
        {
            this.Index = index;
            this.ImageUrl = imageUrl;
            this.Title = title;
        }
    }

    public class ImageBlob
    {
        public int Size { get; set; }
        public string MimeType { get; set; }
        public byte[] Blob { get; set; }
        public string FileName { get; set; }
        public string FileFormat { get; set; }
    }


}
