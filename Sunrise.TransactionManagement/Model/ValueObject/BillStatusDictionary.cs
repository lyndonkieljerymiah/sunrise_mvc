using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValueObjects;

namespace Sunrise.TransactionManagement.Model.ValueObject
{
    public class BillStatusDictionary : StatusBaseDictionary
    {
        public BillStatusDictionary(string code) : base(code)
        {

        }

        public BillStatusDictionary()
        {

        }

        public static BillStatusDictionary Create(string code)
        {
            return new BillStatusDictionary(code);
        }


        public static BillStatusDictionary CreateActive()
        {
            return new BillStatusDictionary("sscn");
        }

        public static BillStatusDictionary CreatePending()
        {
            return new BillStatusDictionary("ssp");
        }
        

        public bool IsPending() => this.Code == "ssp" ? true : false;
        public bool IsActive() => this.Code == "sscn" ? true : false;

    }
}
