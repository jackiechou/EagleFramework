using System;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;

namespace Eagle.WebApp.Attributes
{
    public class LocalizedControllerActivator : IControllerActivator
    {
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            //Get the {language} parameter in the RouteData
            string defaultLanguage = GlobalSettings.DefaultLanguageCode;
            if (requestContext.RouteData.Values["lang"]!=null && requestContext.RouteData.Values["lang"].ToString() !=string.Empty && requestContext.RouteData.Values["lang"].ToString() != defaultLanguage)
            {
                defaultLanguage = requestContext.RouteData.Values["lang"].ToString();
            }

            CultureInfo cultureInfo = new CultureInfo(defaultLanguage);
            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "MM/dd/yyyy",
                LongTimePattern = "MM/dd/yyyy HH:mm:ss"
            };
        
            var numberFormat = new NumberFormatInfo()
            {
                CurrencyGroupSeparator = ".",
                CurrencySymbol = defaultLanguage == LanguageType.English ? "USD" : "VND"
                //NumberGroupSeparator
                //NumberDecimalSeparator
            };

            cultureInfo.DateTimeFormat = dateformat;
            cultureInfo.NumberFormat = numberFormat;

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(defaultLanguage);

            return DependencyResolver.Current.GetService(controllerType) as IController;
        }
    }
}