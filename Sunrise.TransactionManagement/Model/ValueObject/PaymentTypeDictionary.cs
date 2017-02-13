using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValueObjects;

namespace Sunrise.TransactionManagement.Model.ValueObject
{
    public class PaymentTypeDictionary : StatusBaseDictionary
    {
        public static PaymentTypeDictionary CreateCheque()
        {
            return new PaymentTypeDictionary("ptcq");
        }

        public static PaymentTypeDictionary CreateCash()
        {
            return new PaymentTypeDictionary("ptcs");
        }

        public PaymentTypeDictionary(string code)
        {
            this.Code = code;
        }

        public PaymentTypeDictionary()
        {

        }

        public bool IsCheque() => this.Code == "ptcq" ? true : false;
        public bool IsCash() => this.Code == "ptcs" ? true : false;
    }
}
