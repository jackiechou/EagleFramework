using System.Web.Mvc;
using Eagle.Core.Configuration;

namespace Eagle.WebApp.Themes
{
    public abstract class ThemeControllerBase : Controller
    {
        protected override void Execute(System.Web.Routing.RequestContext requestContext)
        {
            // Allow the Theme to be overriden via the querystring
            // If a Theme Name is Passed in the querystring then use it and override the previously set Theme Name
            // http://localhost/Default.aspx?theme=Red
            var previewTheme = requestContext.HttpContext.Request.QueryString[CacheKeySetting.ThemeName];
            if (!string.IsNullOrEmpty(previewTheme))
            {
                requestContext.HttpContext.Items[CacheKeySetting.ThemeName] = previewTheme;
            }
            
            base.Execute(requestContext);
        }
    }
}