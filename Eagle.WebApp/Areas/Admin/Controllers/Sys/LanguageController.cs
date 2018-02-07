using System.Globalization;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class LanguageController : BaseController
    {
        private ILanguageService LanguageService { get; set; }

        public LanguageController(ILanguageService languageService) : base(new IBaseService[] { languageService })
        {
            LanguageService = languageService;
        }

        #region GET METHODS =========================================================================
        //
        // GET: /Admin/Language/
        public ActionResult Index()
        {
            return View("../Sys/Language/Index");
        }

        // GET: /Admin/Language/Search
        [HttpGet]
        public ActionResult Edit()
        {
            ViewBag.AvailableLanguages = LanguageService.PopulateAvailableLanguageMultiSelectList();
            ViewBag.SelectedLanguages = LanguageService.PopulateSelectedLanguageMultiSelectList(ApplicationId, ApplicationLanguageStatus.Active);
            return PartialView("../Sys/Language/_Edit");
        }

        // GET: /Admin/Language/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Sys/Language/_SearchForm");
        }

        // GET: /Admin/Language/Search
        [HttpGet]
        public ActionResult Search(LanguageSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["LanguageSearchRequest"] = filter;
            }
            else
            {
                if (TempData["LanguageSearchRequest"] != null)
                {
                    filter = (LanguageSearchEntry)TempData["LanguageSearchRequest"];
                }
            }
            TempData.Keep();
            int? recordCount = 0;
            var sources = LanguageService.GetApplicationLanguages(ApplicationId, filter, ref recordCount, null, page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Sys/Language/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/Language/Create
        [HttpPost]
        public ActionResult CreateApplicationLanguage(ApplicationLanguageEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    LanguageService.InsertApplicationLanguage(ApplicationId, entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.CreateSuccess }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var modeStateErrors = ModelState.GetModelStateErrors();
                    return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        // PUT: /Admin/Language/EditApplicationLanguages
        [HttpPut]
        public ActionResult EditApplicationLanguages(ApplicationLanguageListEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    LanguageService.EditApplicationLanguages(ApplicationId, entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateSuccess }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var modeStateErrors = ModelState.GetModelStateErrors();
                    return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }


        //
        // PUT: /Admin/Language/UpdateSelectedLanguage/5
        [HttpPut]
        public ActionResult UpdateSelectedApplicationLanguage(string languageCode)
        {
            try
            {
                LanguageService.UpdateSelectedApplicationLanguage(ApplicationId, languageCode);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // PUT: /Admin/Language/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateApplicationLanguageStatus(string languageCode, bool status)
        {
            try
            {
                LanguageService.UpdateApplicationLanguageStatus(ApplicationId, languageCode, status ? ApplicationLanguageStatus.Active : ApplicationLanguageStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion ==============================================================================

        //[AcceptVerbs(HttpVerbs.Get)]
        // public JsonResult GetLanguageList()
        // {
        //     LanguageRepository _repository = new LanguageRepository();
        //     List<LanguageViewModel> lst = _repository.GetList();
        //     return Json(lst, JsonRequestBehavior.AllowGet);
        // }

        //public ActionResult PopulateListBox()
        //{
        //    SelectList lst = LanguageRepository.PopulateActiveLanguages(null,false);
        //    return View("../Sys/Pages/_List", lst);
        //}



        //public void SwitchLanguage(int LanguageId, string CurrentLanguage, string DesiredUrl)
        //{

        //    //CultureInfo VNCIclone = new CultureInfo("vi-VN");

        //    CultureInfo myCIclone = new CultureInfo(CurrentLanguage);
        //    myCIclone.DateTimeFormat = myCIclone.DateTimeFormat; 

        //    Session["CurrentLanguage"] = myCIclone;

        //    Session["LanguageId"] = LanguageId;
        //    try
        //    {
        //        AccountViewModel acc = (AccountViewModel)Session[SettingKeys.AccountInfo];
        //        List<SYS_tblFunctionPermissionViewModel> moduleList = db.sp_clsCommon("ModuleList", CurrentLanguage, "", 0, 0, 1, acc.UserName).Select(p => new SYS_tblFunctionPermissionViewModel() { Url = p.Url, FunctionID = p.FunctionID, FunctionNameE = p.FunctionNameE, Rank = p.Rank, Parent = p.Parent, Tooltips = p.Tooltips, FView = (p.FView != 0 && p.FView != null), FEdit = (p.FEdit != 0 && p.FEdit != null), FDelete = (p.FDelete != 0 && p.FDelete != null), FExport = (p.FExport != 0 && p.FExport != null) }).ToList();
        //        moduleList.Add(new SYS_tblFunctionPermissionViewModel() { Url = "/Admin/ChangePassword" });
        //        EmployeeRepository _repository = new EmployeeRepository(db);

        //        EmployeeViewModel Emp = (EmployeeViewModel)Session[SettingKeys.EmpInfo];
        //        EmployeeViewModel model = new EmployeeViewModel();
        //        if (Emp != null)
        //        {
        //            model= _repository.GetEmployee(Emp.EmpID, LanguageId);
        //            if (model == null)
        //            {
        //                model = new EmployeeViewModel() { FullName = acc.UserName };
        //            }
        //            Session[SettingKeys.EmpInfo] = model;
        //        }

        //        EmployeeViewModel EmpId = (EmployeeViewModel)Session[SettingKeys.EmpInfo];
        //        if (EmpId != null)
        //        {
        //            if (Emp != null && Emp.EmpID == EmpId.EmpID)
        //            {
        //                Session[SettingKeys.EmpInfo] = model;
        //            }
        //            else
        //            {
        //                 var modelEmpId = _repository.GetEmployee(EmpId.EmpID, LanguageId);
        //                 if (modelEmpId != null)
        //                 {
        //                     modelEmpId = new EmployeeViewModel() { FullName = acc.UserName };
        //                 }
        //                 Session[SettingKeys.EmpInfo] = modelEmpId;
        //            }
        //        }
        //        Session[SettingKeys.MenuList] = moduleList;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //    Response.Redirect(DesiredUrl);
        //}

        public void SwitchLang(int languageId, string currentLanguage, string desiredUrl)
       {
           CultureInfo myCIclone = new CultureInfo(currentLanguage);
           myCIclone.DateTimeFormat = myCIclone.DateTimeFormat;

           Session["CurrentLanguage"] = myCIclone;
           Session["LanguageId"] = languageId;
           Response.Redirect(desiredUrl, true);
           Response.End();
        }

        //public void SwitchLanguage(int SelectedLanguageId, string SelectedLanguageCode, string DesiredUrl)
        //{
        //    CultureInfo myCIclone = new CultureInfo(SelectedLanguageCode);
        //    myCIclone.DateTimeFormat = myCIclone.DateTimeFormat;

        //    Session[SettingKeys.CurrentLanguage] = myCIclone;
        //    Session[SettingKeys.LanguageCode] = SelectedLanguageCode;
        //    Session[SettingKeys.LanguageId] = SelectedLanguageId;
        //    try
        //    {
        //        AccountViewModel acc = (AccountViewModel)Session[SettingKeys.AccountInfo];
        //        List<SYS_tblFunctionPermissionViewModel> moduleList = db.sp_clsCommon("ModuleList", SelectedLanguageCode, "", 0, 0, 1, acc.UserName).Select(p => new SYS_tblFunctionPermissionViewModel() { Url = p.Url, FunctionID = p.FunctionID, FunctionNameE = p.FunctionNameE, Rank = p.Rank, Parent = p.Parent, Tooltips = p.Tooltips, FView = (p.FView != 0 && p.FView != null), FEdit = (p.FEdit != 0 && p.FEdit != null), FDelete = (p.FDelete != 0 && p.FDelete != null), FExport = (p.FExport != 0 && p.FExport != null) }).ToList();
        //        moduleList.Add(new SYS_tblFunctionPermissionViewModel() { Url = "/Admin/ChangePassword" });
        //        EmployeeRepository _repository = new EmployeeRepository(db);

        //        EmployeeViewModel Emp = (EmployeeViewModel)Session[SettingKeys.EmpInfo];
        //        EmployeeViewModel model = new EmployeeViewModel();
        //        if (Emp != null)
        //        {
        //            model = _repository.GetEmployee(Emp.EmpID, LanguageId);
        //            if (model == null)
        //            {
        //                model = new EmployeeViewModel() { FullName = acc.UserName };
        //            }
        //            Session[SettingKeys.EmpInfo] = model;
        //        }

        //        EmployeeViewModel EmpId = (EmployeeViewModel)Session[SettingKeys.EmpInfo];
        //        if (EmpId != null)
        //        {
        //            if (Emp != null && Emp.EmpID == EmpId.EmpID)
        //            {
        //                Session[SettingKeys.EmpInfo] = model;
        //            }
        //            else
        //            {
        //                var modelEmpId = _repository.GetEmployee(EmpId.EmpID, LanguageId);
        //                if (modelEmpId != null)
        //                {
        //                    modelEmpId = new EmployeeViewModel() { FullName = acc.UserName };
        //                }
        //                Session[SettingKeys.EmpInfo] = modelEmpId;
        //            }
        //        }

        //        Session[SettingKeys.MenuList] = moduleList;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //    Response.Redirect(DesiredUrl);

        //    //if (CurrentLanguage.Contains(ConstantClass.Language_en)) CurrentLanguage = "en-US";
        //    //else CurrentLanguage = "vi-VN";
        //    //Session[ConstantClass.Session_CultureLanguage] = new CultureInfo(CurrentLanguage);
        //    //Session[ConstantClass.Session_Language] = CurrentLanguage.Substring(0, 2);
        //    //try
        //    //{
        //    //    HomeRepository _pageRepository = new HomeRepository(db);
        //    //    List<PageViewModel> pagelst = _pageRepository.GetMenu(Session[ConstantClass.Session_Language].ToString());

        //    //    Session[ConstantClass.Session_Page] = pagelst;
        //    //    Session[ConstantClass.Session_ChangeLanguage] = true;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    ex.ToString();
        //    //}
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
                    LanguageService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
