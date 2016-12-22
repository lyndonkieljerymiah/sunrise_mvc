using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Domains.ViewModels
{
    public class SalesViewModel
    {
        private readonly IEnumerable<Selection> _selections;
        
        public SalesViewModel(IEnumerable<Selection> selections)
        {
            _selections = selections;
            this.RentalType = _selections.Where(t => t.Type == "RentalType").FirstOrDefault()?.Code;
            this.ContractStatus = _selections.Where(t => t.Type == "ContractStatus").FirstOrDefault()?.Code;
        }

        public SalesViewModel()
        {
            
        }

        [Display(Name = "Villa No")]
        public string VillaNo { get; set; }
      
        [Display(Name = "Rental Type")]
        public string RentalType { get; set; }

        [Display(Name = "Contract Status")]
        public string ContractStatus { get; set; }

        [Display(Name = "Start")]
        public DateTime PeriodStart { get; set; }

        [Display(Name = "End")]
        public DateTime PeriodEnd { get; set; }

        [Display(Name = "Amount")]
        public Decimal Amount { get; set; }

        public IEnumerable<SelectListItem> RentalTypes
        {
            get
            {
                var types = _selections
                    .Where(s => s.Type == "RentalType")
                    .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });
                return types;
            }
        }

        public IEnumerable<SelectListItem> ContractStatuses
        {
            get
            {
                var statuses = _selections
                    .Where(s => s.Type == "ContractStatus")
                    .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });
                return statuses;
            }
        }
    }
}