using System.Web.Mvc;
using Eagle.Services;
using Eagle.Services.SystemManagement;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class PermissionController : BaseController
    {
        private ISecurityService SecurityService { get; set; }
        private IUserService UserService { get; set; }
        
        public PermissionController(ISecurityService securityService, IUserService userService, ILanguageService languageService)
            : base(new IBaseService[] { securityService, userService, languageService })
        {
            //SignInManager = signInManager;
            //SignInManagerCookie = signInManagerCookie;
            SecurityService = securityService;
            UserService = userService;
        }

        ////
        //// GET: /Admin/Permission/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[ChildActionOnly]
        //[SessionExpiration]
        //public ActionResult LoadMenuRolePermission()
        //{
        //    string strHTML = _securityService.GenerateDynamicRolePermissionTable(GlobalSettings.ApplicationId, PermissionCodeStatus.SYSTEM_MENU);
        //    return PartialView("../Sys/Permissions/_RolePermission", strHTML);
        //}

        //[ChildActionOnly]
        //[SessionExpiration]
        //public ActionResult LoadModuleRolePermission()
        //{
        //    string strHTML = _securityService.GenerateDynamicRolePermissionTable(GlobalSettings.ApplicationId, PermissionCodeStatus.SYSTEM_MODULE);
        //    return PartialView("../Sys/Permissions/_RolePermission", strHTML);
        //}

        //[ChildActionOnly]
        //[SessionExpiration]
        //public ActionResult LoadPageRolePermission()
        //{
        //    string strHTML = _securityService.GenerateDynamicRolePermissionTable(GlobalSettings.ApplicationId, PermissionCodeStatus.SYSTEM_PAGE);
        //    return PartialView("../Sys/Permissions/_RolePermission", strHTML);
        //}

        //[ChildActionOnly]
        //[SessionExpiration]
        //public ActionResult LoadFunctionRolePermission()
        //{
        //    string strHTML = _securityService.GenerateDynamicRolePermissionTable(GlobalSettings.ApplicationId, PermissionCodeStatus.SYSTEM_FUNCTION);
        //    return PartialView("../Sys/Permissions/_RolePermission", strHTML);
        //}

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    SecurityService = null;
                    UserService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
