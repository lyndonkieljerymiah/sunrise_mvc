using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Compositor;
using Sunrise.TransactionManagement.Data.Contract;
using Sunrise.TransactionManagement.Data.Factory;
using Sunrise.TransactionManagement.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionManagement.Test.Actual
{

    [TestClass]
    public class ContractViewTest
    {
        private ContractViewCollectionComposite Contracts { get; set; }
        private ContractViewCollectionComposite OfficialContracts { get; set; }
        private ContractDataService ContractData { get; set; }


        [TestInitialize]
        public void Initialize()
        {

            ContractData = new ContractDataService(new AppDbContext(), new ReferenceDbContext());

            Contracts = this.ContractData.GetViewContracts();

        }

        [TestMethod]
        public async Task  Can_Get_6MonthsExpiry()
        {
            DateTime dueDate = DateTime.Today.AddMonths(6);
            
            using(var factory = new ContractDataFactory()) {
                var contracts = factory.Contracts.GetViewContracts();
                var list = await Contracts
                        .GetActiveContracts()
                        .ThoseExpiryIn(dueDate)
                        .ToListAsync();

                //assert that count of due is 
                Assert.IsNotNull(list);
                Assert.AreNotEqual(0, list.Count());
            }
        }
    }
}
