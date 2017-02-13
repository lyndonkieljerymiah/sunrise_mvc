using Ninject.Web.WebApi;
using Sunrise.Client.Persistence.Manager;
using System.Web.Http;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Sunrise.Client.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Sunrise.Client.App_Start.NinjectWebCommon), "Stop")]

namespace Sunrise.Client.App_Start
{
    using Maintenance.Data.Factory;
    using Maintenance.Data.MasterFile;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using System;
    using System.Web;
    using TenantManagement.Data.Tenants;
    using VillaManagement.Data.Factory;
    using VillaManagement.Data.Villas;


    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //villa setup ver 1.1

            //unit work
            kernel
                .Bind<TransactionManagement.Persistence.Repository.IUnitOfWork>()
                .To<TransactionManagement.Persistence.Repository.UnitOfWork>();
            
            //masterfile
            kernel.Bind<IMasterFileFactory>().To<MasterFileFactory>();
            kernel.Bind<ISelectionDataService>().To<SelectionDataService>();
            
            
            //villa
            kernel.Bind<IVillaDataService>().To<VillaDataService>();
            kernel.Bind<IVillaDataFactory>().To<VillaDataFactory>();

            //tenant
            kernel.Bind<ITenantDataService>().To<TenantDataService>();
            kernel.Bind<ITenantDataFactory>().To<TenantDataFactory>();
            
            kernel.Bind<ContractDataManager>().To<ContractDataManager>();
            kernel.Bind<VillaDataManager>().To<VillaDataManager>();
            kernel.Bind<SelectionDataManager>().To<SelectionDataManager>();
            kernel.Bind<TenantDataManager>().To<TenantDataManager>();
            kernel.Bind<BillDataManager>().To<BillDataManager>();
            kernel.Bind<UserDataManager>().To<UserDataManager>();
        }        
    }
}
