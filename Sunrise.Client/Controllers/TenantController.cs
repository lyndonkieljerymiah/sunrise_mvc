using System.Web.Mvc;

namespace Sunrise.Client.Controllers
{
    [Authorize]
    [RoutePrefix("Tenant")]
    public class TenantController : Controller
    {
        
        // GET: Tenant
        [Route("List")]
        public ActionResult Index()
        {   
            return View();
        }


        [Route("Register")]
        public PartialViewResult Register()
        {   
            return PartialView("_Register");
        }


    }
}