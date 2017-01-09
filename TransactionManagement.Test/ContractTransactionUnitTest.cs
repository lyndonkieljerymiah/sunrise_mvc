using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Model;
using Utilities.Helper;

namespace TransactionManagement.Test
{
    [TestClass]
    public class ContractTransactionUnitTest
    {

        [TestMethod]
        public void Initial_Contract_Value()
        {
            //create transaction
            var transaction = Transaction.CreateNew(12, 7000m, new MonthRateCalculation());

            Assert.AreEqual(transaction.PeriodEnd, Convert.ToDateTime("01/03/2018"));
            Assert.AreEqual(transaction.AmountPayable, 84000m);
            Assert.IsNotNull(transaction);
        }

        [TestMethod]
        public void Pay_Cheque_StatusShouldRecevied()
        {
            //initial
            var transaction = Transaction.Map("123", "rtff", "csl", DateTime.Today, DateTime.Today.AddYears(1), 54000m, "a", "b", "123456");
            transaction.AddPayment(DateTime.Today, "ptcq", "bdb", "123456", "bdb", DateTime.Today, DateTime.Today.AddMonths(1), 7500m,"");

            var ps = transaction.Payments;
            foreach (var payment in ps)
            {
                Assert.AreEqual("psv", payment.Status);
            }
        }


        [TestMethod]
        public void Pay_Cash_StatusShouldCleared()
        {
            var transaction = Transaction.Map("123","rtff","csl",DateTime.Today,DateTime.Today.AddYears(1),54000m,"a","b","123456");
            transaction.AddPayment(DateTime.Today, "ptcs", "bdb", "Cash", "", DateTime.Today, DateTime.Today.AddMonths(1), 7500m, "");
            transaction.AddPayment(DateTime.Today,"ptcs", "bdb", "Cash", "", DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(2), 7500m, "");

            var ps = transaction.Payments;
            foreach(var payment in ps)
            {
                Assert.AreEqual("psc", payment.Status);
            }
            
        }

        [TestMethod]
        public void IsPaymentCovered_Exist_ShouldFailed()
        {
            var transaction = Transaction.Map("123", "rtff", "csl", DateTime.Today, DateTime.Today.AddYears(1), 54000m, "a", "b", "123456");
            var is1Success = transaction.AddPayment(DateTime.Today,"ptcq", "bdb", "123456", "", DateTime.Today, DateTime.Today.AddMonths(1), 7500m, "");
            var is2Success = transaction.AddPayment(DateTime.Today,"ptcs", "bdb", "Cash", "", DateTime.Today.AddMonths(1).AddDays(1), DateTime.Today.AddMonths(2), 7500m, "");

            Assert.AreEqual(true, is1Success);
            Assert.AreEqual(false, is2Success);
        }

    }
}
