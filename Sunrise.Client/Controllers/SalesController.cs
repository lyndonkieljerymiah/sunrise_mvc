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
    [RoutePrefix("Sales")]
    public class SalesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public SalesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Sales
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