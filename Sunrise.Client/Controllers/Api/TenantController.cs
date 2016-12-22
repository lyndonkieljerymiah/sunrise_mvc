﻿using System;
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
        /// Create new Tenant Register Form API
        /// url: /api/tenant/create
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("create")]
        public async Task<IHttpActionResult> Create()
        {
            var strKeys = new string[] { "TenantType" };
            var selections = await _uw.Selections.GetSelections(strKeys);
            var newTenant = new TenantRegisterViewModel(selections);

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

            var tenant = new Tenant();

            return Ok(vm);
        }


        

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _uw.Dispose();
        }
    }
}