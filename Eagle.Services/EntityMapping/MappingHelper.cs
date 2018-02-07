using AutoMapper;
using Eagle.EntityFramework.EntityMapping;

namespace Eagle.Services.EntityMapping
{
    public class MappingHelper
    {
        private MappingHelper()
        {
            ConfigureMapping();
        }

        public static void InitializeMapping()
        {
            Instance = new MappingHelper();
        }

        public static MappingHelper Instance { get; set; }
        private static void ConfigureMapping()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                // There should be one mapping configuration class per leaf namespace.
                SystemMapping.ConfigureMapping();
                BusinessMapping.ConfigureMapping();
                ContentMapping.ConfigureMapping();
                ServiceMapping.ConfigureMapping();
                Mapper.AssertConfigurationIsValid();
            });

        }
    }
}
