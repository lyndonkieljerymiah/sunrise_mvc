using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sunrise.Client.Controllers
{
    [RoutePrefix("receivable")]
    [Authorize]
    public class ReceivableController : Controller
    {

        // GET: Receivable
        public ActionResult Index()
        {   
            return View();
        }

        //Get: List
        public ActionResult List()
        {
            return View();
        }


        public PartialViewResult Payment()
        {
            return PartialView();
        }

        public PartialViewResult Reconcile()
        {
            return PartialView();
        }

    }
}