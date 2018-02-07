using System;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Eagle.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AjaxValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {

        private const string TokenField = "__RequestVerificationToken";
        private static HttpAntiForgeryException CreateValidationException(string message)
        {
            return new HttpAntiForgeryException(message);
        }

        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <exception cref="System.ArgumentNullException">filterContext</exception>
        /// <ignore>true</ignore>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            var request = filterContext.HttpContext.Request;

            if (request.IsAjaxRequest())
            {
                var cookie = request.Cookies.Get(AntiForgeryConfig.CookieName);
                if (!request.Headers.AllKeys.Contains(TokenField))
                {
                    throw CreateValidationException("ValidationFailed");
                }
                var tokenValue = request.Headers.GetValues(TokenField).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(tokenValue))
                {
                    throw CreateValidationException("ValidationFailed");
                }
                AntiForgery.Validate(cookie != null ? cookie.Value : null, tokenValue);
            }
            else
            {
                if (request.HttpMethod == WebRequestMethods.Http.Post)
                {
                    new ValidateAntiForgeryTokenAttribute().OnAuthorization(filterContext);
                }
                else
                {
                    // for forms, only POST is supported.
                    throw CreateValidationException("InvalidMethod");
                }
            }
        }
    }
}