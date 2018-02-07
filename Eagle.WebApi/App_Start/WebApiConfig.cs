using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using Eagle.WebApi.Filters;
using Eagle.WebApi.MediaTypeFormatters;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Eagle.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Allow CORS and make pagination available
            var cors = new EnableCorsAttribute("*", "*", "*", "X-Pagination, X-TAK") {SupportsCredentials = true};
            config.EnableCors(cors);
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            //Validate Model from Filter - [ValidateModel]
            config.Filters.Add(new ValidateModelAttribute());

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Formatters.Add(new BinaryMediaTypeFormatter());
            config.Formatters.Add(new CsvMediaTypeFormatter(new QueryStringMapping("format", "csv", "text/csv")));


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // config.Services.Add(typeof(IExceptionLogger), new AiExceptionLogger());

        }
    }
}
