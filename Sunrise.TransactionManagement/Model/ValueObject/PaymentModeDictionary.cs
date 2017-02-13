using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValueObjects;

namespace Sunrise.TransactionManagement.Model.ValueObject
{
    public class PaymentModeDictionary : StatusBaseDictionary
    {

        public static PaymentModeDictionary CreatePayment()
        {
            return new PaymentModeDictionary("pmp");
        }

        public static PaymentModeDictionary CreateDeposit()
        {
            return new PaymentModeDictionary("pmsd");
        }

        public PaymentModeDictionary(string code)
        {
            this.Code = code;
        }

        public PaymentModeDictionary()
        {

        }

        public bool IsPayment() => this.Code == "pmp" ? true : false;
        public bool IsSecurityDeposit() => this.Code == "pmp" ? true : false;


    }
}
