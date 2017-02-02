using PagedList;
using PagedList.EntityFramework;
using Sunrise.TransactionManagement.DTO;
using Sunrise.TransactionManagement.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.Compositor
{   

    public class ContractViewCollectionComposite 
    {
        private IQueryable<TransactionView> _transactionView;

        public ContractViewCollectionComposite(IQueryable<TransactionView> transactionView)
        {
            _transactionView = transactionView;
        }
        
        public ContractViewCollectionComposite GetOfficialContracts()
        {
            var contains = new string[] { "sscn", "ssp" };
            if (_transactionView.All(t => contains.Contains(t.Status)))
                return this;

            return new ContractViewCollectionComposite(_transactionView.Where(c => contains.Contains(c.StatusCode)));
        }
        public ContractViewCollectionComposite GetActiveContracts()
        {
            var contains = new string[] { "sscn"};
            if (_transactionView.All(t => contains.Contains(t.Status)))
                return this;
            return new ContractViewCollectionComposite(_transactionView.Where(c => contains.Contains(c.StatusCode)));
        }
        public ContractViewCollectionComposite ThoseExpiryIn(DateTime value) 
        {
            return new ContractViewCollectionComposite(_transactionView.Where(c => c.PeriodEnd <= value));
        }
        public ContractViewCollectionComposite FilterByCode(string codeNo)
        {
            return new ContractViewCollectionComposite(_transactionView.Where(c => c.Code.Contains(codeNo)));
        }
        
        public IPagedList<TransactionView> ToPage(int pageNumber,int pageSize)
        {
            return _transactionView.ToPagedList(pageNumber, pageSize);
        }
        public async Task<IPagedList<TransactionView>> ToPageListAsync(int pageNumber, int pageSize)
        {
            return await _transactionView
                .OrderBy(c => c.Code)
                .ToPagedListAsync(pageNumber, pageSize);
        }
        
        public List<TransactionView> ToList()
        {
            return _transactionView.ToList();
        }
        public async Task<IEnumerable<TransactionView>> ToListAsync()
        {
            return await _transactionView.ToListAsync();
        }
        
        //special function
        public async Task<IPagedList<TransactionListDTO>> ToDTOPageListAsync(int pageNumber, int pageSize)
        {
            return await _transactionView
                .Select(t => new TransactionListDTO
                {
                    Id = t.Id,
                    Code = t.Code,
                    PeriodStart = t.PeriodStart,
                    PeriodEnd = t.PeriodEnd,
                    DateCreated = t.DateCreated,
                    TenantName = t.Tenant.Name,
                    VillaNo = t.Villa.VillaNo,
                    Status = t.Status,
                    StatusCode = t.StatusCode,
                    AmountPayable = t.AmountPayable,
                    CreditAmount = t.Payments.Sum(p => (decimal?)p.Amount) ?? 0,
                    AmountBalance = t.Payments
                                    .Where(p => p.StatusCode != PaymentStatusSelection.Clear)
                                    .Sum(p => (decimal?)p.Amount) ?? 0
                })
                .OrderBy(c => c.Code)
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public ContractViewCollectionComposite GetContractsWithStatusOf(ContractStatusEnum status = ContractStatusEnum.All)
        {
            throw new NotImplementedException();
        }
    }


    

   
}
