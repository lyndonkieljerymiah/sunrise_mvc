using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.VillaManagement.Model;
using Sunrise.VillaManagement.Persistence.Repository;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace VillaManagement.Test.Real
{
    [TestClass]
    public class VillaUnitTest
    {
        
        [TestMethod]
        public async Task Can_Add_Villa()
        {
            using(var uow = new UnitOfWork())
            {
                var villa = Villa.Map("V102", "E1000", "W10000", "12335", "rtff", 10, "Fully furnished",4500m);
                uow.Villas.Add(villa);
                await uow.SaveChanges();
            }
        }

       


        [TestMethod]
        public async Task Can_Save_BigData()
        {
            decimal[] ratesPerMonth = { 4500m, 7500m, 6500m };
            using (var uow = new UnitOfWork())
            {
                var r = new Random();
                ICollection<Villa> villas = new List<Villa>();
                for (var i = 46; i < 145; i++)
                {
                    i++;
                    var rNext = r.Next(ratesPerMonth.Length - 1);
                    var rate = ratesPerMonth[rNext];
                    var villa = Villa.Map("V0" + i, "E1000"+i, "W10000"+i, "12335", "rtff", 10, "Fully furnished", rate);
                    villas.Add(villa);
                }
                uow.Villas.AddRange(villas);
                await uow.SaveChanges();
            }

        }



    }
}
