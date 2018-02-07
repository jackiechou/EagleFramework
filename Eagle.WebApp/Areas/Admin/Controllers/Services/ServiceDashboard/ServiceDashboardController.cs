using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services;
using Eagle.Services.SystemManagement;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Services.ServiceDashboard
{
    public class ServiceDashboardController : BaseController
    {
        private IMenuService MenuService { get; set; }

        public ServiceDashboardController(IMenuService menuService) : base(new IBaseService[] { menuService })
        {
            MenuService = menuService;
        }

        // GET: Admin/ServiceDashboard
        public ActionResult Index()
        {
            return View("../Services/ServiceDashboard/Index");
        }

        public ActionResult PopulateServiceMenu()
        {
            int serviceMenuParentId = 123;
            var lst = MenuService.GetMenuListByParentId(serviceMenuParentId, MenuStatus.Published);
            return PartialView("../Services/ServiceDashboard/_ServiceShortCuts", lst);
        }

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    MenuService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}