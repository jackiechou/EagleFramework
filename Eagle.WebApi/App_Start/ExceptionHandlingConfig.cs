using System.Web.Http;
using Eagle.WebApi.Filters;

namespace Eagle.WebApi
{
    /// <summary>
    /// Exception Handling ConfigurationB
    /// </summary>
    public static class ExceptionHandlingConfig
    {
        /// <summary>
        /// Registers the exception handler and loggers.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static void RegisterExceptionHandler(HttpConfiguration configuration)
        {
            //  configuration.Services.Replace(typeof(IExceptionHandler), new ServiceExceptionHandler());
            //    configuration.Services.Add(typeof(IExceptionLogger), new ServiceExceptionLogger(new NullLogger()));
            configuration.Filters.Add(new UnhandledExceptionFilter());
        }
    }
}