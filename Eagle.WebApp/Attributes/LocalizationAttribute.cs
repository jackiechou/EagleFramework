using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Configuration;

namespace Eagle.WebApp.Attributes
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        private readonly string _defaultLanguage;

        public LocalizationAttribute(string defaultLanguage)
        {
            _defaultLanguage = defaultLanguage;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!string.IsNullOrWhiteSpace(filterContext.RouteData.Values["lang"]?.ToString()))
            {
                // set the culture from the route data (url)
                var lang = filterContext.RouteData.Values["lang"].ToString();
                CultureInfo cultureInfo = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

                // save the location into cookie
                var cookieLanguage = new HttpCookie("Eagle.CurrentUICulture", Thread.CurrentThread.CurrentUICulture.Name)
                {
                    Expires = DateTime.UtcNow.AddYears(1)
                };
                filterContext.HttpContext.Response.SetCookie(cookieLanguage);
            }
            else
            {
                // load the culture info from the cookie
                var cookie = filterContext.HttpContext.Request.Cookies["Eagle.CurrentUICulture"];
                string langHeader;
                if (cookie != null)
                {
                    // set the culture by the cookie content
                    langHeader = cookie.Value;
                    CultureInfo cultureInfo = new CultureInfo(langHeader);
                    Thread.CurrentThread.CurrentUICulture = cultureInfo;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                }
                else
                {
                    // set the culture by the location if not speicified
                    if (filterContext.HttpContext.Request.UserLanguages != null)
                    {
                        langHeader = filterContext.HttpContext.Request.UserLanguages[0];
                        CultureInfo cultureInfo = new CultureInfo(langHeader);
                        Thread.CurrentThread.CurrentUICulture = cultureInfo;
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                    }
                    else
                    {
                        langHeader = _defaultLanguage ?? GlobalSettings.DefaultLanguageCode;
                        CultureInfo cultureInfo = new CultureInfo(langHeader);
                        Thread.CurrentThread.CurrentUICulture = cultureInfo;
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                    }

                    // save the location into cookie
                    var cookieLanguage = new HttpCookie("Eagle.CurrentUICulture", Thread.CurrentThread.CurrentUICulture.Name)
                    {
                        Expires = DateTime.UtcNow.AddYears(1)
                    };
                    filterContext.HttpContext.Response.SetCookie(cookieLanguage);
                }
                // set the lang value into route data
                filterContext.RouteData.Values["lang"] = langHeader;
            }

            base.OnActionExecuting(filterContext);
           
        }
    }
}