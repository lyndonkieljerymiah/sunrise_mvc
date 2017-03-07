using System.Collections.Generic;
using Sunrise.Maintenance.Model;
using System;
using Sunrise.TransactionManagement.Model.ValueObject;
using Utilities.ValueObjects;

namespace Sunrise.Client.Domains.ViewModels
{

    public class ContractListViewModel
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string VillaNo { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public string ContractStatusDescription { get; set; }
        public string ContractStatusCode { get; set; }

        public decimal AmountPayable { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal AmountBalance { get; set; }
        public int MonthDue { get; set; }

        public bool EditState { get { return new TransactionStatusDictionary(ContractStatusCode).IsPending(); } }

    }

}
