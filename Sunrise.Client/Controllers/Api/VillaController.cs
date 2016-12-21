using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Sunrise.Client.Domains.ViewModels;

namespace Sunrise.Client.Controllers.Api
{
    [RoutePrefix("api/villa")]
    public class VillaController : ApiController
    {

        [HttpGet]
        [Route("availability/{villaNo?}")]
        public VillaViewModel Availability(string villaNo)
        {
            VillaViewModel vm = null;

            if (villaNo == "12345")
            {
                vm = new VillaViewModel
                {
                    VillaNo = villaNo,
                    Status = "Available",
                    PicturePath = Url.Content("~/Content/imgs/villa-spalletti-1.jpg"),
                    Capacity = "12 Person",
                    Description = "This 6 bedroom villa was recently refurbished, creating the perfect holiday villa rental in the heart of Jomtien. There are 6 beautifully appointed bedrooms in the villa, sleeping 12 people in total, set over upper and lower floors."
                };
            }


            return vm;
        }
    }
}
