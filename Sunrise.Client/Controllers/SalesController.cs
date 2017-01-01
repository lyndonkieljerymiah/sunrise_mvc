using System.Web.Mvc;

namespace Sunrise.Client.Controllers
{
    
    [RoutePrefix("sales")]
    [Authorize]
    public class SalesController : Controller
    {
        // GET: SalesRegister
        //shop
        public ActionResult Index()
        {
            return View();
        }

        [Route("checkout/{villaId?}")]
        public ViewResult Checkout(int villaId)
        {
            ViewBag.Id = villaId;
            return View();
        }

        [Route("billing/{transactionId?}")]
        public ViewResult Billing(string transactionId)
        {
            ViewBag.Id = transactionId;
            return View();
        }
    }
}