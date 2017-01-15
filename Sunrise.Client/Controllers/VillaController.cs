using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sunrise.Client.Controllers
{
    [RoutePrefix("villa")]
    public class VillaController : Controller
    {
        // GET: Villa
        public ActionResult Index()
        {
            return View();
        }

        [Route("create")]
        public ActionResult Create()
        {
            return View();
        }
    }
}