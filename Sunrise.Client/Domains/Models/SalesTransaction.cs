﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Helpers.Validations;

namespace Sunrise.Client.Domains.Models
{
    public class SalesTransaction
    {

        public static SalesTransaction CreateTransaction(int villaId, string rentalType, string contractStatus,
            DateTime periodStart, DateTime periodEnd, decimal amount,string userId)
        {
            return  new SalesTransaction(villaId,rentalType,contractStatus,periodStart,periodEnd,amount,userId);
        }

        public SalesTransaction(int villaId,string rentalType,string contractStatus,
            DateTime periodStart,DateTime periodEnd,decimal amount,string userId) : this()
        {
            this.RentalType = rentalType;
            this.ContractStatus = contractStatus;
            this.PeriodStart = periodStart;
            this.PeriodEnd = periodEnd;
            this.VillaId = villaId;
            this.UserId = userId;
            this.Amount = amount;
        }

        public SalesTransaction()
        {
            this.DateCreated = DateTime.Today;
            this.Status = "ssp";
            this.Id = Guid.NewGuid().ToString();
            this.Payments = new HashSet<Payment>();
        }
        
        public string Id { get; private set; }
        public DateTime DateCreated { get; private set; }
        public string RentalType { get; set; }
        public string ContractStatus { get; set; }

        
        public DateTime PeriodStart { get; set; }

        
        public DateTime PeriodEnd { get; set; }
        public Decimal Amount { get; set; }
        public string Status { get; set; }
        public int VillaId { get; set; }
        public int TenantId { get; set; }
        public string UserId { get; set; }
        
        public virtual Villa Villa { get; set; }
        public virtual Tenant Tenant { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public void setStatus(string status)
        {
            this.Status = status;
        }

        public void AddPayment(string term,string chequeNo,string bank,string paymentMode,decimal amount,string remarks,DateTime coveredPeriodFrom,DateTime coveredPeriodTo)
        {
            var payment = Payment.Create(term,chequeNo,bank,paymentMode,amount,remarks,coveredPeriodFrom,coveredPeriodTo);
            Payments.Add(payment);
            if (this.Status != "sscn") this.Status = "sscn";
        }
    }
}
