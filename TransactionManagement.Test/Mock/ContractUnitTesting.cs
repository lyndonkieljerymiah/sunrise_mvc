using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Model;
using Sunrise.TransactionManagement.Model.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Helper;
using Utilities.ValueObjects;

namespace TransactionManagement.Test.Mock
{
    [TestClass]
    public class ContractUnitTesting
    {

        private ICollection<Contract> Contracts { get; set; }
        

        private Contract CreateContract(string rentalType,string contractStatus,string villaId,string tenantId,DateTime dateStart,decimal rateAmount)
        {
            var contract = Contract.CreateNewEmpty(villaId, 12,rateAmount);

            contract.RentalType = StatusDictionary.CreateByDefault(rentalType);
            contract.ContractType = StatusDictionary.CreateByDefault(contractStatus);
            contract.VillaId = villaId;
            contract.TenantId = tenantId;
            contract.Period = DateTimeRange.SetRange(dateStart, 12);

            return contract;
        }

        [TestInitialize]
        public void Initialize()
        {
            Contracts = new List<Contract>
            {
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123456",DateTime.Today,1300m),
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123457",DateTime.Today.AddDays(10),1300m),
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123458",DateTime.Today.AddDays(20),1100m),
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123459",DateTime.Today.AddDays(25),1200m),
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123410",DateTime.Today.AddDays(30),1100m),
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123411",DateTime.Today.AddDays(35),1200m),
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123412",DateTime.Today.AddDays(40),1500m),
            };
        }


        /// <summary>
        /// TODO:   #1 Test if contract is set to pending at first
        ///         #2 Check date period if 1 year
        ///         
        /// </summary>
        [TestMethod]
        public void NewContract_Should_ProperlySet()
        {
            var contract = Contracts.FirstOrDefault();

            var transactionStatus = TransactionStatusDictionary.CreatePending(); //default pending
            DateTimeRange d = DateTimeRange.SetRange(DateTime.Today, 12); //check date if ok
            Payable amount = Payable.Create(1300m, d); //computable amount
            
            Assert.IsNotNull(contract);
            Assert.AreEqual(transactionStatus.Code, contract.Status.Code);
            Assert.AreEqual(true, contract.Period.Equals(d));
            Assert.AreEqual(amount.Amount, contract.Amount.Amount);
        }

        /// <summary>
        /// TODO: 
        ///         #1 Test contract should failed to activate if no bill was entered
        ///         #2 Vice versa automatically activate when bill created
        /// </summary>
        [TestMethod]
        public void Contract_ShouldFailToActivate()
        {
            var contract = Contracts.FirstOrDefault();
            contract.ActivateStatus(); 
            Assert.AreEqual(false, contract.Status.IsActive());
            
        }











    }
}
