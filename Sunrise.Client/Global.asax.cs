﻿using AutoMapper;
using Newtonsoft.Json.Serialization;
using Sunrise.Client.Domains.ViewModels;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Sunrise.VillaManagement.Model;
using Sunrise.TenantManagement.Model;
using Sunrise.TransactionManagement.Model;
using Sunrise.TransactionManagement.DTO;
using Sunrise.Client.Infrastructure.Binding;
using Sunrise.VillaManagement.DTO;

namespace Sunrise.Client
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            Mapping();
           
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        private void Mapping()
        {
            //mapper configuration
            Mapper.Initialize(cfg =>
            {

                cfg.CreateMap<Villa, VillaViewModel>().ReverseMap();

                #region TenantView to ViewModel Map
                cfg.CreateMap<IndividualView, IndividualViewModel>().ReverseMap();
                cfg.CreateMap<CompanyView, CompanyViewModel>().ReverseMap();
                cfg.CreateMap<TenantView, TenantRegisterViewModel>()
                    .ForMember(dest => dest.Address1, opts => opts.MapFrom(src => src.Address.Address1))
                    .ForMember(dest => dest.Address2, opts => opts.MapFrom(src => src.Address.Address2))
                    .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Address.City))
                    .ForMember(dest => dest.PostalCode, opts => opts.MapFrom(src => src.Address.PostalCode))
                    .ForMember(dest => dest.Company, opts => opts.Condition(src => (src.Company != null)))
                    .ForMember(dest => dest.Individual, opts => opts.Condition(src => (src.Individual != null)))
                    .ReverseMap();
                #endregion

                #region Tenant to ViewModel Map
                cfg.CreateMap<Individual, IndividualViewModel>().ReverseMap();
                cfg.CreateMap<Company, CompanyViewModel>().ReverseMap();
                cfg.CreateMap<Tenant, TenantRegisterViewModel>()
                    .ForMember(dest => dest.Address1, opts => opts.MapFrom(src => src.Address.Address1))
                    .ForMember(dest => dest.Address2, opts => opts.MapFrom(src => src.Address.Address2))
                    .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Address.City))
                    .ForMember(dest => dest.PostalCode, opts => opts.MapFrom(src => src.Address.PostalCode))
                    .ForMember(dest => dest.Company, opts => opts.Condition(src => (src.Company != null)))
                    .ForMember(dest => dest.Individual, opts => opts.Condition(src => (src.Individual != null)));
                #endregion

                #region VillaView to ViewModel
                cfg.CreateMap<VillaGallery, ImageGalleryViewModel>().ReverseMap();
                cfg.CreateMap<Villa, VillaViewModel>()
                .ForMember(dest => dest.ImageGalleries, opts => opts.MapFrom(src => src.Galleries))
                .ReverseMap();

                cfg.CreateMap<VillaManagement.DTO.VillaListDTO, VillaListViewModel>().ReverseMap();
                #endregion


                #region contract 
                cfg.CreateMap<Payment, PaymentViewModel>();
                cfg.CreateMap<PaymentViewModel, Payment>()
                    .ForMember(dest => dest.Status, opts => opts.Ignore());
                cfg.CreateMap<PaymentView, PaymentViewModel>().ReverseMap();

                cfg.CreateMap<TransactionListDTO, ContractListViewModel>().ReverseMap();
                
                //transaction villa view
                cfg.CreateMap<TransactionManagement.DTO.VillaView, VillaViewModel>().ReverseMap();
                cfg.CreateMap<TransactionView, BillingViewModel>()
                 .ForMember(dest => dest.Villa, opts => opts.MapFrom(src => src.Villa))
                 .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Tenant.Name))
                 .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.Tenant.Address.ToString()))
                 .ForMember(dest => dest.Payments, opts => opts.MapFrom(scr => scr.Payments))
                 .ReverseMap();

                cfg.CreateMap<TransactionView, ReceivableViewModel>()
                 .ForMember(dest => dest.Villa, opts => opts.MapFrom(src => src.Villa))
                 .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Tenant.Name))
                 .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.Tenant.Address.ToString()))
                 .ForMember(dest => dest.Payments, opts => opts.MapFrom(scr => scr.Payments))
                 .ReverseMap();

                cfg.CreateMap<Transaction, TransactionRegisterViewModel>().ReverseMap();


                #endregion
            });

        }
    }
}
