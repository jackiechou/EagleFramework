using System;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Controllers
{
    public class MenuDesktopController : Controller
    {
        private IMenuService MenuService { get; set; }
        private IPageService PageService { get; set; }

        public MenuDesktopController(IMenuService menuService, IPageService pageService)
        {
            MenuService = menuService;
            PageService = pageService;
        }       

        public ActionResult GetTopMenu()
        {
            var applicationId = GlobalSettings.DefaultApplicationId;
            string strHtml = MenuService.LoadDesktopBootstrapMenu(applicationId, MenuPositionSetting.TopSite);
            return Json(strHtml, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMainMenu()
        {
            var applicationId = GlobalSettings.DefaultApplicationId;
            string strHtml = MenuService.LoadDesktopBootstrapMenu(applicationId, MenuPositionSetting.MiddleSite);
            return Json(strHtml, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PopulateSiteMapByMenuCode(string menuCode)
        {
            var result = MenuService.PopulateSiteMapByMenuCode(menuCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PopulateSiteMap(string controllerName = "home", string actionName = "index")
        { 
            var result = MenuService.PopulateSiteMap(controllerName, actionName);
            return Json(result, JsonRequestBehavior.AllowGet);
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
                    PageService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
