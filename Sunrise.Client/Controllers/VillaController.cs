using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sunrise.Client.Controllers
{
    [RoutePrefix("villa")]
    [Authorize]
    public class VillaController : Controller
    {
        // GET: Villa
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("create")]
        public ActionResult Create()
        {
            return View();
        }

        [Route("edit/{id?}")]
        public ActionResult Edit(string id)
        {
            ViewBag.Id = id;
            return View("Create");
        }



    }
}