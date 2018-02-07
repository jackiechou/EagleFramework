using System.Linq;
using System.Security.Claims;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApi.Filters
{
    public class CustomAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        private string function;
        public IFunctionCommandService FunctionCommandService { get; set; }

        public CustomAuthorizeAttribute(string value)
        {
            this.function = value;
        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User as ClaimsPrincipal;

            if (!user.Identity.IsAuthenticated)
            {
                 base.HandleUnauthorizedRequest(filterContext);
              
            }
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
                base.HandleUnauthorizedRequest(filterContext);

            var requiredClaims = FunctionCommandService.GetClaimsAuthorization(function);
            if (requiredClaims.Count() < 1)
                base.HandleUnauthorizedRequest(filterContext);

            var claims = claimsIdentity.Claims.Select(x => x.Value);
            var matchClaims = requiredClaims.Where(x => claims.Contains(x.Value));
            if (requiredClaims.Count() != matchClaims.Count())
                base.HandleUnauthorizedRequest(filterContext);

            base.OnAuthorization(filterContext);
        }
    }
}