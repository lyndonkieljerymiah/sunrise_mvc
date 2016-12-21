using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.Models
{
    public class Payment
    {
        public int Term { get; set; }
        public string ChequeNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }

        public int Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string Remarks { get; set; }

        public virtual  SalesTransaction SalesTransaction { get; set; }
        
    }
}
