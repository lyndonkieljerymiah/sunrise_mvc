using AutoMapper;
using PagedList;
using Sunrise.Client.Domains.Configuration;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Infrastructure.Extension;
using Sunrise.TransactionManagement.DTO;
using Sunrise.TransactionManagement.Model;
using Sunrise.TransactionManagement.Model.ValueObject;
using Sunrise.TransactionManagement.Persistence.Repository;
using System;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.Client.Persistence.Manager
{

    public class ContractDataManager
    {

        private IUnitOfWork UOW { get; }
        private ContractConfig Config { get; }
        public ContractDataManager(IUnitOfWork unitOfWork)
        {
            UOW = unitOfWork;
            Config = new ContractConfig();
            Config.DefaultMonth = 12;
        }

        #region contract
        public ContractRegisterCreateViewModel CreateNewContract(string villaNo, DateTime periodStart, decimal ratePerMonth)
        {
            var newContract = Contract.CreateNewEmpty(villaNo, Config.DefaultMonth, ratePerMonth);
            var viewModel = Mapper.Map<ContractRegisterCreateViewModel>(newContract);

            return viewModel;
        }
        public async Task<CustomResult> AddContract(ContractRegisterCreateViewModel vmTransaction, Func<string, Task> callback)
        {
            var result = new CustomResult();
            try
            {
                var transaction = Contract.Map(vmTransaction.Code,
                    vmTransaction.RentalType,
                    vmTransaction.ContractStatus,
                    vmTransaction.PeriodStart,
                    vmTransaction.PeriodEnd, vmTransaction.Amount,
                    vmTransaction.Villa.Id, vmTransaction.Register.Id, vmTransaction.UserId);

                UOW.Contracts.Add(transaction);
                await UOW.Commit();
                result.Success = true;
                result.ReturnObject = (string)transaction.Id;
                if (callback != null) await callback(vmTransaction.Villa.Id);
            }
            catch (Exception e)
            {
                result.AddError("ContractAddException", e.Message);
            }

            return result;
        }
        public async Task<CustomResult> RemoveContract(string id, Func<string, string, Task> callback = null)
        {
            var result = new CustomResult();
            try
            {
                var contract = await UOW.Contracts.FindQueryAsync(id);
                UOW.Contracts.Remove(contract);
                await UOW.Commit();
                result.Success = true;
                if (callback != null) await callback(contract.TenantId, contract.VillaId);
            }
            catch (Exception e)
            {
                result.AddError("ContractRemoveException", e.Message);
            }
            return result;
        }
        public async Task<IPagedList<ContractListViewModel>> GetOfficialContracts(int pageNumber = 0, int pageSize = 20)
        {   
            IPagedList<ContractView>  contracts = (await UOW.Contracts.GetOfficialContracts())
                                        .ToPagedList(pageNumber, pageSize);
            return contracts.ToMappedPagedList<ContractView, ContractListViewModel>();
        }
        public async Task<IPagedList<ContractListViewModel>> GetActiveContracts(int pageNumber, int pageSize)
        {
            var contracts = (await UOW.Contracts.GetActiveContracts()).ToPagedList(pageNumber, pageSize);
            return contracts.ToMappedPagedList<ContractView, ContractListViewModel>();
        }
        public async Task<ContractRegisterEditViewModel> GetContract(string contractId)
        {
            try
            {
                if ((await HasBalance(contractId)))
                    return null;
                                
                //get the old contract 
                var contract = await UOW.Contracts.GetSingleContract(contractId);

                //copy to new
                var newContract = Contract.CreateRenewEmpty(contract.Id, contract.Code, Config.DefaultMonth, contract.RatePerMonth, contract.PeriodEnd);

                var vmNewContract = Mapper.Map<ContractRegisterEditViewModel>(contract);
                vmNewContract.PeriodStart = newContract.Period.Start;
                vmNewContract.PeriodEnd = newContract.Period.End;
                vmNewContract.Amount = newContract.Amount.Amount;

                return vmNewContract;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        public async Task<CustomResult> Renew(ContractRegisterEditViewModel register, string userId, Func<string, Task> callback = null)
        {
            var result = new CustomResult();
            try
            {
                //check if has balance
                if ((await HasBalance(register.Id)))
                {
                    result.AddError("ContractBalanceDue", "Unable to renew contract");
                }

                //take the old one and update
                var oldContract = await UOW.Contracts.FindQueryAsync(register.Id);
                var isSuccess = oldContract.ContractCompletion();

                //make sure the contract is completed before creating new one
                if (isSuccess)
                {
                    //create new one
                    var newContract = Contract.Map(
                        register.Code, register.RentalTypeCode,
                        register.ContractStatusCode, register.PeriodStart, register.PeriodEnd,
                        register.Amount,
                        register.VillaId, register.TenantId, userId);

                    UOW.Contracts.Add(newContract);
                    await UOW.Commit();
                    result.Success = true;

                }
                else
                {
                    result.Success = false;
                    result.AddError("renewContractException", "Payment must be clear to renew.");
                }
            }
            catch(Exception e)
            {
                result.AddError("ContractRenewException", e.Message);
            }
            return result;
        }
        public async Task<ContractTerminateViewModel> GetContractForTermination(string contractId)
        {
            var statusActive = TransactionStatusDictionary.CreateActive();
            var contract = await UOW.Contracts.FindQueryAsync(c => c.Id == contractId && c.Status.Code == statusActive.Code);
            if (contract == null)
                return null;
            return new ContractTerminateViewModel { ContractId = contract.Id, ContractCode = contract.Code };
        }
        public async Task<CustomResult> Terminate(string contractId, string description, string userId, Func<string, Task> callback)
        {
            var result = new CustomResult();
            var contract = await UOW.Contracts.FindQueryAsync(contractId);
            contract.TerminateContract(description, userId);

            UOW.Contracts.Update(contract);
            await UOW.Commit();
            result.Success = true;
            await callback(contract.VillaId);
            return result;
        }
        
        #endregion
        
      


        #region method
        private async Task<bool> HasBalance(string contractId)
        {
            //check if there's any balance
            var bill = await UOW.Bills.FindQueryAsync(b => b.ContractId == contractId,
                b => b.Payments,
                b => b.Reconciles);

            if (bill == null)
                return false;
            else if (bill.GetBalanceDue() > 0)
                return false;



            return true;
        }
        #endregion
    }
}