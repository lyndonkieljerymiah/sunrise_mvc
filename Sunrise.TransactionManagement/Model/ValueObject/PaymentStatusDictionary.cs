using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValueObjects;

namespace Sunrise.TransactionManagement.Model.ValueObject
{
    public class PaymentStatusDictionary : StatusBaseDictionary
    {


        public static PaymentStatusDictionary CreateClear()
        {
            return new PaymentStatusDictionary("psc");
        }

        public static PaymentStatusDictionary CreateHold()
        {
            return new PaymentStatusDictionary("psh");
        }
        public static PaymentStatusDictionary CreateReceived()
        {
            return new PaymentStatusDictionary("psv");
        }
        public static PaymentStatusDictionary CreateBounce()
        {
            return new PaymentStatusDictionary("psb");
        }

        public PaymentStatusDictionary(string code)
        {
            this.Code = code;
        }
        public PaymentStatusDictionary()
        {

        }
        public bool IsClear() => this.Code == "psc" ? true : false;
        public bool IsHold() => this.Code == "psh" ? true : false;
        public bool IsReceived() => this.Code == "psv" ? true : false;
        public bool IsBounce() => this.Code == "psb" ? true : false;

    }
}
