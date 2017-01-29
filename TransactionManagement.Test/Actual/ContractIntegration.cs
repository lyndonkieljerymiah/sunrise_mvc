using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Data.Factory;
using System.Threading.Tasks;
using System.Linq;
using Sunrise.TransactionManagement.Model;
using Utilities.Helper;
using Sunrise.TransactionManagement.Data.Config;

namespace TransactionManagement.Test.Actual
{
    [TestClass]
    public class ContractIntegration
    {
        private IContractDataFactory _factory;
        

        [TestInitialize]
        public void TestInitialize()
        {
            _factory = new ContractDataFactory();

            ContractConfig.DefaultMonth = 12;
            ContractConfig.RenewalDefaultMonth = 12;
        }

        [TestMethod]
        public async Task Can_Get_Contracts()
        {
            var contracts = await _factory.Contracts.GetContracts("", 1, 1);
            int count = contracts.PageCount;

            Assert.IsNotNull(contracts);
            Assert.AreNotEqual(0, count);
            Assert.AreNotEqual(0, contracts.Count());
        }

        [TestMethod]
        public async Task Can_Get_ContractList()
        {

            var contracts = await _factory.Contracts.GetContractsForListing("", 1, 100);
            Assert.IsNotNull(contracts);
            Assert.AreNotEqual(0, contracts.Count());
            Assert.AreNotEqual(0, contracts.FirstOrDefault().AmountPayable);
        }

        [TestMethod]
        public async Task Can_Get_ContractByCode()
        {
            var contract = await _factory.Contracts.GetActiveContract("CV00112017");

            Assert.IsNotNull(contract);
            Assert.IsNotNull(contract.Tenant);
            Assert.IsNotNull(contract.Villa);
        }

        [TestMethod]
        public async Task Can_Get_ContractExpiry()
        {
            var contracts = await _factory.Contracts.GetExpiryContracts(12, 1, 5);

            Assert.AreNotEqual(0,contracts.PageCount);
            Assert.AreEqual(0, contracts.FirstOrDefault().AmountBalance);
        }

        [TestMethod]
        public async Task Can_Get_ContractStillValid()
        {
            var contracts = await _factory.Contracts.GetExpiryContracts(6, 1, 5);
            Assert.AreEqual(0, contracts.PageCount);
        }

        [TestMethod]
        public async Task Can_Renew_Contract()
        {
            //assumes
            string contractId = "76aef48d-4420-4d81-8c55-5ca1e1dcdc08";

            //take another
            var oldContract = await _factory.Contracts.GetContractViewById(contractId);
            
            //create new contract
            var calculationRate = new MonthRateCalculation(oldContract.Villa.RatePerMonth);
            var newContract = Transaction.CreateRenew(oldContract.Id,oldContract.Id, ContractConfig.DefaultMonth, oldContract.Villa.RatePerMonth, oldContract.PeriodEnd,calculationRate);

            //entry
            

            Assert.IsNotNull(newContract);

        }


    }
}
