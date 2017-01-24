using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Data.Factory;
using System.Threading.Tasks;
using System.Linq;

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
        }

        [TestMethod]
        public async Task Can_Get_Contracts()
        {
            var contracts = await _factory.Contracts.GetContracts("", 1, 100);
            Assert.IsNotNull(contracts);
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


    }
}
