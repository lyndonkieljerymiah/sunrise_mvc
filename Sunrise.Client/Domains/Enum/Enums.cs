using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.Enum
{
    public enum GenderEnum
    {
        Male,
        Female
    }


    public enum VillaStatusEnum
    {
        Available,
        NotAvailable,
        Reserved
    }

    public struct CustomResult
    {
        public bool Success;
        public List<string> Errors;
        public object ReturnObject;
        public IEnumerable<object> ReturnObjects;
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
