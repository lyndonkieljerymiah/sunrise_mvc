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

    public struct Result
    {
        public bool Success;
        public List<string> Errors;
        public object ReturnObject;
        public IEnumerable<object> ReturnObjects;
    }

    

}
