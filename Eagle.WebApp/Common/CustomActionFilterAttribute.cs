using System;
using System.Web.Mvc;

namespace Eagle.WebApp.Common
{
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            var request = actionContext.HttpContext.Request;
            //var response = actionContext.HttpContext.Response;
            string returnedUrl = "/Admin/Account/Unauthorized";
            bool isAuthorized = actionContext.HttpContext.User.Identity.IsAuthenticated; // check authorization

            if (isAuthorized)
            {
                string returnParamPattern = "desiredUrl";
                string requestUrl = request.RawUrl;
                returnedUrl = requestUrl.Contains(returnParamPattern)
                    ? requestUrl.Substring(requestUrl.IndexOf(returnParamPattern, StringComparison.Ordinal) + 10)
                    : requestUrl;
            }
            
            //response.RedirectLocation = returnedUrl;
            actionContext.Result = new RedirectResult(returnedUrl);
        }
    }
}