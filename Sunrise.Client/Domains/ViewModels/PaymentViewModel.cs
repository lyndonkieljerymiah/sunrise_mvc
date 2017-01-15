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
        public PaymentViewModel(bool enabledWriteState = false)
        {
            PaymentDate = DateTime.Today;
            CoveredPeriodFrom = DateTime.Today;
            CoveredPeriodTo = CoveredPeriodFrom.Date.AddMonths(1);
            this.BankCode = "";
            this.PaymentTypeCode = "ptcq";
            this.PaymentModeCode = "pmp";
        }

        public int Id { get; set; }

        public string TransactionId { get; set; }
        public string VillaId { get; set; }

        
        public string PaymentType { get; set; }
        [Required]
        public string PaymentTypeCode { get; set; }

        [Required]
        public string ChequeNo { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [CustomCurrencyValue]
        public decimal Amount { get; set; }
        
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string PaymentStatus { get; set; }

        public bool WriteState {
            get {
                return this.StatusCode == "psv" || this.StatusCode == null ? true : false;
            }
        }
        public DateTime? StatusDate { get; set; }
        public string Remarks { get; set; }
        public string Bank { get; set; }

        [CustomRequiredMatchToValidation("PaymentTypeCode", "ptcq", ErrorMessage = "Bank is required")]
        public string BankCode { get; set; }

        public string PaymentMode { get; set; }
        public string PaymentModeCode { get; set; }
        

        [Required]
        [CustomDateEndStartValidation("CoveredPeriodTo", ValueComparison.IsLessThan, ErrorMessage = "Start date must be earlier than end date")]
        [CustomDateCurrentValidation(ErrorMessage = "Start date must be current or later date")]
        public DateTime CoveredPeriodFrom { get; set; }
        
        [Required]
        [CustomDateEndStartValidation("CoveredPeriodFrom", ValueComparison.IsGreaterThan, ErrorMessage = "End date must be later than end date")]
        public DateTime CoveredPeriodTo { get; set; }
        
    }


    public class PaymentDictionary
    {
        private IEnumerable<Selection> _selections;

        public PaymentDictionary(IEnumerable<Selection> selections)
        {
            _selections = selections;
        }

        public IEnumerable<SelectListItem> Terms
        {
            get
            {
                var types = _selections
                  .Where(s => s.Type == "PaymentTerm")
                  .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

                return types;
            }
        }
        public IEnumerable<SelectListItem> Modes
        {
            get
            {
                var types = _selections
                   .Where(s => s.Type == "PaymentMode")
                   .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

                return types;

            }
        }
        public IEnumerable<SelectListItem> Banks {
            get
            {
                var types = _selections
                 .Where(s => s.Type == "Bank")
                 .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

                return types;
            }
        }
        public IEnumerable<SelectListItem> Statuses { get
            {
                var types = _selections
                .Where(s => s.Type == "PaymentStatus")
                .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

                return types;
            }
        }

        public PaymentViewModel InitialValue { get; set; }
    }
}
