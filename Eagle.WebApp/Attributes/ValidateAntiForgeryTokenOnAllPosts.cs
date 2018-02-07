using System.Net;
using System.Web.Helpers;

namespace System.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class ValidateAntiForgeryTokenOnAllPosts : AuthorizeAttribute
    {
        //public const string HttpHeaderName = "x-RequestVerificationToken";
        public const string HttpHeaderName = "__RequestVerificationToken";
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            //  Only validate POSTs
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                //  Ajax POSTs and normal form posts have to be treated differently when it comes
                //  to validating the AntiForgeryToken
                if (request.IsAjaxRequest())
                {
                    var headerTokenValue = request.Headers[HttpHeaderName];

                    // Ajax POSTs using jquery have a header set that defines the token.
                    // However using unobtrusive ajax the token is still submitted normally in the form.
                    // if the header is present then use it, else fall back to processing the form like normal
                    if (headerTokenValue != null)
                    {
                        var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                        var cookieValue = antiForgeryCookie != null
                            ? antiForgeryCookie.Value
                            : null;


                        AntiForgery.Validate(cookieValue, headerTokenValue);
                    }
                    else
                    {
                        new ValidateAntiForgeryTokenAttribute()
                            .OnAuthorization(filterContext);
                    }
                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute()
                        .OnAuthorization(filterContext);
                }
            }
        }
    }
}