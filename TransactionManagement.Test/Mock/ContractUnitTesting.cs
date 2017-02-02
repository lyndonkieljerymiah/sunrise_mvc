using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Helper;

namespace TransactionManagement.Test.Mock
{
    [TestClass]
    public class ContractUnitTesting
    {


        [TestMethod]
        public void Cannot_GetRenewal_If_DueBalance()
        {
            //contract
            var contract = Contract.CreateNewEmpty("1234", 12,750m,DateTime.Today, new MonthRateCalculation());
            contract.VillaId = "100";
            contract.TenantId = "T100";
            
            //add payment
            contract.AddPayment(DateTime.Today, "ptcq", "pmp", "23123456", "doha", DateTime.Today, DateTime.Today.AddMonths(12), 1200m, "", "12121212");
            contract.AddPayment(DateTime.Today, "ptcq", "pmp", "23123456", "doha", DateTime.Today, DateTime.Today.AddMonths(12), 1200m, "", "12121212");
            contract.ActivateStatus();

            Assert.AreNotEqual(0, contract.GetBalanceDue());
            Assert.AreEqual(contract.AmountPayable , contract.GetBalanceDue());
        }

        




        
    }
}
