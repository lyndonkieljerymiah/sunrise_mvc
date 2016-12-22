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

        [Route("Template/Register/{tv?}")]
        public async Task<PartialViewResult> Register(string tv = "ttin")
        {
            var selections = await _uw.Selections.GetSelections(new string[] { "TenantType" });
            var vmTenant = new TenantRegisterViewModel(selections);
            vmTenant.Type = tv;
            return PartialView("_Register",vmTenant);
        }
        

    }
}