using System;
using System.CodeDom;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunrise.Client.Domains.Enum;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Domains.ViewModels;

namespace AppUnitTest.Model
{
    [TestClass]
    public class TenantTest
    {
        [TestInitialize]
        public void Initialize()
        {
            //mapper configuration
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Villa, VillaViewModel>().ReverseMap();
                cfg.CreateMap<SalesTransaction, SalesRegisterViewModel>().ReverseMap();
                cfg.CreateMap<Individual, IndividualViewModel>().ReverseMap();
                cfg.CreateMap<Company, CompanyViewModel>().ReverseMap();
                cfg.CreateMap<Tenant, TenantRegisterViewModel>()
                    .ForMember(dest => dest.Address1, opts => opts.MapFrom(src => src.Address.Address1))
                    .ForMember(dest => dest.Address2, opts => opts.MapFrom(src => src.Address.Address2))
                    .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Address.City))
                    .ForMember(dest => dest.PostalCode, opts => opts.MapFrom(src => src.Address.PostalCode)).ReverseMap();
            });

        }
        [TestMethod]
        public void Can_Map_Tenant()
        {

            var tenant = Tenant.Create("ttin", "1234", "aasss", "ar@gmail.com", "1234", "1234", "1234", "addfdf", "sddd",
                "sdsd", "12222");
            tenant.AddIndividual(DateTime.Today, GenderEnum.Male,"1222","12131" );

            var vm = Mapper.Map<TenantRegisterViewModel>(tenant);
            Assert.AreEqual("1234 1234",vm.FullAddress);
        }
    }
}
