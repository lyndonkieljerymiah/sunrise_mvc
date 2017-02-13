using Sunrise.TransactionManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.DTO
{
    public class TransactionListDTO
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Code { get; set; }
        public string TenantName { get; set; }
        public string VillaNo { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }

        public decimal AmountPayable { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal AmountBalance { get; set; }
    }
}
