using Sunrise.Client.Infrastructure.Validations;
using Sunrise.Maintenance.Model;
using Sunrise.TransactionManagement.DTO;
using Sunrise.TransactionManagement.Model.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Utilities.ValueObjects;

namespace Sunrise.Client.Domains.ViewModels
{
    /// <summary>
    /// TODO: For Displaying Sales - Readonly 
    /// </summary>
    public class BillingViewModel
    {


        public BillingViewModel()
        {
            var dt = DateTimeRange.SetRange(12);
            this.PeriodStart = dt.Start;
            this.PeriodEnd = dt.End;
            this.Payments = new HashSet<PaymentViewModel>();
            this.Reconciles = new HashSet<ReconcileViewModel>();
        }

        public string Id { get; set; }
        public string ContractId { get; set; }
        public string BillCode { get; set; }
        public string StatusCode { get; set; }


        public string ContractCode { get; set; }
        public DateTime DateStamp { get; set; }
        public string TransactionStatusCode { get; set; }
        public string TransactionStatusDescription { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Decimal Amount { get; set; }

        public bool PendingState
        {
            get
            {
                if (new BillStatusDictionary(StatusCode).IsPending())
                {
                    return true;
                }
                return false;
            }
        }
        public bool ActiveState
        {
            get
            {
                if (new BillStatusDictionary(StatusCode).IsPending())
                {
                    return true;
                }
                return false;
            }
        }

        //tenant
        public string Name { get; set; }
        public string Address { get; set; }

        //villa
        public string VillaNo { get; set; }
        public string ElecNo { get; set; }
        public int Capacity { get; set; }
        public string WaterNo { get; set; }
        public string QtelNo { get; set; }
        public string RentalType { get; set; }
        public string VillaStatus { get; set; }
        public decimal RatePerMonth { get; set; }

        public ICollection<PaymentViewModel> Payments { get; set; }
        public ICollection<ReconcileViewModel> Reconciles { get; set; }

        public PaymentDictionary PaymentDictionary { get; private set; }

        public decimal TotalReceived { get; set; }
        public decimal TotalCleared { get; set; }
        public decimal Balance { get; set; }

        public void Initialize(IEnumerable<Selection> selections)
        {
            this.PaymentDictionary = new PaymentDictionary(selections);
            var payment = new PaymentViewModel();

            payment.BillId = this.Id;
            payment.PaymentDate = DateTime.Today;
            payment.PeriodStart = DateTime.Today;
            payment.PeriodEnd = DateTime.Today.AddMonths(1);
            payment.PaymentModeCode = PaymentModeDictionary.CreatePayment().Code;
            payment.PaymentTypeCode = PaymentTypeDictionary.CreateCheque().Code;
            payment.PaymentDate = DateTime.Today;
            payment.Amount = this.RatePerMonth;
            this.PaymentDictionary.InitialValue = payment;

            var reconcile = new ReconcileViewModel();

            reconcile.BillId = this.Id;
            reconcile.PaymentTypeCode = PaymentTypeDictionary.CreateCheque().Code;
            reconcile.Date = DateTime.Today;
            reconcile.PeriodStart = DateTime.Today;
            reconcile.PeriodEnd = DateTime.Today.AddMonths(1);

            this.PaymentDictionary.ReconcileInitialValue = reconcile;
        }
    }

    public class PaymentViewModel
    {
        private BillView _billView = new BillView();

        public PaymentViewModel()
        {
            this.StatusCode = (PaymentStatusDictionary.CreateReceived()).Code;
            this.PaymentTypeCode = (PaymentTypeDictionary.CreateCheque()).Code;
            this.PaymentModeCode = (PaymentModeDictionary.CreatePayment()).Code;
            this.BankCode = "bdb";

            //this.BankCode = StatusDictionary.CreateByDefault();

        }

        public int Id { get; set; }
        public string BillId { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public string PaymentTypeCode { get; set; }
        public string PaymentTypeDescription { get; set; }

        [Required]
        public string PaymentModeCode { get; set; }
        public string PaymentModeDescription { get; set; }

        [Required]
        public string ChequeNo { get; set; }

        [CustomRequiredMatchToValidation("PaymentTypeCode", "ptcq", ErrorMessage = "Bank is required")]
        public string BankCode { get; set; }
        public string BankDescription { get; set; }

        [Required]
        [CustomDateEndStartValidation("PeriodEnd", ValueComparison.IsLessThan, ErrorMessage = "Start date must be earlier than end date")]
        public DateTime PeriodStart { get; set; }

        [Required]
        [CustomDateEndStartValidation("PeriodStart", ValueComparison.IsGreaterThan, ErrorMessage = "End date must be later than end date")]
        public DateTime PeriodEnd { get; set; }

        [Required]
        [CustomCurrencyValue]
        public decimal Amount { get; set; }

        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }


        public DateTime? StatusDate { get; set; }
        public string Remarks { get; set; }

        public bool IsModify { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsModifyStatus { get; set; }

        public bool IsClear
        {
            get
            {
                return new PaymentStatusDictionary(StatusCode).IsClear() || this.StatusCode == null ? true : false;
            }
        }
        public bool IsReceived
        {
            get
            {
                return new PaymentStatusDictionary(StatusCode).IsReceived() ? true:false;
            }
        }
    }

    public class PaymentDictionary
    {
        private IEnumerable<Selection> _selections;
        public PaymentDictionary(IEnumerable<Selection> selections)
        {
            _selections = selections;
        }
        public IEnumerable<SelectListItem> Terms
        {
            get
            {
                var types = _selections
                  .Where(s => s.Type == "PaymentTerm")
                  .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

                return types;
            }
        }
        public IEnumerable<SelectListItem> Modes
        {
            get
            {
                var types = _selections
                   .Where(s => s.Type == "PaymentMode")
                   .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

                return types;

            }
        }
        public IEnumerable<SelectListItem> Banks
        {
            get
            {

                var types = _selections
                 .Where(s => s.Type == "Bank")
                 .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

                return types;
            }
        }
        public IEnumerable<SelectListItem> Statuses
        {
            get
            {
                var types = _selections
                .Where(s => s.Type == "PaymentStatus")
                .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

                return types;
            }
        }

        public PaymentViewModel InitialValue { get; set; }
        public ReconcileViewModel ReconcileInitialValue { get; set; }
    }
    public class ReconcileViewModel
    {
        public ReconcileViewModel()
        {

        }
        public int Id { get; set; }
        public string BillId { get; set; }

        [Required]
        public string PaymentTypeCode { get; set; }
        public string PaymentTypeDescription { get; set; }

        public string BankDescription { get; set; }
        public string BankCode { get; set; }

        [Required]
        public string ChequeNo { get; set; }
        [Required]
        public string ReferenceNo { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal DishonouredAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PeriodStart { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PeriodEnd { get; set; }

        public string Remarks { get; set; }

        public bool IsModify { get; set; }
        public bool IsDeleted { get; set; }
    }


}
