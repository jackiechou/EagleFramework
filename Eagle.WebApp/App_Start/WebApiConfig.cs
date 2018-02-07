using System.Web.Http;
using System.Web.Http.Cors;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Eagle.WebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //// Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors();
            config.EnableCors(cors);

            // Web API routes
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // add global authorization filter
            config.Filters.Add(new ClaimsAuthorizeAttribute());
        }
    }
}
