using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sunrise.Client.Domains.ViewModels;

namespace Sunrise.Client.Controllers.Api
{
    [RoutePrefix("api/bill")]
    public class BillingController : ApiController
    {

        [Route("sales/{id?}")]
        public ICollection<SalesViewModel> GetSalesSummary(int id)
        {
            var sales = new List<SalesViewModel>
            {
                new SalesViewModel
                {
                    VillaNo = "V1000",
                    PeriodStart = Convert.ToDateTime("01/01/2017"),
                    PeriodEnd = Convert.ToDateTime("01/01/2019"),
                    Amount = 450000m
                }
            };

            return sales;
        }
    }
}
