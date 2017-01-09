using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Sunrise.Client.Helpers.Validations;
using Sunrise.Maintenance.Model;

namespace Sunrise.Client.Domains.ViewModels
{

    /// <summary>
    /// TODO: For Displaying Sales - Readonly 
    /// </summary>
    public class BillingViewModel
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Code { get; set; }
        public string RentalType { get; set; }
        public string ContractStatus { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Decimal AmountPayable { get; set; }

        public string Status { get; set; }
        public string StatusCode { get; set; }
        
        public VillaViewModel Villa { get; set; }

        //tenant
        public string Name { get; set; }
        public string Address { get; set; }
        public string QatarId { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? ValidityDate { get; set; }
        public string CrNo { get; set; }
        

        public ICollection<PaymentViewModel> Payments { get; set; }
        
        public PaymentDictionary PaymentDictionary { get; private set; }



        public decimal TotalPayment
        {
            get
            {
                decimal totalPayment = 0;
                if (Payments != null && Payments.Count > 0)
                {
                    totalPayment = Payments
                        .Where(p => p.StatusCode == "psc")
                        .Sum(p => p.Amount);
                }
                return totalPayment;
            }

        }
        public decimal TotalBalance { get
            {
                var totalBalance = AmountPayable - TotalPayment;
                return totalBalance;
            } 

        }
        
        public void SetCheatSheet()
        {
            foreach(var payment in Payments)
            {
                payment.VillaId = this.Villa.Id;
            }
        }
        public void Initialize(IEnumerable<Selection> selections)
        {
            this.PaymentDictionary = new PaymentDictionary(selections);
            var payment = new PaymentViewModel(true);

            payment.TransactionId = this.Id;
            payment.VillaId = this.Villa.Id;
            this.PaymentDictionary.InitialValue = payment;
        }
    }
  
    /// <summary>
    /// TODO: Register Sales - Write/Read
    /// </summary>
    public class TransactionRegisterViewModel
    {   
        
        public static TransactionRegisterViewModel CreateWithVilla(VillaViewModel vm)
        {
            var sales = new TransactionRegisterViewModel();
            sales.Villa = vm;
            return sales;
        }

        public TransactionRegisterViewModel()
        {
            this.RentalTypes = new List<SelectListItem>();
            this.ContractStatuses = new List<SelectListItem>();
            this.PeriodStart = DateTime.Today;
            this.PeriodEnd = DateTime.Today.AddYears(1);

        }

        public string Id { get; set; }

        [Required]
        public string RentalType { get; set; }
        public string RentalTypeDescription { get; set; }

        [Required]
        public string ContractStatus { get; set; }
        public string ContractStatusDescription { get; set; }

        [Required]
        [CustomDateEndStartValidation("PeriodEnd", ValueComparison.IsLessThan,ErrorMessage = "Start must be earlier than end date")]
        [CustomDateCurrentValidation(ErrorMessage = "Start date must be current or later date")]
        public DateTime PeriodStart { get; set; }

        [Required]
        [CustomDateEndStartValidation("PeriodStart", ValueComparison.IsGreaterThan, ErrorMessage = "End must be later than start date")]
        public DateTime PeriodEnd { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Cannot Be null")]
        [CustomCurrencyValue]
        public Decimal AmountPayable { get; set; }

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

        public void ComputeTotalAmount()
        {
            var totalDays = (this.PeriodEnd.Date - this.PeriodStart.Date).TotalDays;
            var totalMonth = Convert.ToInt16(totalDays)/30;
            var totalAmountPerDay = this.Villa.RatePerMonth* (totalMonth);
            this.AmountPayable = totalAmountPerDay;
        }
      
       
    }


    


    
    

}