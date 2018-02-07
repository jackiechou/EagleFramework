using System.Web.Mvc;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Services;
using Eagle.Services.SystemManagement;
using Eagle.WebApp.Attributes.Session;
using Eagle.Services.Contents;

namespace Eagle.WebApp.Areas.Admin.Controllers.Common
{
    public class DashboardController : BaseController
    {
        private ILanguageService LanguageService { get; set; }
        private IUserService UserService { get; set; }
        private IMenuService MenuService { get; set; }
        private INewsService NewsService { get; set; }

        public DashboardController(IUserService userService, ILanguageService languageService, IMenuService menuService, INewsService newsService)
            : base(new IBaseService[] { userService, languageService, menuService, newsService })
        {
            UserService = userService;
            LanguageService = languageService;
            MenuService = menuService;
            NewsService = newsService;
        }

        //
        // GET: /Admin/Dashboard/
        [SessionExpiration]
        public ActionResult Index()
        {
            return View("../Common/Home/Index");
        }

        [HttpGet]
        public ActionResult GetInfoBox()
        {
            return PartialView("../Common/Home/_InfoBox");
        }

        [HttpGet]
        public ActionResult GeAdminQuickShoptcuts()
        {
            var lst = MenuService.GetListByPosition(ApplicationId, UserId, (int)MenuPositionAdmin.Middle, (int)MenuStatus.Published);
            return PartialView("../Common/Home/_QuickShoptcuts", lst);
        }

        [HttpGet]
        public ActionResult GeStatistics()
        {
            return PartialView("../Common/Home/_Statistics");
        }

        [HttpGet]
        public ActionResult GetLatestMembers()
        {
            int recordCount;
            var sources = UserService.GetUsers(ApplicationId, out recordCount, "SeqNo DESC", 1, 8);
            return PartialView("../Common/Home/_LatestMembers", sources);
        }

        [HttpGet]
        public ActionResult GetLatestPost()
        {
            int recordCount;
            var sources = NewsService.GetNewsList(null, NewsStatus.Published, out recordCount, "NewsId DESC", 1, 5);
            return PartialView("../Common/Home/_LatestPost", sources);
        }

        public ActionResult GeProfileOnLeft()
        {
            var item = UserService.GetUserProfile(UserId);
            return PartialView("../Shared/_LeftProfile", item);
        }

        public ActionResult GeProfileOnTop()
        {
            var item = UserService.GetUserProfile(UserId);
            return PartialView("../Shared/_TopProfile", item);
        }

        #region REMINDER =====================================================================================
        [SessionExpiration]
        public ActionResult LoadReminder()
        {
            return PartialView("../Shared/_Reminder");
        }
        #endregion ===========================================================================================

        //void SwitchLanguage(string languageCode);
        //public void SwitchLanguage(string languageCode)
        //{
        //    System.Globalization.CultureInfo myCIclone = new System.Globalization.CultureInfo(languageCode);
        //    myCIclone.DateTimeFormat = myCIclone.DateTimeFormat;
        //    HttpContext.Current.Session[SettingKeys.CurrentLanguage] = myCIclone;
        //    HttpContext.Current.Session[SettingKeys.LanguageCode] = languageCode;

        //    //Change MENU due to LanguageCode
        //    //List<SYS_tblFunctionPermissionViewModel> moduleList = context.sp_clsCommon("ModuleList", languageCode, "", 0, 0, 1, acc.UserName).Select(p => new SYS_tblFunctionPermissionViewModel() { Url = p.Url, FunctionID = p.FunctionID, FunctionNameE = p.FunctionNameE, Rank = p.Rank, Parent = p.Parent, Tooltips = p.Tooltips, FView = (p.FView != 0 && p.FView != null), FEdit = (p.FEdit != 0 && p.FEdit != null), FDelete = (p.FDelete != 0 && p.FDelete != null), FExport = (p.FExport != 0 && p.FExport != null) }).ToList();
        //    //moduleList.Add(new SYS_tblFunctionPermissionViewModel() { Url = "/Admin/ChangePassword" });
        //    //HttpContext.Current.Session[SettingKeys.MenuList] = moduleList;

        //    //Change User Info due to LanguageCode
        //    //EmployeeViewModel model = new EmployeeViewModel();
        //    //EmployeeViewModel Emp = (EmployeeViewModel)HttpContext.Current.Session[SettingKeys.EmpInfo];
        //    //if (Emp != null)
        //    //    HttpContext.Current.Session[SettingKeys.EmpInfo] = Emp;
        //    //else
        //    //{
        //    //    EmployeeRepository _repository = new EmployeeRepository(context);
        //    //    var modelEmpId = _repository.GetEmployeeProfile(acc.EmpId, languageCode);
        //    //    if (modelEmpId != null)
        //    //    {
        //    //        modelEmpId = new EmployeeViewModel() { FullName = acc.UserName };
        //    //    }
        //    //    HttpContext.Current.Session[SettingKeys.EmpInfo] = modelEmpId;
        //    //}
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
                    LanguageService = null;
                    UserService = null;
                    MenuService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
