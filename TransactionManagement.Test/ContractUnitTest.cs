using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Model;
using Utilities.Helper;

namespace TransactionManagement.Test
{
    [TestClass]
    public class ContractUnitTest
    {

        [TestMethod]
        public void Initial_Contract_Value_ShouldCalculateAccurately()
        {
            var monthToAdd = 12;
            var villaRatePerMonth = 7000m;
            
            //create transaction
            var transaction = Transaction.CreateNew(monthToAdd, villaRatePerMonth, new MonthRateCalculation());
            var expectedOutput = monthToAdd * villaRatePerMonth;

            Assert.AreEqual(transaction.PeriodEnd,DateTime.Today.AddMonths(monthToAdd));
            Assert.AreEqual(transaction.AmountPayable, expectedOutput);
            Assert.IsNotNull(transaction);
        }



        [TestMethod]
        public void Pay_Cheque_StatusShouldRecevied()
        {
            //initial
            var transaction = Transaction.Map("123", "rtff", "csl", DateTime.Today, DateTime.Today.AddYears(1), 54000m, "a", "b", "123456");
            transaction.AddPayment(DateTime.Today, "ptcq", "bdb", "123456", "bdb", DateTime.Today, DateTime.Today.AddMonths(1), 7500m,"","");

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
            transaction.AddPayment(DateTime.Today, "ptcs", "bdb", "Cash", "", DateTime.Today, DateTime.Today.AddMonths(1), 7500m, "","");
            transaction.AddPayment(DateTime.Today,"ptcs", "bdb", "Cash", "", DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(2), 7500m, "","");

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
            var is1Success = transaction.AddPayment(DateTime.Today,"ptcq", "bdb", "123456", "", DateTime.Today, DateTime.Today.AddMonths(1), 7500m, "","");
            var is2Success = transaction.AddPayment(DateTime.Today,"ptcs", "bdb", "Cash", "", DateTime.Today.AddMonths(1).AddDays(1), DateTime.Today.AddMonths(2), 7500m, "","");

            Assert.AreEqual(true, is1Success);
            Assert.AreEqual(false, is2Success);
        }

    }
}
