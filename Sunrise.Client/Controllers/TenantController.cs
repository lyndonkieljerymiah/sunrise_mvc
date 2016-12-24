using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Abstract;

namespace Sunrise.Client.Controllers
{
    [RoutePrefix("Tenant")]
    public class TenantController : Controller
    {
        private readonly IUnitOfWork _uw;


        public TenantController(IUnitOfWork uw)
        {
            _uw = uw;
        }

        // GET: Tenant
        [Route("List")]
        public ActionResult Index()
        {   
            return View();
        }

        [Route("Register/{tv?}")]
        public PartialViewResult Register(string tv = "ttin")
        {
            ViewBag.Type = tv;
            return PartialView("_Register");
        }
        

    }
}