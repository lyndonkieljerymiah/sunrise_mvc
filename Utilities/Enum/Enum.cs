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

    public enum GenderEnum
    {
        Male,
        Female
    }

    public struct CustomResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get;private set; }
        public object ReturnObject { get; set; }
        public ICollection<object> ReturnObjects { get; set; }
        
        public CustomResult(List<string> errors)
        {
            Success = false;
            Errors = errors;
            ReturnObject = new object();
            ReturnObjects = new HashSet<object>();
        }

        public void AddError(string error)
        {
            if (Errors == null)
                Errors = new List<string>();
            Errors.Add(error);
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

}
