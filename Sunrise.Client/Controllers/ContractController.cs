using System.Web.Mvc;

namespace Sunrise.Client.Controllers
{
    
    [RoutePrefix("contract")]
    [Authorize]
    public class ContractController : Controller
    {
        [Route("{villaId?}")]
        public ViewResult Contract(string villaId)
        {
            ViewBag.Id = villaId;
            return View();
        }


       
    }
}