using System.Web.Mvc;

namespace Eagle.WebApp.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        string area = "Admin";

        public override string AreaName => area;

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                    this.area + "_Login",
                    this.area + "/login/{desiredUrl}",
                    new { controller = "Account", action = "Login", desiredUrl = UrlParameter.Optional });

            context.MapRoute(
               area + "_Home",
               area + "/{controller}/{action}/{pageid}/{menuid}",
               new { area = area, controller = "Dashboard", action = "Index", menuid = UrlParameter.Optional, pageid = UrlParameter.Optional });
        }
    }
}
