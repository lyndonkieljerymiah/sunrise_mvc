﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.Model
{
    public class Payment
    {

        public static Payment CreateNew()
        {
            return new Payment();
        }

        public static Payment Map(DateTime paymentDate,string paymentType, string paymentMode, string chequeNo,
            string bank, DateTime coveredFrom, DateTime coveredTo,
            decimal amount,string remarks)
        {
            return new Payment
            {  
                PaymentDate = paymentDate,
                PaymentType = paymentType,
                PaymentMode = paymentMode,
                ChequeNo = chequeNo,
                Bank = bank,
                CoveredPeriodFrom = coveredFrom,
                CoveredPeriodTo = coveredTo,
                Amount = amount,
                Remarks = remarks
            };
        }

        public Payment()
        {
            this.PaymentDate = DateTime.Today;
            this.StatusDate = DateTime.Today;
            this.PaymentType = "ptcq";
            this.PaymentMode = "pmp";
            this.Status = "psv";
        }

        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMode { get; set; }
       
        public string ChequeNo { get; set; }
        public string Bank { get; set; }

        public DateTime CoveredPeriodFrom { get; set; }
        public DateTime CoveredPeriodTo { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; private set; }
        public DateTime? StatusDate { get; private set; }

        public string Remarks { get; set; }

        public virtual Transaction Transaction { get; set; }

        public void SetStatus(string status,string remarks)
        {   
            this.Status = status;
            this.Remarks = remarks;
            this.StatusDate = DateTime.Today;
        }
    }
}
