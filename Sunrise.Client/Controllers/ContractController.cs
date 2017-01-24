using System.Web.Mvc;

namespace Sunrise.Client.Controllers
{
    
    [RoutePrefix("contract")]
    [Authorize]
    public class ContractController : Controller
    {
               
        public ViewResult Index() {
            return View();
        }

        [Route("{villaId?}")]
        public ViewResult Contract(string villaId)
        {
            ViewBag.Id = villaId;
            return View();
        }

        [Route("search")]
        public PartialViewResult Search()
        {
            return PartialView();
        }

        


    }
}