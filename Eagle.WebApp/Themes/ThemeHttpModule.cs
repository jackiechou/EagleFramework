using System;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Services.Skins;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Themes
{
    public class ThemeHttpModule : IHttpModule
    {
        private IThemeService ThemeService { get; set; }
        private ICacheService CacheService { get; set; }

        public void Init(HttpApplication application, IThemeService themeService, ICacheService cacheService)
        {
            ThemeService = themeService;
            CacheService = cacheService;
            application.BeginRequest += application_BeginRequest;
        }

        private void application_BeginRequest(object sender, EventArgs e)
        {
            var applicationId = GlobalSettings.DefaultApplicationId;
            string selectedTheme = CacheKeySetting.ThemeName;
            string selectedThemeSrc = CacheKeySetting.ThemeSrc;

            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            if (context.Cache == null)
            {
                var theme = ThemeService.GetSelectedTheme(applicationId);
                if (theme != null)
                {
                    CacheService.Add(selectedTheme, theme.PackageName, new CacheItemPolicy
                    {
                        AbsoluteExpiration = System.Web.Caching.Cache.NoAbsoluteExpiration.ToUniversalTime(),
                        Priority = CacheItemPriority.NotRemovable
                    });

                    CacheService.Add(selectedThemeSrc, theme.PackageSrc, new CacheItemPolicy
                    {
                        AbsoluteExpiration = System.Web.Caching.Cache.NoAbsoluteExpiration.ToUniversalTime(),
                        Priority = CacheItemPriority.NotRemovable
                    });
                }
            }

            string themeName = CacheService.Get<string>(CacheKeySetting.ThemeName) ?? GlobalSettings.DefaultThemeName;
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ThemeViewEngine(themeName));
            //context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //context.Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //context.Response.Cache.SetNoStore();
        }


        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            context.LogRequest += new EventHandler(OnLogRequest);
        }

        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            //custom logging logic can go here
        }
    }
}
