using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sunrise.Client.Controllers
{
    [RoutePrefix("receivable")]
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


    }
}