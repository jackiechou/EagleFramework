using System.Web.Mvc;
using System.Web.Routing;

namespace Eagle.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //#if DEBUG
            //            routes.IgnoreRoute("{*browserlink}", new { browserlink = @".*/arterySignalR/ping" });
            //#endif

            //// elFinder's connector route       
            routes.MapRoute(null, "connector", new { controller = "File", action = "Index", folder = UrlParameter.Optional, subFolder = UrlParameter.Optional });
            routes.MapRoute(null, "Thumbnails/{tmb}", new { controller = "File", action = "Thumbs", tmb = UrlParameter.Optional });
            //routes.IgnoreRoute("elfinder.connector");

            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            //routes.MapRoute("Login","account/login/{desiredUrl}",new { controller = "Account", action = "Login", desiredUrl = UrlParameter.Optional });
            routes.MapRoute("Home","{controller}/{action}/{pageid}/{menuid}",new { controller = "Home", action = "Index", menuid = UrlParameter.Optional, pageid = UrlParameter.Optional });
        }
    }
}