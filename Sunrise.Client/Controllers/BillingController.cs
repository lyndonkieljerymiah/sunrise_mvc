using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sunrise.Client.Controllers
{
    [RoutePrefix("billing")]
    public class BillingController : Controller
    {


        [Route("{transactionId?}")]
        public ViewResult Billing(string transactionId)
        {
            ViewBag.Id = transactionId;
            return View();
        }

        [Route("")]
        public ViewResult List()
        {
            return View();
        }


     
    }
}