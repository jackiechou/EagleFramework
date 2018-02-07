using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Eagle.Core.Configuration;
using Eagle.Services;

namespace Eagle.WebApp
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            const string connectionStr = Settings.ConnectionString;

            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //// OPTIONAL: Register model binders that require DI.
            //builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            //builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            //builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();



            // Register our Data dependencies
           

            //builder.RegisterType<BaseService>().AsSelf().PropertiesAutowired();

            builder.RegisterModule(new Log4NetAutofacModule());

            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            //builder.RegisterModule(new DataEntityFrameworkAutoFacModule(connectionStr));

            //builder.RegisterModule(new RepositoryAutoFacModule(connectionStr));

            builder.RegisterModule(new ServicesAutoFacModule(connectionStr));

            //builder.RegisterType<UserController>().InstancePerRequest();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            builder.RegisterType<Startup>()
            .As<Startup>()
            .InstancePerDependency()
            .PropertiesAutowired();
            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}