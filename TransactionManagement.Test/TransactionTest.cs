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
        public void Can_Start_Contract()
        {   

            //create transaction
            var transaction = Transaction.CreateNew(12,7000m,new MonthRateCalculation());

            Assert.AreEqual(transaction.PeriodEnd, Convert.ToDateTime("01/03/2018"));
            Assert.AreEqual(transaction.AmountPayable, 84000m);
            Assert.IsNotNull(transaction);


        }

        [TestMethod]
        public async Task Can_Save_Transaction_To_Db()
        {
            using (var uow = new UnitOfWork())
            {

                var transactionToSave = Transaction.Map("V102","rtff", "csl",
                    Convert.ToDateTime("01/03/2017"), Convert.ToDateTime("01/03/2018"),
                    84000m,
                    "32eb3206-1e4d-44f2-a414-4fa185022867",
                    "4e594630-683f-4b05-989e-ff47f13e5dda",
                    "4e594630-683f-4b05-989e-ff47f13e5dda");

                uow.Transactions.Add(transactionToSave);
                await uow.SaveChanges();

                var transactions = await uow.Transactions.GetQueryAsync();

                Assert.IsNotNull(transactions);
                Assert.AreEqual(transactions.Count(),1);
            }
        }

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


    }
}
