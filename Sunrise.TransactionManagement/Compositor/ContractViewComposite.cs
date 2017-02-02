using Sunrise.TransactionManagement.DTO;
using Sunrise.TransactionManagement.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.Compositor
{
    public class ContractViewComposite
    {
        private TransactionView _transactionView;

        public ContractViewComposite(TransactionView transactionView)
        {
            _transactionView = transactionView;
        }

        public bool HasRemainingBalance()
        {
            var balancePayment = _transactionView.Payments.Where(p => p.StatusCode == PaymentStatusSelection.Clear).Sum(p => p.Amount);
            if (_transactionView.AmountPayable > balancePayment)
                return false;

            return true;
        }


    }
}
