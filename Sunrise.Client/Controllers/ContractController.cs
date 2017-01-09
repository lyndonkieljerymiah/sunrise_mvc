using System.Web.Mvc;

namespace Sunrise.Client.Controllers
{
    
    [RoutePrefix("contract")]
    [Authorize]
    public class ContractController : Controller
    {
        public PartialViewResult Index() {
            return PartialView();
        }

        [Route("{villaId?}")]
        public ViewResult Contract(string villaId)
        {
            ViewBag.Id = villaId;
            return View();
        }


       
    }
}