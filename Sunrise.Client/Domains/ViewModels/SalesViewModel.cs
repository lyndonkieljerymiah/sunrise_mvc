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
        
        public static SalesViewModel CreateWithVilla(VillaViewModel vm)
        {
            var sales = new SalesViewModel();
            sales.Villa = vm;
            return sales;
        }

        public SalesViewModel()
        {
            this.RentalTypes = new List<SelectListItem>();
            this.ContractStatuses = new List<SelectListItem>();
        }

        public string Id { get; set; }

        [Display(Name = "Villa No")]
        public string VillaNo { get; set; }
        
        [Display(Name = "Rental Type")]
        public string RentalType { get; set; }

        public string RentalTypeDescription { get; set; }

        [Display(Name = "Contract Status")]
        public string ContractStatus { get; set; }

        public string ContractStatusDescription { get; set; }

        [Display(Name = "Start")]
        public DateTime PeriodStart { get; set; }

        [Display(Name = "End")]
        public DateTime PeriodEnd { get; set; }

        [Display(Name = "Amount")]
        public Decimal Amount { get; set; }

        public string Status { get; set; }

        public VillaViewModel Villa { get; set; }
        
        public IEnumerable<SelectListItem> RentalTypes { get; private set; }
        
        public void SetRentalTypes(IEnumerable<Selection> selections) 
        {
            var types = selections
                    .Where(s => s.Type == "RentalType")
                    .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

            this.RentalTypes = types;
        }

        public IEnumerable<SelectListItem> ContractStatuses { get; private set; }

        public void SetContractStatuses(IEnumerable<Selection> selections)
        {
            this.ContractStatuses = selections
                 .Where(s => s.Type == "ContractStatus")
                 .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });
        }



    }


    
    

}