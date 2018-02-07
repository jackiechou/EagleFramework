using System.Web.Mvc;
using Eagle.Core.Attributes;
using Eagle.WebApp.Common;
using Thinktecture.IdentityModel.Authorization.Mvc;

namespace Eagle.WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // filters.Add(new AiHandleErrorAttribute());
            //filters.Add(new LocalizationAttribute(GlobalSettings.DefaultLanguageCode), 0);
            //filters.Add(new RequireHttpsAttribute());
            //filters.Add(new AuthorizeAttribute());

            // filters.Add(new CustomAuthorize());
            //filters.Add(new ClaimsAuthorizeAttribute());
            //filters.Add(new HandleExceptionAttribute());
        }
    }
}
