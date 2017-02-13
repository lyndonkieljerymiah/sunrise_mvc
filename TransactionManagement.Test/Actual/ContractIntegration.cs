using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Model;
using Sunrise.TransactionManagement.Model.ValueObject;
using Sunrise.TransactionManagement.Persistence.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using TransactionManagement.Test.ModelMock;
using Utilities.ValueObjects;

namespace TransactionManagement.Test.Actual
{
    [TestClass]
    public class ContractIntegration
    {
        [TestMethod]
        public async Task Can_Add_Contract()
        {
            //tenant and villa test real data
            var tenantId = "123456";
            var userId = "32963a95-9dbb-4177-b2e1-78935060e5cc";
            var villaMock = new VillaMock
            {
                Id = "02b86518-b1ed-4947-9e34-81b2d7d7e6f3",
                Capacity = 10,
                DateStamp = DateTime.Today,
                Description = "Fully Furnished",
                ElecNo = "E123445",
                WaterNo = "W123444",
                QtelNo = "Q123344",
                RatePerMonth = 4500.00m,
                Status = "vsav",
                Type = "rtff",
                VillaNo = "V100"
            };

            using (IUnitOfWork uow = new UnitOfWork())
            {
                //create for display
                var contract = Contract.CreateNewEmpty(villaMock.VillaNo, 12, villaMock.RatePerMonth);

                contract.RentalType = StatusDictionary.CreateByDefault(villaMock.Type);

                //create for saving
                var contractForSaving = Contract.Map(contract.Code, contract.RentalType.Code,
                    contract.ContractStatus.Code, contract.Period.Start, contract.Period.End,
                    contract.Amount.Amount, villaMock.Id, tenantId, userId);

                uow.Contracts.Add(contractForSaving);
                

                await uow.Commit();
            }
        }

        /// <summary>
        /// #1 Check if no null return 
        /// #2 Check if there are contracts 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Can_List_All_Contracts()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                //get all contract from db
                var contracts = await uow.Contracts.GetContracts(TransactionStatusDictionary.GetActive());

                Assert.IsNotNull(contracts);
                Assert.AreNotEqual(0, contracts.Count());
            }
        }

        /// <summary>
        /// #1 Check if no null return 
        /// #2 Check if there are contracts 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Can_Contract_ById()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                //get all contract from db
                var contract = await uow.Contracts.GetSingleContract("6089d080-1e30-45d8-80f6-8cfc4e67f0da");

                Assert.IsNotNull(contract);
                Assert.AreEqual("6089d080-1e30-45d8-80f6-8cfc4e67f0da", contract.Id);
            }
        }



    }
}
