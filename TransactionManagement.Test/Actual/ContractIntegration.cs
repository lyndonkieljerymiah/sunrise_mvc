using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Data.Factory;
using System.Threading.Tasks;
using System.Linq;
using Sunrise.TransactionManagement.Model;
using Utilities.Helper;
using Sunrise.TransactionManagement.Data.Config;
using System.Net.Http;
using System.Net.Http.Headers;

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
            var contracts = await _factory.Contracts.GetViewContracts()
                                    .ToPageListAsync(1, 100);
            int count = contracts.PageCount;

            Assert.IsNotNull(contracts);
            Assert.AreNotEqual(0, count);
            Assert.AreNotEqual(0, contracts.Count());
        }

        [TestMethod]
        public async Task Can_Get_ContractList()
        {

            var contracts = await _factory.Contracts.GetViewContracts()
                .GetOfficialContracts()
                .ToPageListAsync(1, 100);
            
            Assert.IsNotNull(contracts);
            Assert.AreNotEqual(0, contracts.Count());
            Assert.AreNotEqual(0, contracts.FirstOrDefault().AmountPayable);
        }

        [TestMethod]
        public async Task Can_Get_ContractByCode()
        {
            var contract = await _factory.Contracts.FindContractViewByCode("CV00112017");

            Assert.IsNotNull(contract);
            Assert.IsNotNull(contract.Tenant);
            Assert.IsNotNull(contract.Villa);
        }

        [TestMethod]
        public async Task Can_Get_ContractExpiry()
        {
            var contracts = await _factory.Contracts.GetViewContracts()
                .GetActiveContracts()
                .ThoseExpiryIn(DateTime.Today.AddMonths(12))
                .ToDTOPageListAsync(1, 5);

            Assert.AreNotEqual(0,contracts.PageCount);
            Assert.AreEqual(0, contracts.FirstOrDefault().AmountBalance);
        }
        

        [TestMethod]
        public async Task Can_Renew_Contract()
        {
            //assumption
            string contractId = "76aef48d-4420-4d81-8c55-5ca1e1dcdc08";

            //take another
            var oldContract = await _factory.Contracts.FindContractViewByKey(contractId);
            
            //create new contract
            var calculationRate = new MonthRateCalculation(oldContract.Villa.RatePerMonth);
            var newContract = Contract.CreateRenewEmpty(oldContract.Id,oldContract.Code, ContractConfig.DefaultMonth, oldContract.Villa.RatePerMonth, oldContract.PeriodEnd,calculationRate);

            //entry


            Assert.IsNotNull(newContract);
            Assert.AreEqual(contractId, newContract.Id);

        }

        [TestMethod]
        public async Task Can_Validate_Remaining_Balance()
        {
            string contractId = "67b82869-f522-43cc-aa0c-3f1d7e7ddf6a";
            string contractIdCompleted = "76aef48d-4420-4d81-8c55-5ca1e1dcdc08";
            
            //take another
            var contractWithRemainingBalance = await _factory.Contracts.FindContractViewByKey(contractId);
            var contractFullyPaid = await _factory.Contracts.FindContractViewByKey(contractIdCompleted);

            Assert.AreEqual(true, contractWithRemainingBalance.HasRemainingBalance());
            Assert.AreEqual(false, contractFullyPaid.HasRemainingBalance());
        }
    }
}
