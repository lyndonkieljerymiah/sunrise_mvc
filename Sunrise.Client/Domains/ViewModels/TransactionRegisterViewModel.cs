using Sunrise.Client.Infrastructure.Validations;
using Sunrise.Maintenance.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
namespace Sunrise.Client.Domains.ViewModels
{

   
    /// <summary>
    /// TODO: Register Sales - Write/Read
    /// </summary>
    public class ContractRegisterCreateViewModel
    {
        public ContractRegisterCreateViewModel()
        {
            this.RentalTypes = new List<SelectListItem>();
            this.ContractStatuses = new List<SelectListItem>();
            this.Villa = new VillaViewModel();
            this.Register = new TenantRegisterViewModel();
        }

        public string Id { get; set; }
        public string Code { get; set; }

        [Required]
        public string RentalType { get; set; }
        
        [Required]
        public string ContractStatus { get; set; }
        

        [Required]
        [CustomDateEndStartValidation("PeriodEnd", ValueComparison.IsLessThan, ErrorMessage = "Start must be earlier than end date")]
        public DateTime PeriodStart { get; set; }

        [Required]
        [CustomDateEndStartValidation("PeriodStart", ValueComparison.IsGreaterThan, ErrorMessage = "End must be later than start date")]
        public DateTime PeriodEnd { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Cannot Be null")]
        [CustomCurrencyValue]
        public Decimal Amount { get; set; }

        public string Status { get; set; }
        public string UserId { get; set; }
        public string VillaId { get; set; }
        public string TenantId { get; set; }
        

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

    public class ContractRegisterEditViewModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public DateTime DateCreated { get; set; }

        [Required]
        public string RentalTypeCode { get; set; }
        public string RentalTypeDescription { get; set; }
        [Required]
        public string ContractStatusCode { get; set; }
        public string ContractStatusDescription { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PeriodStart { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PeriodEnd { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public string StatusCode { get; private set; }
        public string StatusDescription { get; set; }

        public string VillaId { get; set; }
        public string VillaNo { get; set; }
        public decimal RatePerMonth { get; set; }
        public string TenantId { get; set; }
        public string UserId { get; set; }

        public bool IsReversed { get; private set; }
        public bool IsTerminated { get; private set; }

        //tenant
        public string Name { get; set; }
        public string Address { get; set; }
    }






}