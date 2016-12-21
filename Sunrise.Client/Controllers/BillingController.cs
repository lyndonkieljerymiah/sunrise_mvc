using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sunrise.Client.Domains.ViewModels;

namespace Sunrise.Client.Controllers
{
    [RoutePrefix("Billing")]
    public class BillingController : Controller
    {

        [Route("View/{id?}")]
        public ActionResult ViewBill(int id)
        {
            var billing = new BillingViewModel()
            {
                Name = "Arnold Mercado",
                Code = "V12222333"
            
            };
            return View(billing);
        }

        public PartialViewResult PaymentView()
        {
            return PartialView();
        }
    }
}
