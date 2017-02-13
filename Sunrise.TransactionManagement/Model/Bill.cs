using Sunrise.TransactionManagement.Model.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValueObjects;

namespace Sunrise.TransactionManagement.Model
{
    public class Bill
    {
        #region factory
        public static Bill Create(Contract contract)
        {
            var bill = new Bill(contract.Code);
            bill.ContractId = contract.Id;
            bill.Contract = contract;
            return bill;
        }
        #endregion
        
        public Bill(string code) : this()
        {   
            this.Code = "B-" + code;
        }

        public Bill()
        {
            this.Id = Guid.NewGuid().ToString();
            this.DateStamp = DateTime.Today;
            this.Status = BillStatusDictionary.CreatePending();
            this.Payments = new HashSet<Payment>();
        }

        public string Id { get; private set; }
        public string ContractId { get; set; }
        public string Code { get; private set; }
        public DateTime DateStamp { get; private set; }
        public BillStatusDictionary Status { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Reconcile> Reconciles { get; set; }
        
        public void ActivateBill()
        {
            this.Status = BillStatusDictionary.CreateActive();
        }

        #region method payment
        public bool AddPayment(
           DateTime paymentDate, string paymentType, string paymentMode, string chequeNo,
            string bank, DateTime coveredFrom, DateTime coveredTo,
            decimal amount, string remarks, string userId)
        {

            //no  existing covered date
            Payment payment = null;
            var paymentModeDictionary = new PaymentModeDictionary(paymentMode);
            var paymentTypeDictionary = new PaymentTypeDictionary(paymentType);

            if (paymentModeDictionary.IsSecurityDeposit() || IsNotCovered(coveredFrom.Date))
            {
                //create payment
                payment = Payment.Create(paymentDate, paymentTypeDictionary, paymentModeDictionary, chequeNo, bank,DateTimeRange.Create(coveredFrom,coveredTo), amount, remarks, userId);
                //payment is cash must be status cleared
                if (paymentTypeDictionary.IsCash())
                {
                    var clearStatus = PaymentStatusDictionary.CreateClear();
                    payment.SetStatus(clearStatus.Code, "", userId);
                }
                Payments.Add(payment);
                return true;
            }
            return false;
        }
        
        public bool UpdatePayment(int id,
          DateTime paymentDate, string paymentType, string paymentMode, string chequeNo,
           string bank, DateTime coveredFrom, DateTime coveredTo,
           decimal amount, string remarks, string userId)
        {

            var payment = Payments.SingleOrDefault(p => p.Id == id);
            if (payment == null)
            {
                throw new Exception("Cannot find payment");
            }
            var paymentModeDictionary = new PaymentModeDictionary(paymentMode);
            var paymentTypeDictionary = new PaymentTypeDictionary(paymentType);
            if (paymentModeDictionary.IsSecurityDeposit() || IsNotCovered(coveredFrom.Date,payment.Id))
            {
                payment.PaymentDate = paymentDate;
                payment.PaymentType = paymentTypeDictionary;
                payment.PaymentMode = paymentModeDictionary;
                payment.ChequeNo = chequeNo;
                payment.Bank = new StatusDictionary(bank);
                payment.Period = DateTimeRange.Create(coveredFrom, coveredTo);
                payment.Amount = amount;
                payment.Remarks = remarks;
                payment.LogStamp(userId);
                return true;
            }
            return false;
        }
        
        public bool UpdatePayment(int id,
          DateTime paymentDate, string paymentType, string paymentMode, string chequeNo,
           string bank, DateTime coveredFrom, DateTime coveredTo,
           decimal amount, string remarks, string status, string userId)
        {
            var payment = Payments.SingleOrDefault(p => p.Id == id);
            if (payment == null)
            {
                throw new Exception("Cannot find payment");
            }
            var paymentModeDictionary = new PaymentModeDictionary(paymentMode);
            var paymentTypeDictionary = new PaymentTypeDictionary(paymentType);
            if (paymentModeDictionary.IsSecurityDeposit() || IsNotCovered(coveredFrom.Date, payment.Id))
            {
                payment.PaymentDate = paymentDate;
                payment.PaymentType = paymentTypeDictionary;
                payment.PaymentMode = paymentModeDictionary;
                payment.ChequeNo = chequeNo;
                payment.Bank = new StatusDictionary(bank);
                payment.Period = DateTimeRange.Create(coveredFrom, coveredTo);
                payment.Amount = amount;
                payment.SetStatus(status, remarks, userId);
                return true;
            }
            return false;

        }
        
        public void RemovePayment(int id)
        {
            var payment = this.Payments.SingleOrDefault(p => p.Id == id);
            if(payment != null)
            {
                payment.IsMarkDeleted = true;
            }
        }

        private bool IsNotCovered(DateTime value, int id = 0)
        {
            var existingDate = 0;
            if (Payments.Count > 0)
            {
                if (id == 0)
                {
                    existingDate = this.Payments
                        .Where(p => value.Date >= p.Period.Start &&
                                value.Date < p.Period.End  && !p.PaymentMode.IsSecurityDeposit()).Count();
                }
                else
                {
                    //exclude the selected payment
                    existingDate = this.Payments
                        .Where(p => value.Date >= p.Period.Start &&
                                value.Date < p.Period.End && !p.PaymentMode.IsSecurityDeposit() && p.Id != id).Count();
                }
            }
            return existingDate > 0 ? false : true;
        }
        #endregion


        #region Reconcile
        public void AddReconcile(string chequeNo,string paymentType,string referenceNo,string bank,decimal dishonouredAmount,decimal amount,DateTime periodStart,DateTime periodEnd,string remarks)
        {
            var reconcile = Reconcile.Create(chequeNo,paymentType, referenceNo,bank, dishonouredAmount, amount, DateTimeRange.Create(periodStart, periodEnd), remarks);
            Reconciles.Add(reconcile);

        }
        public void UpdateReconcile(int id, string chequeNo, string referenceNo,string bank, decimal dishonouredAmount, decimal amount, DateTime periodStart, DateTime periodEnd, string remarks)
        {

            var reconcile = Reconciles.SingleOrDefault(r => r.Id == id);
            if(reconcile != null)
            {
                reconcile.ChequeNo = chequeNo;
                reconcile.ReferenceNo = referenceNo;
                reconcile.Bank = StatusDictionary.CreateByDefault(bank);
                reconcile.DishonouredAmount = dishonouredAmount;
                reconcile.Amount = amount;
                reconcile.Period = DateTimeRange.Create(periodStart, periodEnd);
                reconcile.Remarks = remarks;
            }
            Reconciles.Add(reconcile);

        }
        public void RemoveReconcile(int id)
        {
            var reconcile = Reconciles.SingleOrDefault(r => r.Id == id);
            reconcile.IsMarkDeleted = true;
        }
        #endregion
        
        #region check balance due
        public decimal GetBalanceDue()
        {
            //get the payment first
            decimal paymentAmount = Payments.Count > 0 ? Payments.Where(p => p.Status.IsClear()).Sum(p => p.Amount) : 0;
            decimal reconcileAmount = Reconciles.Count > 0 ? Reconciles.Sum(r => r.Amount) : 0;
            var totalPaidAmount = paymentAmount + reconcileAmount;
            return Contract.Amount.GetBalance(totalPaidAmount);
        }
        #endregion

    }
}
