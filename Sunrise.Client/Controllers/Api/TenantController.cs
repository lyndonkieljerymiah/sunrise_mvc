using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sunrise.Client.Domains.ViewModels;

namespace Sunrise.Client.Controllers.Api
{

    [RoutePrefix("api/tenant")]
    public class TenantController : ApiController
    {

        /// <summary>
        /// Create new Tenant Register Form API
        /// url: /api/tenant/create
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("create")]
        public IHttpActionResult Create()
        {
            var newTenant = new TenantRegisterViewModel
            {
                Type = "in",
                Individual = new IndividualViewModel()
                {
                    Gender = "male",
                    Birthday = DateTime.Now
                },
                Company = new CompanyViewModel()
                {
                    ValidityDate = DateTime.Now
                }
            };
            
            return Ok(newTenant);
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
                    Type = "Individual",
                    EmailAddress = "arnold.mercado@hotmail.com"
                }
            };

            return tenants;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public IHttpActionResult Update(TenantRegisterViewModel vm)
        {

            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(vm);
        }
    }
}
