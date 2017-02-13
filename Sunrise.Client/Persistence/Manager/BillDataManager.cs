using AutoMapper;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.TransactionManagement.Model;
using Sunrise.TransactionManagement.Model.ValueObject;
using Sunrise.TransactionManagement.Persistence.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.Client.Persistence.Manager
{
    public class BillDataManager
    {
        private IUnitOfWork UOW { get; }

        public BillDataManager(IUnitOfWork unitOfWork)
        {
            UOW = unitOfWork;
        }
        public async Task<BillingViewModel> GenerateBill(string contractId)
        {
            //check whether the contract has a pending bill exist
            var existingBill = await UOW.Bills.GetBillByContractCode(contractId);
            BillingViewModel billViewModel = null;
            if (existingBill == null)
            {
                //get contract first 
                var contract = await UOW.Contracts.FindQueryAsync(contractId);
                if (contract == null) return null;

                //make sure is pending
                if (!contract.Status.IsPending())
                    return null;

                var editableContract = await UOW.Contracts.GetSingleContract(contractId);
                var bill = Bill.Create(contract);

                billViewModel = new BillingViewModel
                {
                    Id = bill.Id,
                    ContractId = editableContract.Id,
                    BillCode = bill.Code,
                    DateStamp = bill.DateStamp,
                    StatusCode = bill.Status.Code,
                    ContractCode = editableContract.Code,
                    TransactionStatusCode = editableContract.StatusDescription,
                    TransactionStatusDescription = editableContract.StatusDescription,
                    PeriodStart = editableContract.PeriodStart,
                    PeriodEnd = editableContract.PeriodEnd,
                    Name = editableContract.Name,
                    Address = editableContract.Address,
                    VillaNo = editableContract.VillaNo,
                    ElecNo = editableContract.ElecNo,
                    WaterNo = editableContract.WaterNo,
                    QtelNo = editableContract.QTelNo,
                    RentalType = editableContract.RentalType,
                    VillaStatus = editableContract.VillaStatus,
                    RatePerMonth = editableContract.RatePerMonth,
                    Amount = editableContract.Amount
                };
            }
            else
            {
                billViewModel = Mapper.Map<BillingViewModel>(existingBill);
            }

            return billViewModel;
        }
        public async Task<BillingViewModel> GetBillByCode(string billCode)
        {
            //get bill
            var bill = await UOW.Bills.GetBill(billCode);
            return Mapper.Map<BillingViewModel>(bill);
        }

        /// <summary>
        /// insert and update
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="updateStatus"></param>
        /// <returns></returns>
        public async Task<CustomResult> Create(BillingViewModel model, string userId, Func<string, Task> updateStatus)
        {
            var result = new CustomResult();
            try
            {
                //check first wethear the bill is currently exist
                var existingBill = await UOW.Bills.FindQueryAsync(b => b.Id == model.Id, b => b.Payments);
                if (existingBill == null)
                {
                    //get contract and create bill
                    var contract = await UOW.Contracts.FindQueryAsync(model.ContractId);
                    var bill = Bill.Create(contract);
                    bool isSuccess = false;
                    if (model.Payments.Count > 0)
                    {
                        foreach (var payment in model.Payments)
                        {
                            isSuccess = bill.AddPayment(payment.PaymentDate,
                                                payment.PaymentTypeCode,
                                                payment.PaymentModeCode,
                                                payment.ChequeNo,
                                                payment.BankCode,
                                                payment.PeriodStart,
                                                payment.PeriodEnd, payment.Amount, payment.Remarks, userId);
                            if (!isSuccess)
                                break;
                        }
                    }
                    if (isSuccess)
                    {   
                        bill.ActivateBill();
                        UOW.Bills.Add(bill);
                        contract.ActivateStatus();
                        UOW.Contracts.Update(contract);
                        await UOW.Commit();
                        result.Success = true;
                        await updateStatus(contract.VillaId);
                    }
                    else
                    {
                        result.AddError("BillUpdatePaymentException", "Unable to update. There must be a conflict period");
                        result.Success = false;
                    }
                }
                else
                {
                    if (model.Payments.Count > 0)
                    {
                        foreach (var payment in model.Payments)
                        {
                            var isExist = existingBill.Payments.Any(p => p.Id == payment.Id);
                            if (!isExist)
                            {
                                existingBill.AddPayment(payment.PaymentDate,
                                                    payment.PaymentTypeCode,
                                                    payment.PaymentModeCode,
                                                    payment.ChequeNo,
                                                    payment.BankCode,
                                                    payment.PeriodStart,
                                                    payment.PeriodEnd, payment.Amount, payment.Remarks, userId);
                            }
                            if (payment.IsDeleted)
                            {
                                existingBill.RemovePayment(payment.Id);
                            }
                            else if (payment.IsModify && !payment.IsDeleted)
                            {
                                existingBill.UpdatePayment(payment.Id, payment.PaymentDate, payment.PaymentTypeCode,
                                    payment.PaymentModeCode,
                                    payment.ChequeNo,
                                    payment.BankCode,
                                    payment.PeriodStart,
                                    payment.PeriodEnd,
                                    payment.Amount, payment.Remarks, userId);
                            }
                        }

                        UOW.Bills.Update(existingBill);
                        await UOW.Commit();
                        result.Success = true;
                    }
                }
            }
            catch (Exception e)
            {
                result.AddError("BillCreatePaymentException", e.Message);
                result.Success = false;
            }
            return result;
        }
        public async Task<CustomResult> Update(BillingViewModel model, string userId)
        {
            var result = new CustomResult();
            try
            {
                //get the bill
                var bill = await UOW.Bills.FindQueryAsync(b => b.Id == model.Id, b => b.Payments, b => b.Reconciles);
                if (bill != null)
                {
                    if (model.Payments.Count > 0)
                    {
                        //update 
                        foreach (var payment in model.Payments)
                        {
                            var isExist = bill.Payments.Any(p => p.Id == payment.Id);
                            if (payment.IsModify && !payment.IsDeleted)
                            {
                                bill.UpdatePayment(payment.Id, 
                                    payment.PaymentDate,
                                    payment.PaymentTypeCode,
                                    payment.PaymentModeCode,
                                    payment.ChequeNo,
                                    payment.BankCode,
                                    payment.PeriodStart,
                                    payment.PeriodEnd,
                                    payment.Amount, payment.Remarks,payment.StatusCode, userId);
                            }
                        }
                    }

                    if(model.Reconciles.Count > 0)
                    {
                        foreach (var reconcile in model.Reconciles)
                        {
                            bill.AddReconcile(
                                reconcile.ChequeNo,
                                reconcile.PaymentTypeCode,
                                reconcile.ReferenceNo,
                                reconcile.BankCode, 
                                reconcile.DishonouredAmount, 
                                reconcile.Amount, 
                                reconcile.PeriodStart, 
                                reconcile.PeriodEnd, 
                                reconcile.Remarks);
                        }
                    }
                    UOW.Bills.Update(bill);
                    await UOW.Commit();
                    result.Success = true;
                }
            }
            catch (Exception e)
            {
                result.AddError("BillUpdatePaymentException", e.Message);
            }
            return result;
        }
        
        
    }
}
