using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Eagle.Core.Configuration;
using Eagle.Core.Logging;
using Eagle.Services;
using IContainer = Autofac.IContainer;

namespace Eagle.WebApi
{
    /// <summary>
    /// AutofacWebApi Configuration
    /// </summary>
    public class AutofacWebApi : Autofac.Module
    {
        const string ConnectionString = Settings.ConnectionString;
        /// <summary>
        /// Registers the resolver.
        /// </summary>
        public static void RegisterResolver(ContainerBuilder builder = null)
        {
            //Register WebApi
            var resolver = new AutofacWebApiDependencyResolver(RegisterServices(new ContainerBuilder()));

            // Configure Web API with the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

        }

        /// <summary>
        /// Registers the services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //In order to register api, we have to Install-Package Autofac.WebApi
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterTypes().PropertiesAutowired();

            //// Register UnitOfWork Implementation
            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();
            //builder.RegisterModule(new RepositoryAutoFacModule(ConnectionString));
            builder.RegisterModule(new ServicesAutoFacModule(ConnectionString));


            var container = builder.Build();

            return container;
        }
    }
}