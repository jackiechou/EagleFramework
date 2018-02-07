using Eagle.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Eagle.Services.SystemManagement
{
    public class ApplicationRoleManagerToken : RoleManager<IdentityRole>
    {
        public ApplicationRoleManagerToken(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManagerToken Create(IdentityFactoryOptions<ApplicationRoleManagerToken> options, IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManagerToken(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));

            return appRoleManager;
        }
    }
}
