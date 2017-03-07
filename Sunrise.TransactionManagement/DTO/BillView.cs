using Sunrise.TransactionManagement.Model.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.DTO
{
    public class BillView
    {
        public string Id { get; set; }
        public DateTime DateStamp { get; set; }
        public string BillCode { get; set; }
        public string ContractId { get; set; }
        public string ContractCode { get; set; }
        public string ContractStatusDescription { get; set; }
        public decimal Amount { get; set; }

        public string TenantCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string TelNo { get; set; }
        public string MobileNo { get; set; }
        
        public string VillaNo { get; set; }
        public string ElecNo { get; set; }
        public int Capacity { get; set; }
        public string WaterNo { get; set; }
        public string QtelNo { get; set; }
        public string RentalType { get; set; }
        public string VillaStatus { get; set; }
        public decimal RatePerMonth { get; set; }

        public ICollection<PaymentView> Payments { get; set; }
        public ICollection<ReconcileView> Reconciles { get; set; }

        #region
        public decimal TotalReceived
        {
            get
            {
                return this.Payments == null || this.Payments.Count == 0 ? 0 : this.Payments.Sum(p => p.Amount);
            }
        }
        public decimal TotalCleared {
            get
            {
                var payments = this.Payments == null || this.Payments.Count == 0 ? 0 : 
                    this.Payments.Where(p => new PaymentStatusDictionary(p.StatusCode).IsClear()).Sum(p => p.Amount);
                var reconcile = this.Reconciles == null || this.Reconciles.Count == 0 ? 0 :
                    this.Reconciles.Sum(r => r.Amount);
                return payments + reconcile;
            }   
        }
        public decimal TotalBounce
        {
            get
            {
                var payments = this.Payments == null || this.Payments.Count == 0 ? 0 :
                    this.Payments.Where(p => new PaymentStatusDictionary(p.StatusCode).IsBounce()).Sum(p => p.Amount);

                return payments;
            }
        }
        public decimal TotalDishonored
        {
            get
            {
                return TotalBounce * -1;
            }
        }
        public decimal TotalReconcile { get { return this.Reconciles.Sum(r => r.Amount); } }
        public decimal Balance {
            get {
                return Amount - TotalCleared;
            }
        }
        #endregion


    }
}
