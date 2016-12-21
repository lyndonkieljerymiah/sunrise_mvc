using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sunrise.Client.Domains.ViewModels;

namespace Sunrise.Client.Controllers
{
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
        public ActionResult Register()
        {
            var vmTenant = new TenantRegisterViewModel
            {
                Type = "in"
            };
            return View(vmTenant);
        }
        

        [Route("View/{tv?}")]
        public PartialViewResult TenantView(string tv)
        {
            var vmTenant = new TenantRegisterViewModel
            {
                Type = tv
            };

            return PartialView(vmTenant);
        }

        [Route("Sales")]
        public PartialViewResult SalesView()
        {   

            return PartialView(new SalesViewModel());
        }


        [Route("Profile")]
        public ActionResult ProfileView()
        {

            var profile = new TenantProfileViewModel()
            {
                Name = "Arnold Mercado",
                Code = "V12222333",
                Sales = new List<SalesViewModel>
                {
                    new SalesViewModel
                    {
                        VillaNo = "V1000",
                        RentalType = "Fully Furnished",
                        ElectricNo = "E1323333",
                        PeriodStart = Convert.ToDateTime("01/01/2017"),
                        PeriodEnd = Convert.ToDateTime("01/01/2019"),
                        Amount = 450000m
                    }
                }
            };

            return View(profile);
        }
        
    }
}