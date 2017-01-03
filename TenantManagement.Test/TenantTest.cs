using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TenantManagement.Persistence.Repository;
using Sunrise.TenantManagement.Model;
using System.Threading.Tasks;
using System.Linq;
using Utilities.Enum;

namespace TenantManagement.Test
{
    [TestClass]
    public class TenantTest
    {
        [TestMethod]
        public void Tenant_Auto_Type()
        {
            var tenant = Tenant.Map(
                "Arnold Mercado",
                "arnold.mercado@gmail.com",
                "+97466647957",
                "+97466647957",
                "+97466647957",
                "AlWakrah",
                "",
                "Doha City",
                "122222");

            tenant.AddAttributeIndividual(Convert.ToDateTime("08/27/1979"), GenderEnum.Male, "12345", "Nabco");
            Assert.IsNull(tenant.Company);
            Assert.AreEqual(tenant.TenantType, "ttin");
        }

        [TestMethod]
        public async Task Can_Add_Tenant()
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var tenant = Tenant.Map(
                        "Nabco Furniture",
                        "it@nabco.com",
                        "+97466647957",
                        "+97466647957",
                        "+97466647957",
                        "AlWakrah",
                        "",
                        "Doha City",
                        "122222");

                    tenant.AddAttributeCompany("CR155550", "Furniture", Convert.ToDateTime("01/31/2027"), "Rodrigo");
                    uow.Tenants.Add(tenant);
                    await uow.SaveChanges();

                    var tenants = await uow.Tenants.GetQueryAsync();
                    Assert.AreEqual(1, tenants.Count());
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

    }
}
