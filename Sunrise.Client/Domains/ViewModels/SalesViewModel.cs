using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sunrise.Client.Domains.ViewModels
{
    public class SalesViewModel
    {
        [Display(Name = "Villa No")]
        public string VillaNo { get; set; }

        [Display(Name = "Electric No")]
        public string ElectricNo { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

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
                return new List<SelectListItem>
                {
                    new SelectListItem() { Text = "Semi-Furnished", Value= "sf",Selected = true},
                    new SelectListItem() { Text = "Full Furnished", Value= "ff"}
                };
            }
        }

        public IEnumerable<SelectListItem> ContractStatuses
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem() { Text = "Legalized", Value= "leg",Selected = true},
                    new SelectListItem() { Text = "Monthly Basis", Value= "mb"}
                };
            }
        }
    }
}