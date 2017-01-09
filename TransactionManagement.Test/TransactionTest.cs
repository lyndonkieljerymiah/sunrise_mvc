using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Model;
using Utilities.Helper;
using Sunrise.TransactionManagement.Persistence.Repository;
using System.Threading.Tasks;
using System.Linq;

namespace TransactionManagement.Test
{
    [TestClass]
    public class TransactionTest
    {
      
        
        [TestMethod]
        public async Task Can_Get_Transaction_From_Db()
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var transaction = await uow.Transactions.GetTransactionView("f84cb8b1-2c95-48a3-ace7-8364f6a16d2c");
                    Assert.IsNotNull(transaction);
                    Assert.IsNotNull(transaction.Tenant.Company);
                    Assert.IsNotNull(transaction.Tenant.Individual);
                    Assert.IsNotNull(transaction.Villa);
                    Assert.IsNotNull(transaction.Tenant);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [TestMethod]
        public async Task Can_Get_Transaction_By_Code()
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var transaction = await uow.Transactions.GetContractByCode("VS102");
                    var totalBalance = transaction.GetBalanceDue();

                    Assert.IsNotNull(transaction);
                    Assert.AreNotEqual(0, totalBalance);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [TestMethod]
        public async Task Can_Prevent_If_CurrentDateExist()
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var newTransaction = await uow.Transactions.GetContractById("4face5b6-6758-4092-80ec-bbb24865751c");
                    var success = newTransaction.AddPayment(DateTime.Today,"ptcq", "pmp", "123456", "bdb", DateTime.Today, DateTime.Today.AddMonths(6), 7000m, "");
                    Assert.AreEqual(1, newTransaction.Payments.Count);
                    Assert.IsFalse(success);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [TestMethod]
        public async Task Can_Get_Transaction_Thru_Payment()
        {
            try
            {

                using (var uow = new UnitOfWork())
                {
                    var payment = await uow.Payments.GetPaymentById(9);
                    Assert.IsNotNull(payment);
                    Assert.IsNotNull(payment.Transaction);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
