using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.TransactionManagement.Abstract;
using Sunrise.Client.Domains.ViewModels;
using AutoMapper;
using Utilities.Enum;
using Sunrise.TransactionManagement.Data.Factory;

namespace Sunrise.Client.Persistence.Manager
{
    public class ReceivableDataManager
    {

        private IContractDataFactory Factory { get; set; }

        public ReceivableDataManager(IContractDataFactory factory)
        {
            Factory = factory;
        }

        public async Task<ReceivableViewModel> GetActiveContract(string contractCode)
        {
            var transaction = await Factory.Contracts.GetActiveContract(contractCode);
            var vm = Mapper.Map<ReceivableViewModel>(transaction);
            vm.SetEditMode();
            return vm;
        }

        public async Task<CustomResult> ReverseContract(string id, Func<string,Task> callback)
        {
            var contract = await Factory.Contracts.GetContractById(id);
            contract.ReversedContract();

            var result = await Factory.Contracts.UpdateContract(contract);
            if (result.Success)
            {
                if (callback != null) await callback(contract.VillaId);
            }
            return result;
        }

        public async Task<CustomResult> ClearPayment(string transactionId, IEnumerable<PaymentViewModel> values, string userId, Func<string,Task> callback)
        {
            var result = new CustomResult();
            bool isTriggerUpdate = false;
            try
            {
                var contract = await Factory.Contracts.GetContractById(transactionId);
                if (contract == null)
                    throw new Exception("Invalid Contract");

                foreach (var payment in values)
                {
                    contract.UpdatePaymentStatus(payment.Id, payment.StatusCode, payment.Remarks, userId);
                    if (payment.StatusCode == "psc")
                    {
                        isTriggerUpdate = true;
                    }
                }

                result = await Factory.Contracts.UpdateContract(contract);
                if (isTriggerUpdate)
                    if(callback != null) await callback(contract.VillaId);
            }
            catch (Exception e)
            {
                result.AddError("InternalErrorException", e.Message);
            }

            return result;
        }

    }
}
