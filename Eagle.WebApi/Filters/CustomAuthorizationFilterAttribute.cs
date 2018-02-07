using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApi.Filters
{
    public class CustomAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        private string function;
        public IFunctionCommandService FunctionCommandService { get; set; }

        public CustomAuthorizationFilterAttribute(string value)
        {
            this.function = value;
        }
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            var claimsIdentity = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if (!claimsIdentity.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }
            var requiredClaims = FunctionCommandService.GetClaimsAuthorization(function);
            if (requiredClaims.Count() < 1)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }

            var claims = claimsIdentity.Claims.Select(x => x.Value);
            var matchClaims = requiredClaims.Where(x => claims.Contains(x.Value));
            if (requiredClaims.Count() != matchClaims.Count())
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }
            
            return Task.FromResult<object>(null);
        }
    }
}