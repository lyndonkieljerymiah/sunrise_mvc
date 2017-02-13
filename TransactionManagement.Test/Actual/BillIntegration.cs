using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.TransactionManagement.Persistence.Repository;
using Sunrise.TransactionManagement.Model;
using System.Threading.Tasks;
using Sunrise.TransactionManagement.Model.ValueObject;

namespace TransactionManagement.Test.Actual
{
    [TestClass]
    public class BillIntegration
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Can_Add_Bill()
        {
            using(IUnitOfWork uow = new UnitOfWork())
            {
                var contract = await uow.Contracts.FindQueryAsync("8d53d14b-b036-4dff-9e3f-c66ebbaf8bc8");
                var userId = "32963a95-9dbb-4177-b2e1-78935060e5cc";
                //create bill
                var bill = Bill.Create(contract);

                //add payment
                bill.AddPayment(DateTime.Today, PaymentTypeDictionary.CreateCheque().Code, PaymentModeDictionary.CreatePayment().Code, "1233444", "btdb", DateTime.Today, DateTime.Today.AddYears(1), 4500m,"", userId);
                uow.Bills.Add(bill);
                await uow.Commit();
            }
        }

        [TestMethod]
        public async Task Can_View_Bill()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                var billNo = "57496ca5-ac49-4bb1-a296-a7c105937df0";
                var bill = await uow.Bills.GetBill(billNo);
                
                Assert.IsNotNull(bill);
                Assert.AreNotEqual(0, bill.Payments.Count);
            }
        }
       
    }
}
