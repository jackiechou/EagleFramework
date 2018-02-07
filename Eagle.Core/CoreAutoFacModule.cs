using Autofac;
using Eagle.Core.Configuration;

namespace Eagle.Core
{
    public class CoreAutoFacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<IConfigurationProvider>(c => new ConfigurationProvider());
        }
    }
}
