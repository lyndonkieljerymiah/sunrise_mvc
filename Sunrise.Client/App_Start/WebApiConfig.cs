using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Infrastructure.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Sunrise.Client
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var provider = new VillaModelBinderProvider(typeof(VillaViewModel), new VillaBinder());
            config.Services.Insert(typeof(ModelBinderProvider), 0, provider);  
        }
    }
}
