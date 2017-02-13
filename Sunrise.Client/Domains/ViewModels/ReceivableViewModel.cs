using Sunrise.Maintenance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.ViewModels
{
    public class ReceivableViewModel
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

        //tenant
        public string Name { get; set; }
        public string Address { get; set; }
        public string QatarId { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? ValidityDate { get; set; }
        public string CrNo { get; set; }
        public bool UpdateState { get; set; }

        public ICollection<PaymentViewModel> Payments { get; set; }
        public VillaViewModel Villa { get; set; }

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
        public decimal TotalBalance
        {
            get
            {
                var totalBalance = AmountPayable - TotalPayment;
                return totalBalance;
            }

        }
        public decimal TotalReceivedPayment
        {
            get
            {
                decimal totalPayment = 0;
                if (Payments != null)
                    totalPayment = this.Payments.Sum(p => p.Amount);
                return totalPayment;
            }
        }
        
        public void Initialize(IEnumerable<Selection> selections)
        {
            this.PaymentDictionary = new PaymentDictionary(selections);
            var payment = new PaymentViewModel(true);

            payment.TransactionId = this.Id;
            payment.VillaId = this.Villa.Id;
            payment.Amount = this.Villa.RatePerMonth;

            this.PaymentDictionary.InitialValue = payment;
        }

        public void SetEditMode()
        {   
            UpdateState = Payments.Where(p => p.IsClear == true).Count() == 0 ? false : true;
        }
    }
}
