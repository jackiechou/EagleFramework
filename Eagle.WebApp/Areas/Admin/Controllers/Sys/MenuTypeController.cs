using System.Web.Mvc;
using Eagle.Services;
using Eagle.Services.SystemManagement;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class MenuTypeController : BaseController
    {
        private IMenuService MenuService { get; set; }
        
        public MenuTypeController(IMenuService menuService) : base(new IBaseService[] { menuService })
        {
            MenuService = menuService;
        }
        //
        // GET: /Admin/MenuType/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[SessionExpiration]
        //public ActionResult Create()
        //{
        //    MenuTypeDetail model = new MenuTypeDetail();
        //    return PartialView("../Sys/MenuTypes/_Create", model);
        //}

        //[SessionExpiration]
        //public List<MenuTypeDetail> PopulateMenuTypeList()
        //{
        //    return MenuService.GetActiveList(GlobalSettings.ScopeTypeId, GlobalSettings.LanguageCode);
        //}


        #region Dispose

        private bool _disposed;

        [System.Web.Mvc.NonAction]
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
