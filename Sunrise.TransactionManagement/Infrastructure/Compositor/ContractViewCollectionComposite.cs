using PagedList;
using PagedList.EntityFramework;
using Sunrise.TransactionManagement.DTO;
using Sunrise.TransactionManagement.Enum;
using Sunrise.TransactionManagement.Model.ValueObject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValueObjects;

namespace Sunrise.TransactionManagement.Compositor
{   

    public class ContractViewCollection 
    {
        private IQueryable<ContractView> _transactionView;

        public ContractViewCollection(IQueryable<ContractView> transactionView)
        {
            _transactionView = transactionView;
        }
        
        public ContractViewCollection GetOfficialContracts()
        {
            var contains = TransactionStatusDictionary.GetOfficials();

            if (_transactionView.All(t => contains.Contains(t.Contract.Status.Code)))
                return this;

            return new ContractViewCollection(_transactionView.Where(c => contains.Contains(c.Contract.Status.Code)));
        }
        public ContractViewCollection GetActiveContracts()
        {
            var contains = new string[] {"sscn"};
            if (_transactionView.All(t => contains.Contains(t.Contract.Status.Code)))
                return this;
            return new ContractViewCollection(_transactionView.Where(c => contains.Contains(c.Contract.Status.Code)));
        }
        public ContractViewCollection ThoseExpiryIn(DateTimeRange dateRange) 
        {
            return new ContractViewCollection(_transactionView.Where(c => c.Contract.Period.End <= dateRange.End));
        }

        public ContractViewCollection FilterByCode(string codeNo)
        {
            return new ContractViewCollection(_transactionView.Where(c => c.Contract.Code.Contains(codeNo)));
        }
        
        public IPagedList<ContractView> ToPage(int pageNumber,int pageSize)
        {
            return _transactionView.ToPagedList(pageNumber, pageSize);
        }
        public async Task<IPagedList<ContractView>> ToPageListAsync(int pageNumber, int pageSize)
        {
            return await _transactionView
                .OrderBy(c => c.Contract.Code)
                .ToPagedListAsync(pageNumber, pageSize);
        }
        
        public List<ContractView> ToList()
        {
            return _transactionView.ToList();
        }
        public async Task<IEnumerable<ContractView>> ToListAsync()
        {
            return await _transactionView.ToListAsync();
        }
        
        //special function
        public async Task<IPagedList<TransactionListDTO>> ToDTOPageListAsync(int pageNumber, int pageSize)
        {
            return await _transactionView
                .Select(t => new TransactionListDTO
                {
                    Id = t.Contract.Id,
                    Code = t.Contract.Code,
                    PeriodStart = t.Contract.Period.Start,
                    PeriodEnd = t.Contract.Period.End,
                    DateCreated = t.Contract.DateCreated,
                    TenantName = t.Tenant.Name,
                    VillaNo = t.Villa.VillaNo,
                    Status = t.Contract.Status.Code,
                    StatusDescription = t.StatusDescription,
                    AmountPayable = t.Contract.Amount.Amount
                })
                .OrderBy(c => c.Code)
                .ToPagedListAsync(pageNumber, pageSize);
        }

       
    }


    

   
}
