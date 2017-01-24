using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.VillaManagement.Abstract;
using Sunrise.VillaManagement.Data.Factory;
using Sunrise.VillaManagement.Data.Villas;
using Sunrise.VillaManagement.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaManagement.Test.Real

{
    [TestClass]
    public class VillaIntegration
    {
        private IVillaDataFactory _factory;

        [TestInitialize]
        public void Initialize()
        {   
            _factory = new VillaDataFactory();
        }

        [TestMethod]
        public async Task Can_Get_Villa_Vacants()
        {
            try
            {
                var villas = await _factory.Villas.GetVillasForDisplay("", Utilities.Enum.VillaStatusEnum.All, 1, 20);
                Assert.IsNotNull(villas);
                Assert.IsNotNull(villas.FirstOrDefault().Gallery);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }


        


    }
}
