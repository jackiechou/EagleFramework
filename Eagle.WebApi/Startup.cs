using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Eagle.Services;
using Eagle.Services.SystemManagement;
using Microsoft.Owin;
using Newtonsoft.Json.Serialization;
using Owin;

//[assembly: OwinStartup(typeof(Eagle.WebApi.Startup))]

namespace Eagle.WebApi
{
    public partial class Startup : Services.ServiceStartup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();

           // ConfigureTokenAuthGeneration(app);
           // ConfigureOAuthTokenConsumption(app);

            ConfigureWebApi(httpConfig);
           // app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(httpConfig);
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
