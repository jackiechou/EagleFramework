using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Core.UI.Layout;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Skins;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Eagle.WebApp.Attributes.ThemeLayout;
using Eagle.WebApp.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class PageController : BaseController
    {
        private ILanguageService LanguageService { get; set; }
        private IUserService UserService { get; set; }
        private IPageService PageService { get; set; }
        private IModuleService ModuleService { get; set; }
        private IContentService ContentService { get; set; }
        private IThemeService ThemeService { get; set; }
        private IPermissionService PermissionService { get; set; }

        public PageController(IPageService pageService, IModuleService moduleService, IContentService contentService, ILanguageService languageService, IThemeService themeService, IUserService userService, IPermissionService permissionService)
            : base(new IBaseService[] { pageService, moduleService, contentService, languageService, themeService, userService, permissionService })
        {
            PageService = pageService;
            ModuleService = moduleService;
            ContentService = contentService;
            ThemeService = themeService;
            LanguageService = languageService;
            UserService = userService;
            PermissionService = permissionService;
        }


        #region PAGE PERMISSION

        [HttpGet]
        public ActionResult LoadPagePermission(int id)
        {
            var pagePermission = PermissionService.GetPagePermissions(ApplicationId, id);
            var pageEditEntry = new PageEditEntry
            {
                PagePermissions = pagePermission
            };
            return PartialView("../Sys/Pages/_PagePermission", pageEditEntry);
        }

        #endregion

        [HttpGet]
        public ActionResult PopulatePageMultiSelectList(int pageTypeId=1, int? moduleId=null)
        {
            var lst = PageService.PopulatePageMultiSelectList((PageType)pageTypeId, null, moduleId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PopulatePageByModuleMultiSelectList(int moduleId)
        {
            var lst = PageService.PopulatePageByModuleIdMultiSelectList(moduleId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PopulatePagesByModuleMultiSelectList(int moduleId, PageType? pageTypeId)
        {
            var lst = PageService.PopulatePagesByModuleIdMultiSelectList(moduleId, pageTypeId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PopulatePageSelectList(PageType? pageTypeId, bool? isShowSelectText = true)
        {
            var lst = PageService.PopulatePageSelectList(pageTypeId,null,null, isShowSelectText);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Administrator/Page/
        [PageLayout(LayoutSetting.FullMainLayout, "Page")]
        public ActionResult Index()
        {
            ViewBag.ParentId = PageService.PopulatePageSelectList(null);
            ViewBag.LanguageCode = LanguageService.PopulateApplicationLanguages(ApplicationId, ApplicationLanguageStatus.Active);

            return View("../Sys/Pages/Index");
        }
        
        //PopulateTemplateToDropDownList(bool? status, string selectedValue, bool isShowSelectText = false)
        // GET: /Admin/Page/Create
        [HttpGet]
        [PageTitle("CreatePage")]
        public ActionResult Create()
        {
            ViewBag.TemplateId = ThemeService.PopulateTemplateSelectListBySelectedSkin(null,true);
            ViewBag.PageTypeId = PageService.PopulatePageTypeSelectList((int)ModuleType.Admin);
            return PartialView("../Sys/Pages/_Create");
        }

        // GET: /Admin/Page/Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new PageEditEntry();
            var item = PageService.GetDetails(id);
            if (item != null)
            {
                model.PageId = item.PageId;
                model.TemplateId = item.TemplateId;
                model.PageCode = item.PageCode;
                model.PageName = item.PageName;
                model.PageTitle = item.PageTitle;
                model.PageUrl = item.PageUrl;
                model.PagePath = item.PagePath;
                model.IconClass = item.IconClass;
                model.Description = item.Description;
                model.Keywords = item.Keywords;
                model.PageHeadText = item.PageHeadText;
                model.PageFooterText = item.PageFooterText;
                model.StartDate = item.StartDate;
                model.EndDate = item.EndDate;
                model.DisableLink = item.DisableLink;
                model.DisplayTitle = item.DisplayTitle;
                model.IsExtenalLink = item.IsExtenalLink;
                model.IsMenu = item.IsMenu;
                model.IsSecured = item.IsSecured;
            }

            ViewBag.TemplateId = ThemeService.PopulateTemplateSelectListBySelectedSkin(item.TemplateId.ToString(), true);
            ViewBag.PageTypeId = PageService.PopulatePageTypeSelectList((int)item.PageTypeId);
            ViewBag.AvailableModules = ModuleService.PopulateModuleMultiSelectList((ModuleType?)item.PageTypeId, null, null);
            ViewBag.SelectedModules = PageService.PopulateModulesByPageMultiSelectList(item.PageId, (ModuleType?)item.PageTypeId);
            
            return PartialView("../Sys/Pages/_Edit", model);
        }

        [HttpGet]
        public ActionResult List()
        {
            var sources = PageService.GetListByPageTypeId(PageType.Admin, null);
            return PartialView("../Sys/Pages/_List", sources);
        }

        [HttpGet]
        public JsonResult GetList(string keywords, PageType? pageTypeId)
        {
            var lst = PageService.GetListByPageTypeIdAndKeywords(keywords, pageTypeId, null);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AutoComplete with select2
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageTypeId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAutoCompletePages([System.Web.Http.FromBody]string search, [System.Web.Http.FromUri] int? page, [System.Web.Http.FromBody] PageType? pageTypeId = null)
        {
            int recordCount;
            var jsonList = PageService.GetAutoCompletePages(out recordCount, search, pageTypeId, page);
            return new JsonpResult { Data = jsonList, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        [HttpGet]
        public ActionResult GetAutoCompleteDetails(int id)
        {
            var item = PageService.GetAutoCompleteDetails(id);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDetails(int id)
        {
            var item = PageService.GetDetails(id);
            return Json(item, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Search([System.Web.Http.FromBody]PageSearchEntry filter, [System.Web.Http.FromUri]string sourceEvent, [System.Web.Http.FromUri]int? page = 1)
        {
            if (String.IsNullOrEmpty(sourceEvent))
            {
                TempData["PageSearchRequest"] = filter;
            }
            else
            {
                if (TempData["PageSearchRequest"] != null)
                {
                    filter = (PageSearchEntry)TempData["PageSearchRequest"];
                }
            }
            TempData.Keep();

            int recordCount;
            int pageSize = 10;
            var sources = PageService.Search(filter.SearchText, filter.PageType, null, out recordCount, "PageId ASC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Sys/Pages/_SearchResult", lst);
        }

        [HttpGet]
        public ActionResult LoadPageTree(PageType pageTypeId)
        {
            string strHtml = PageService.LoadPageList(pageTypeId);
            return Json(strHtml, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoadPageTreeList(int pageTypeId)
        {
            var lst = PageService.GetPageTree((PageType)pageTypeId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult LoadDesktopPageTreeList()
        {
            var lst = PageService.GetPageTree(PageType.Site);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        //[ValidateModelState]
        //[ValidatePermission(Action = "Create", Model = "Role")]
        public ActionResult Create(PageEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PageService.Insert(ApplicationId, UserId, RoleId, LanguageCode, entry);
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

        [HttpPost]
        public ActionResult Edit(PageEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PageService.Update(UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.PageId
                        }
                    }, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult UpdateListOrder(string ids)
        {
            string message = string.Empty;
            bool flag = false;
            try
            {
                List<int> lst = ids.Split(',').Select(int.Parse).ToList();
                if (lst.Any())
                {
                    for (int i = 0; i < lst.Count(); i++)
                    {
                        var id = lst[i];
                        PageService.UpdateListOrder(UserId, id, i + 1);
                        flag = true;
                    }
                }
            }
            catch (ValidationError ex)
            {
                message = ValidationExtension.ConvertValidateErrorToString(ex);
            }
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult UpdateStatus(int id, bool status)
        {
            string message;
            bool flag = false;
            try
            {
                var pageStatus = status ? PageStatus.Active : PageStatus.InActive;
                PageService.UpdateStatus(UserId, id, pageStatus);
                flag = true;
                message = LanguageResource.UpdateSuccess;
            }
            catch (ValidationError ex)
            {
                message = ValidationExtension.ConvertValidateErrorToString(ex);
            }
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdatePagePermission(int pageId, Guid roleId, bool allowAccess, string userIds)
        {
            string message;
            bool flag = false;
            try
            {
                var entry = new PagePermissionEntry
                {
                    PageId = pageId,
                    RoleId = roleId,
                    AllowAccess = allowAccess,
                    UserIds = userIds
                };
                PermissionService.UpdatePagePermission(entry);
                flag = true;
                message = LanguageResource.UpdateSuccess;
            }
            catch (ValidationError ex)
            {
                message = ValidationExtension.ConvertValidateErrorToString(ex);
            }
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
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
                    LanguageService = null;
                    UserService = null;
                    PageService = null;
                    ModuleService = null;
                    ContentService = null;
                    ThemeService = null;
                    PermissionService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
