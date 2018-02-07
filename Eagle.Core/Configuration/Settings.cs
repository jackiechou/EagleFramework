using System.Configuration;
using Eagle.Common.Extensions;
using Eagle.Core.Logging;

namespace Eagle.Core.Configuration
{

    /// <summary>
    /// Sherpa configuration settings
    /// </summary>
    public static class Settings 
    {
        public const string ConnectionString = "DefaultConnection";

        public static ConfigurationProvider ConfigurationProvider;

        static Settings()
        {
            ConfigurationProvider = new ConfigurationProvider();
        }

        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static void GetDefaultConfiguration()
        {
            DefaultConnectionString = Get(ConnectionString);
        }

        public static T Get<T>(string key)
        {
            return Get(key).ToType<T>();
        }

        public static string DefaultConnectionString = Get(ConnectionString);

        // public static int RedisCacheExpireMins = int.Parse(Get("RedisCacheExpireMins"));
        //public static string LoginPageUrl = Get("LoginPageUrl");
        // public static int RedisCacheExpireMins = int.Parse(Get("RedisCacheExpireMins"));
    }
}
