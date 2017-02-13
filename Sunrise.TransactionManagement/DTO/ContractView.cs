using System;
using Utilities.ValueObjects;

namespace Sunrise.TransactionManagement.DTO
{
    public class ContractView
    {
        public string Id { get; private set; }
        public string Code { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string Name { get; set; }
        public string VillaNo { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }

        public int MonthDue {
            get {
                var dt = DateTimeRange.Create(PeriodStart, PeriodEnd);
                return dt.GetMonthValue(DateTime.Today);
            }
        }
    }

    public class ContractSingleView
    {

        public string Id { get; set; }
        public string Code { get; set; }
        public DateTime DateCreated { get; set; }
        public string RentalTypeCode { get; set; }
        public string RentalTypeDescription { get; set; }
        public string ContractStatusCode { get; set; }
        public string ContractStatusDescription { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal Amount { get; set; }
        public string StatusCode { get; private set; }
        public string StatusDescription { get; set; }

        public string VillaId { get; set; }
        public string VillaNo { get; set; }
        public string ElecNo { get; set; }
        public string WaterNo { get; set; }
        public string QTelNo { get; set; }
        public decimal RatePerMonth { get; set; }
        public string VillaStatus { get; set; }
        public string TenantId { get; set; }
        public string UserId { get; set; }

        public bool IsReversed { get; private set; }
        public bool IsTerminated { get; private set; }
        
        //tenant
        public string Name { get; set; }
        public string Address { get; set; }
    }

}
