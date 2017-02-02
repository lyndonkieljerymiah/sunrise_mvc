using Sunrise.TransactionManagement.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Helper;

namespace Sunrise.TransactionManagement.Model
{
    public class Contract
    {   

        #region Factory
        public static Contract CreateNewEmpty(string code,int defaultMonthPeriod = 0, decimal ratePerMonth = 0, DateTime periodStart = new DateTime(), IRateCalculation rateCalculation = null)
        {   
            var transaction = new Contract();
            transaction.Code = "C" + code + DateTime.Today.Year.ToString();
            transaction.PeriodStart = periodStart;
            transaction.PeriodEnd = periodStart.AddMonths(defaultMonthPeriod);
            transaction.AmountPayable = rateCalculation.Calculate(ratePerMonth, transaction.PeriodStart.Date, transaction.PeriodEnd.Date);

            return transaction;
        }
        public static Contract CreateRenewEmpty(string id,string code, int defaultMonthPeriod = 0, decimal ratePerMonth = 0, DateTime periodStart = new DateTime(), IRateCalculation rateCalculation = null)
        {
            var transaction = new Contract();

            transaction.Id = id;
            transaction.Code = code + "-R"+ DateTime.Today.Year;
            transaction.PeriodStart = periodStart;
            transaction.PeriodEnd = periodStart.AddMonths(defaultMonthPeriod);
            transaction.AmountPayable = rateCalculation.Calculate(ratePerMonth, transaction.PeriodStart.Date, transaction.PeriodEnd.Date);

            return transaction;
        }
        public static Contract Map(string code, string rentalType, string contractStatus, DateTime periodStart, DateTime periodEnd,
            decimal amountPayable, string villaId, string tenantId, string userId)
        {
            var transaction = new Contract
            {
                Code = code,
                RentalType = rentalType,
                ContractStatus = contractStatus,
                PeriodStart = periodStart,
                PeriodEnd = periodEnd,
                AmountPayable = amountPayable,
                VillaId = villaId,
                TenantId = tenantId,
                UserId = userId
            };

            return transaction;
        }
        #endregion
                
        public Contract()
        {
            this.Id = Guid.NewGuid().ToString();
            this.DateCreated = DateTime.Today;
            this.Status = ContractStatusSelection.Pending;
            this.Payments = new HashSet<Payment>();
            this.IsTerminated = false;
            this.RentalType = "rtff";
            this.ContractStatus = "csl";
            this.DateCreated = DateTime.Today;
        }
        
        public string Id { get; private set; }
        public string Code { get; set; }
        public DateTime DateCreated { get; set; }

        public string RentalType { get; set; }
        public string ContractStatus { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Decimal AmountPayable { get; set; }

        public string Status { get; private set; }

        public string VillaId { get; set; }
        public string TenantId { get; set; }
        public string UserId { get; set; }
        public bool IsReversed { get; private set; }
        public bool IsTerminated { get; private set; }
        
        public virtual Terminate Terminate { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
              

        #region method
     
        public decimal GetBalanceDue()
        {
            return this.Payments.Where(p => p.Status != PaymentStatusSelection.Clear).Sum(p => p.Amount);
        }


        #region crud payment
        public bool AddPayment(
           DateTime paymentDate, string paymentType, string paymentMode, string chequeNo,
            string bank, DateTime coveredFrom, DateTime coveredTo,
            decimal amount, string remarks,string userId)
        {

            //no  existing covered date
            Payment payment = null;

            bool hasCollision = paymentMode == "ptcq" ? IsPaymentDateCovered(coveredFrom.Date) : false;
            if (!hasCollision)
            {
                payment = Payment.Map(paymentDate, paymentType, paymentMode, chequeNo, bank, coveredFrom, coveredTo, amount, remarks,userId);
                //payment is cash must be status cleared
                if (paymentType == "ptcs")
                {
                    payment.SetStatus("psc", "",userId);
                }
                Payments.Add(payment);
                return true;
            }
            return false;
        }

        public bool UpdatePayment(int id,
           DateTime paymentDate, string paymentType, string paymentMode, string chequeNo,
            string bank, DateTime coveredFrom, DateTime coveredTo,
            decimal amount, string remarks, string userId) {

            var payment = Payments.SingleOrDefault(p => p.Id == id);
            bool hasCollision = paymentMode == "ptcq" ? IsPaymentDateCovered(coveredFrom.Date) : false;
            if (!hasCollision)
            {
                payment.PaymentDate = paymentDate;
                payment.PaymentType = paymentType;
                payment.PaymentMode = paymentMode;
                payment.ChequeNo = chequeNo;
                payment.Bank = bank;
                payment.CoveredPeriodFrom = coveredFrom;
                payment.CoveredPeriodTo = coveredTo;
                payment.Amount = amount;
                payment.LogStamp(userId);
                return true;
            }

            return false;
        }

        public void UpdatePaymentStatus(int id, string status, string remarks,string userId)
        {
            var payment = this.Payments.SingleOrDefault(p => p.Id == id);
            if (payment == null)
                throw new Exception("Cannot find payment");
            
            //shouldn't update if it's the same value 
            //means not dirty
            if (payment.Status != status)
                payment.SetStatus(status, remarks,userId);

        }
        #endregion


        #region contract status operation
        public void TerminateContract(string description,string userId)
        {

            if(this.Terminate == null) this.Terminate = new Terminate();

            this.Terminate.Description = description;
            this.Terminate.UserId = userId;
            this.Status = ContractStatusSelection.Cancelled;
            this.IsTerminated = true;
        }
        public void ActivateStatus()
        {
            if (this.Status == ContractStatusSelection.Pending)
                this.Status = ContractStatusSelection.Active;
        }
        public bool ReversedContract()
        {
            //make sure no clear payment
            var clearPaymentCount = this.Payments.Where(p => p.Status == PaymentStatusSelection.Clear).Count();
            if (clearPaymentCount > 0)
                return false;
            
            IsReversed = IsReversed ? false : true;
            if(IsReversed)
            {
                if (IsActive())
                    this.Status = ContractStatusSelection.Pending;
            }
            return true;
        }
        public bool ContractCompletion()
        {
            var unclearPayment = this.Payments.Where(p => p.Status != PaymentStatusSelection.Clear).Sum(p => p.Amount);
            if (unclearPayment != 0)
                return false; //still has unclear payment
            this.Status = ContractStatusSelection.Completed; //contract is completed
            return true;
        }
        #endregion
        

        #region conditional 
        public bool IsPaymentDateCovered(DateTime value,int id=0)
        {
            var existingDate = 0;
            if (Payments.Count > 0)
            {
                if (id == 0)
                {
                    existingDate = this.Payments
                        .Where(p => value.Date >= p.CoveredPeriodFrom.Date &&
                                value.Date < p.CoveredPeriodTo.Date && p.PaymentMode == "pmp").Count();
                }
                else
                {
                    //exclude the selected payment
                    existingDate = this.Payments
                        .Where(p => value.Date >= p.CoveredPeriodFrom.Date &&
                                value.Date < p.CoveredPeriodTo.Date && p.PaymentMode == "pmp" && p.Id != id).Count();

                    var ps = this.Payments
                        .Where(p => value.Date >= p.CoveredPeriodFrom.Date &&
                                value.Date < p.CoveredPeriodTo.Date && p.PaymentMode == "pmp" && p.Id != id).ToList();
                }
            }

            return existingDate > 0 ? true : false;
        }
        public bool IsActive()
        {
            if (this.Status == "sscn")
                return true;
            return false;
        }
        public bool HasRemainingBalance()
        {
            var paidAmount = this.Payments.Where(p => p.Status == PaymentStatusSelection.Clear).Sum(p => p.Amount);
            if (paidAmount >= this.AmountPayable)
            {
                return false;
            }
            return true;
        }
        #endregion


        #endregion


    }


   
}
