using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Domains.ViewModels;

namespace Sunrise.Client
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            //mapper configuration
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Villa, VillaViewModel>().ReverseMap();
                cfg.CreateMap<SalesTransaction, SalesViewModel>().ReverseMap();
                cfg.CreateMap<Individual, IndividualViewModel>().ReverseMap();
                cfg.CreateMap<Company, CompanyViewModel>().ReverseMap();
                cfg.CreateMap<Tenant, TenantRegisterViewModel>()
                    .ForMember(dest => dest.Address1, opts => opts.MapFrom(src => src.Address.Address1))
                    .ForMember(dest => dest.Address2, opts => opts.MapFrom(src => src.Address.Address2))
                    .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Address.City))
                    .ForMember(dest => dest.Company, opts => opts.Condition(src => (src.Company != null)))
                    .ForMember(dest => dest.Individual, opts => opts.Condition(src => (src.Individual != null)))
                    .ForMember(dest => dest.PostalCode, opts => opts.MapFrom(src => src.Address.PostalCode))
                    .ReverseMap();
            });

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           
        }
    }
}
