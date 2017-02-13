using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionManagement.Test.Setup;

namespace TransactionManagement.Test.Mock
{
    [TestClass]
    public class BillUnitTesting
    {   
        /// <summary>
        /// #able to create bill with contract
        /// </summary>
        [TestMethod]
        public void Can_Create_Bill()
        {
            //assuming contract was made
            var contracts = ContractSetup.GenerateDefaultContracts();
            var createdContract = contracts.FirstOrDefault();
            var bill = Bill.Create(createdContract);

            Assert.AreEqual("B-" + createdContract.Code, bill.Code);
        }


       


    }
}
