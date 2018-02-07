using System.Web.Http.Filters;

namespace Eagle.WebApi.Filters
{
    /// <summary>
    /// Service Authorization Attribute
    /// CORS doesn't want to place nice with angular at the moment. Need to decorate controllers with this attribute.
    /// Once we have found a solution, we can stop using this attribute.
    /// </summary>
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
            {
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, X-Authorization");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, HEAD, OPTIONS");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}