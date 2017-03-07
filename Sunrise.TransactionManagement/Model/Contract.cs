using Sunrise.TransactionManagement.DTO;
using Sunrise.TransactionManagement.Enum;
using Sunrise.TransactionManagement.Model.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Helper;
using Utilities.ValueObjects;

namespace Sunrise.TransactionManagement.Model
{
    public class Contract
    {
        
        #region Factory
        public static Contract CreateNewEmpty(string code,int defaultMonthPeriod = 0, decimal ratePerMonth = 0)
        {   
            var transaction = new Contract();
            transaction.Code = "C" + code + DateTime.Today.Year.ToString();

            transaction.Period = DateTimeRange.SetRange(defaultMonthPeriod);
            transaction.Amount = Payable.Create(ratePerMonth,transaction.Period);

            return transaction;
        }
        public static Contract CreateRenewEmpty(string id,string code, int defaultMonthPeriod = 0, decimal ratePerMonth = 0,DateTime periodStart = new DateTime())
        {
            var transaction = new Contract();

            transaction.Id = id;
            transaction.Code = code + "-R";
            transaction.Period = DateTimeRange.SetRange(periodStart, defaultMonthPeriod);
            transaction.Amount = Payable.Create(ratePerMonth,transaction.Period);

            return transaction;
        }
        public static Contract Map(string code, string rentalType, string contractStatus, DateTime periodStart, DateTime periodEnd,
            decimal amountPayable, string villaId, string tenantId, string userId)
        {
            var transaction = new Contract
            {
                Code = code,
                RentalType = StatusDictionary.CreateByDefault(rentalType),
                ContractType = StatusDictionary.CreateByDefault(contractStatus),
                Period = DateTimeRange.Create(periodStart, periodEnd),
                Amount = new Payable(amountPayable),
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
            this.IsTerminated = false;
            this.DateCreated = DateTime.Today;
            this.Status = TransactionStatusDictionary.CreatePending();
            this.RentalType = StatusDictionary.CreateByDefault("rtff");
            this.ContractType = StatusDictionary.CreateByDefault("csl");

            this.Amount = new Payable();
        }
        
        public string Id { get; private set; }
        public string Code { get; set; }
        public DateTime DateCreated { get; set; }
        public StatusDictionary RentalType { get; set; }
        public StatusDictionary ContractType { get; set; }
        public DateTimeRange Period { get; set; }
        public Payable Amount { get; set; }
        public TransactionStatusDictionary Status { get; private set; }
        
        public string VillaId { get; set; }
        public string TenantId { get; set; }
        public string UserId { get; set; }

        public bool IsReversed { get; private set; }
        public bool IsTerminated { get; private set; }
        public virtual Terminate Terminate { get; set; }
        
        #region contract status operation
        public void Renew()
        {

        }
        public void TerminateContract(string description,string userId)
        {
            if (Status.IsActive())
            {
                this.Terminate = new Terminate();
                this.Terminate.Description = description;
                this.Terminate.UserId = userId;
                this.Status.Cancelled();
                this.IsTerminated = true;
            }
        }
        public void ActivateStatus()
        {
            if (this.Status.IsPending())
                this.Status.Activate();
        }
        public bool ReversedContract()
        { 
            IsReversed = IsReversed ? false : true;
            if(IsReversed)
            {
                if (this.Status.IsActive())
                    this.Status.Pending();
            }
            return true;
        }
        public bool ContractComplete()
        {
            if(this.Status.IsActive())  
                this.Status.Completed();
            return true;
        }
        #endregion
        
        


    }



}
