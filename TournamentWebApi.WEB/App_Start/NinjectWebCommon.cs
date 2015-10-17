using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Web.WebApi;
using TournamentWebApi.DepencyResolver.Modules;
using TournamentWebApi.WEB;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace TournamentWebApi.WEB
{
    public static class NinjectWebCommon
    {
        private static IKernel _kernel;

        public static IKernel Kernel
        {
            get { return _kernel ?? (_kernel = new StandardKernel()); }
        }

        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            try
            {
                Kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                Kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices();

                // To allow Ninject resolves interfaces in constructors for Web Api Controllers
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(Kernel);

                return Kernel;
            }
            catch
            {
                Kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        private static void RegisterServices()
        {
            var modules = new List<INinjectModule>
            {
                new ResolverModule(),
                new DependencyResolver.ResolverModule()
            };

            Kernel.Load(modules);
        }
    }
}
