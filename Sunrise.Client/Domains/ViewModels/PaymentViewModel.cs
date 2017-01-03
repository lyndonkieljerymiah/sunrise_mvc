using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Sunrise.Client.Helpers.Validations;
using Sunrise.Maintenance.Model;

namespace Sunrise.Client.Domains.ViewModels
{
    public class PaymentViewModel
    {



        public PaymentViewModel()
        {
            PaymentDate = DateTime.Today;
            CoveredPeriodFrom = DateTime.Today;
            CoveredPeriodTo = CoveredPeriodFrom.Date.AddMonths(1);
            this.Bank = "";
            this.Term = "ptcq";
            this.PaymentMode = "pmp";
        }

        public int Id { get; set; }

        public string SalesTransactionId { get; set; }
        public string VillaId { get; set; }

        [Required]
        public string Term { get; set; }

        [Required]
        public string ChequeNo { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [CustomCurrencyValue]
        public decimal Amount { get; set; }


        public string Status { get; set; }

        public string PaymentStatus { get; set; }

        public DateTime? StatusDate { get; set; }

        public string Remarks { get; set; }

        [CustomRequiredMatchToValidation("Term","ptcq",ErrorMessage = "Bank is required")]
        public string Bank { get; set; }

        public string PaymentMode { get; set; }

        public string FullPaymentMode { get; set; }

        [Required]
        [CustomDateEndStartValidation("CoveredPeriodTo", ValueComparison.IsLessThan, ErrorMessage = "Start date must be earlier than end date")]
        [CustomDateCurrentValidation(ErrorMessage = "Start date must be current or later date")]
        public DateTime CoveredPeriodFrom { get; set; }

        [Required]
        [CustomDateEndStartValidation("CoveredPeriodFrom", ValueComparison.IsGreaterThan, ErrorMessage = "End date must be later than end date")]
        public DateTime CoveredPeriodTo { get; set; }

        public string CoveredPeriod => CoveredPeriodFrom.ToShortDateString() + "-" + CoveredPeriodTo.ToShortDateString();
        

        public IEnumerable<SelectListItem> Terms { get;private set; }
        public IEnumerable<SelectListItem> Modes { get; set; }
        public IEnumerable<SelectListItem> Banks { get; set; }

        public void SetTerms(IEnumerable<Selection> selections)
        {
            var types = selections
                   .Where(s => s.Type == "PaymentTerm")
                   .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

            this.Terms = types;
        }

        public void SetBank(IEnumerable<Selection> selections)
        {
            var types = selections
                  .Where(s => s.Type == "Bank")
                  .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });
            this.Banks = types;
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
