using System;
using System.Web.Http;
using System.Web.Http.Tracing;
using Eagle.Core.Logging;
using log4net;

namespace Eagle.WebApi
{
    /// <summary>
    /// Trace Config used to wire up a trace logger
    /// </summary>
    public static class TraceConfig
    {
        /// <summary>
        /// Wires up a trace logger
        /// </summary>
        /// <param name="configuration">HttpConfiguration</param>
        public static void Register(HttpConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            log4net.Config.XmlConfigurator.Configure();
            configuration.Services.Replace(typeof(ITraceWriter), LogManager.GetLogger(typeof(Logger)));
        }
    }
}