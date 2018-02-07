using System;
using System.Web.Mvc;

namespace Eagle.WebApp.Common
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var request = filterContext.HttpContext.Request;
            string returnedUrl = "/Admin/Account/Unauthorized";
            bool isAuthorized = filterContext.HttpContext.User.Identity.IsAuthenticated; // check authorization

            if (isAuthorized)
            {
                string returnParamPattern = "desiredUrl";
                string requestUrl = request.RawUrl;
                returnedUrl = requestUrl.Contains(returnParamPattern)
                    ? requestUrl.Substring(requestUrl.IndexOf(returnParamPattern, StringComparison.Ordinal) + 10)
                    : requestUrl;

            }
            filterContext.Result = new RedirectResult(returnedUrl);
        }
    }
}