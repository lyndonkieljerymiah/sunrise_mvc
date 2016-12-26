using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Abstract;

namespace Sunrise.Client.Controllers.Api
{

    [RoutePrefix("api/tenant")]
    public class TenantController : ApiController
    {
        private readonly IUnitOfWork _uw;

        public TenantController(IUnitOfWork uw)
        {
            _uw = uw;
        }


       
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public ICollection<TenantRegisterViewModel> List()
        {

            var tenants = new List<TenantRegisterViewModel>()
            {
                new TenantRegisterViewModel
                {
                    Name = "Arnold Mercado",
                    EmailAddress = "arnold.mercado@hotmail.com"
                }
            };

            return tenants;
        }

      

        

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _uw.Dispose();
        }
    }
}
