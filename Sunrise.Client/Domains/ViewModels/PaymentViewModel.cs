using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Domains.ViewModels
{
    public class PaymentViewModel
    {

        public PaymentViewModel()
        {
            PaymentDate = DateTime.Today;
            CoveredPeriodFrom = DateTime.Today;
            CoveredPeriodTo = DateTime.Today;
        }
        public int Id { get; set; }
        public string SalesTransactionId { get; set; }
        public int VillaId { get; set; }
        public string Term { get; set; }
        public string ChequeNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }

        public string Status { get; set; }
        public string PaymentStatus { get; set; }

        public DateTime? StatusDate { get; set; }
        public string Remarks { get; set; }

        public string Bank { get; set; }

        public string PaymentMode { get; set; }
        public string FullPaymentMode { get; set; }

        public DateTime CoveredPeriodFrom { get; set; }
        public DateTime CoveredPeriodTo { get; set; }
        public string CoveredPeriod => CoveredPeriodFrom.ToShortDateString() + "-" + CoveredPeriodTo.ToShortDateString();
        public string Description { get; set; } 

        public IEnumerable<SelectListItem> Terms { get;private set; }
        public IEnumerable<SelectListItem> Modes { get; set; }

        public void SetTerms(IEnumerable<Selection> selections)
        {
            var types = selections
                   .Where(s => s.Type == "PaymentTerm")
                   .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

            this.Terms = types;
        }

        public void SetMode(IEnumerable<Selection> selections)
        {
            var types = selections
                   .Where(s => s.Type == "PaymentMode")
                   .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

            this.Modes = types;
        }

    }
}
