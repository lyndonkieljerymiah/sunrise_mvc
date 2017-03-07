using Sunrise.TransactionManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValueObjects;

namespace TransactionManagement.Test.Setup
{
    public static class ContractSetup
    {
        public static Contract CreateContract(string rentalType, string contractStatus, string villaId, string tenantId, DateTime dateStart, decimal rateAmount)
        {
            var contract = Contract.CreateNewEmpty(villaId, 12, rateAmount);

            contract.RentalType = StatusDictionary.CreateByDefault(rentalType);
            contract.ContractType = StatusDictionary.CreateByDefault(contractStatus);
            contract.VillaId = villaId;
            contract.TenantId = tenantId;
            contract.Period = DateTimeRange.SetRange(dateStart, 12);

            return contract;
        }

        public static IEnumerable<Contract> GenerateDefaultContracts()
        {
            IEnumerable<Contract> contracts = new List<Contract>
            {
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123456",DateTime.Today,1300m),
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123457",DateTime.Today.AddDays(10),1300m),
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123458",DateTime.Today.AddDays(20),1100m),
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123459",DateTime.Today.AddDays(25),1200m),
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123410",DateTime.Today.AddDays(30),1100m),
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123411",DateTime.Today.AddDays(35),1200m),
                CreateContract("rtff","csl","02b86518-b1ed-4947-9e34-81b2d7d7e6f3","123412",DateTime.Today.AddDays(40),1500m),
            };

            return contracts;
        }



    }
}
