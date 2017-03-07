using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.DTO
{
    public class PaymentView
    {   
        public int Id { get; set; }
        public string BillId { get; set; }
        public DateTime PaymentDate { get; private set; }
        public string PaymentTypeDescription { get; set; }
        public string PaymentTypeCode { get; set; }
        public string PaymentModeDescription { get; set; }
        public string PaymentModeCode { get; set; }
        
        public string ChequeNo { get; set; }
        public string BankDescription { get; set; }
        public string BankCode { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public decimal Amount { get; set; }

        public string StatusDescription { get; private set; }
        public string StatusCode { get; set; }
        public DateTime? StatusDate { get; private set; }

        public string Remarks { get; set; }



    }
}
