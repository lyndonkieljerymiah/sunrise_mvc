﻿using System;
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
        public async Task<ActionResult> Index()
        {
            var villas = await _unitOfWork.Villas.GetAllVilla();
            var shop = new ShopViewModel
            {
                Villas = VillaViewModel.CreateRange(villas)
            };
            return View(shop);
        }
        
        [Route("checkout/{villaId?}")]
        public async Task<ViewResult> Checkout(int villaId)
        {
            var villa = await _unitOfWork.Villas.FindAsync(villaId);
            var selections = await _unitOfWork.Selections.GetSelections(new string[] {"TenantType", "RentalType", "ContractStatus"});
            
            var vmVilla = VillaViewModel.Create(villa);
            var salesVm = SalesViewModel.Create(vmVilla, selections);

            return View(salesVm);
        }

        
    }
}