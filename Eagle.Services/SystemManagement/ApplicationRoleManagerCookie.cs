using Eagle.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Eagle.Services.SystemManagement
{
    public class ApplicationRoleManagerCookie : RoleManager<IdentityRole>
    {
        public ApplicationRoleManagerCookie(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManagerCookie Create(IdentityFactoryOptions<ApplicationRoleManagerCookie> options, IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManagerCookie(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));

            return appRoleManager;
        }
    }
}
