using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.TransactionManagement.Abstract;
using Sunrise.Client.Domains.ViewModels;
using AutoMapper;
using Utilities.Enum;

namespace Sunrise.Client.Persistence.Manager
{
    public class ReceivableDataManager
    {
        private IUnitOfWork _unitOfWork;

        public ReceivableDataManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReceivableViewModel> GetActiveContract(string billNo)
        {

            var transaction = await _unitOfWork.Transactions
                .GetTransactionView(c => c.Code == billNo && c.StatusCode == "sscn");

            var vm = Mapper.Map<ReceivableViewModel>(transaction);
            vm.SetEditMode();
            return vm;
        }

        public async Task<CustomResult> ReverseContract(string id,Action<string> updateStatus)
        {
            var result = new CustomResult();
            try
            {
                var contract = await _unitOfWork.Transactions.FindQueryAsync(c => c.Id == id, c => c.Payments);
                contract.ReversedContract();
                await _unitOfWork.SaveChanges();
                result.Success = true;
                updateStatus(contract.VillaId);
            }
            catch(Exception e)
            {
                result.AddError("InternalErrorException", e.Message);
            }
            return result;
        }

        public async Task<CustomResult> ClearPayment(string transactionId, IEnumerable<PaymentViewModel> values,string userId,Action<string> updateStatus)
        {
            var result = new CustomResult();
            bool isTriggerUpdate = false;
            try
            {
                var contract = await _unitOfWork.Transactions.GetContractById(transactionId);
                if (contract == null)
                    throw new Exception("Invalid Contract");

                foreach (var payment in values)
                {
                    contract.UpdatePaymentStatus(payment.Id, payment.StatusCode, payment.Remarks,userId);
                    if(payment.Status == "psc")
                    {
                        isTriggerUpdate = true;
                    }
                }
                await _unitOfWork.SaveChanges();
                result.Success = true;
                if (isTriggerUpdate)
                    updateStatus(contract.VillaId);
            }
            catch (Exception e)
            {
                result.AddError("InternalErrorException", e.Message);
            }

            return result;
        }

    }
}
