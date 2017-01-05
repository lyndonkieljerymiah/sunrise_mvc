using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Model;
using Sunrise.TransactionManagement.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionManagement.Test
{

    [TestClass]
    public class PaymentTest
    {
        

        [TestMethod]
        public async Task Can_Create_Payment()
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var newTransaction = await uow.Transactions.GetContractById("4face5b6-6758-4092-80ec-bbb24865751c");
                    newTransaction.AddPayment("ptcq", "pmp", "123456", "bdb", DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(1), 7000m, "");
                    await uow.SaveChanges();
                    var transaction = await uow.Transactions.GetContractById("4face5b6-6758-4092-80ec-bbb24865751c");
                    Assert.AreEqual(1, transaction.Payments.Count());
                }
            }
            catch(Exception e)
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
                    var success = newTransaction.AddPayment("ptcq", "pmp", "123456", "bdb", DateTime.Today, DateTime.Today.AddMonths(6), 7000m, "");

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

                using(var uow = new UnitOfWork())
                {
                    var payment = await uow.Payments.GetPaymentById(9);
                    Assert.IsNotNull(payment);
                    Assert.IsNotNull(payment.Transaction);
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
