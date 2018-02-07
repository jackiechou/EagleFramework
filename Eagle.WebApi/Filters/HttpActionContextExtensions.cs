using System.Net.Http;
using System.Web.Http.Controllers;

namespace Eagle.WebApi.Filters
{
    internal static class HttpActionContextExtensions
    {
        public static TObject Resolve<TObject>(this HttpActionContext actionContext)
        {
            return (TObject)actionContext.Request.GetDependencyScope().GetService(typeof(TObject));
        }
    }
}