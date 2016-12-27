using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Domains.ViewModels
{

    /// <summary>
    /// TODO: For Displaying Sales - Readonly 
    /// </summary>
    public class SalesViewModel
    {
        public string Id { get; set; }

        public string FullRentalType { get; set; }
        public string FullContractStatus { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Decimal Amount { get; set; }

        public string Status { get; set; }

        public VillaViewModel Villa { get; set; }
        public TenantRegisterViewModel Register { get; set; }
        public ICollection<PaymentViewModel> Payments { get; set; }
    }
  
    /// <summary>
    /// TODO: Register Sales - Write/Read
    /// </summary>
    public class SalesRegisterViewModel
    {   
        
        public static SalesRegisterViewModel CreateWithVilla(VillaViewModel vm)
        {
            var sales = new SalesRegisterViewModel();
            sales.Villa = vm;
            return sales;
        }

        public SalesRegisterViewModel()
        {
            this.RentalTypes = new List<SelectListItem>();
            this.ContractStatuses = new List<SelectListItem>();
        }

        public string Id { get; set; }
        public string VillaNo { get; set; }
        public string RentalType { get; set; }
        public string RentalTypeDescription { get; set; }
        public string ContractStatus { get; set; }
        public string ContractStatusDescription { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Decimal Amount { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }

        public VillaViewModel Villa { get; set; }
        public TenantRegisterViewModel Register { get; set; }
        
        public IEnumerable<SelectListItem> RentalTypes { get; private set; }
        public IEnumerable<SelectListItem> ContractStatuses { get; private set; }

        public void SetContractStatuses(IEnumerable<Selection> selections)
        {
            this.ContractStatuses = selections
                 .Where(s => s.Type == "ContractStatus")
                 .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });
        }
        public void SetRentalTypes(IEnumerable<Selection> selections)
        {
            var types = selections
                    .Where(s => s.Type == "RentalType")
                    .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

            this.RentalTypes = types;
        }

    }


    
    

}