using System;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Attributes;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Core.UI.Layout;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Eagle.WebApp.Attributes.ThemeLayout;
using Eagle.WebApp.Common;


namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class ModuleController : BaseController
    {
        private IModuleService ModuleService { get; set; }
        private IPageService PageService { get; set; }
        private IContentService ContentService { get; set; }
        private IRoleService RoleService { get; set; }

        public ModuleController(IModuleService moduleService, IPageService pageService, IContentService contentService, IRoleService roleService)
            : base(new IBaseService[] { moduleService, pageService, contentService, roleService })

        {
            ModuleService = moduleService;
            PageService = pageService;
            ContentService = contentService;
            RoleService = roleService;
        }
 

        #region MODULE
        public ActionResult PopulateModuleTypeSelectList()
        {
            var lst = ModuleService.PopulateModuleTypeSelectList((int)ModuleType.Admin);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Search([System.Web.Http.FromBody]ModuleSearchEntry filter, string sourceEvent, [System.Web.Http.FromUri]int? page = 1)
        {
            if (String.IsNullOrEmpty(sourceEvent))
            {
                TempData["ModuleSearchRequest"] = filter;
                TempData.Keep();
            }
            else
            {
                if (TempData["ModuleSearchRequest"] != null)
                {
                    filter = (ModuleSearchEntry) TempData["ModuleSearchRequest"];
                    TempData.Keep();
                }
            }

            int recordCount;
            int pageSize = GlobalSettings.DefaultPageSize;
            var sources = ModuleService.Search(filter, out recordCount, "ModuleId ASC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Sys/Modules/_SearchResult", lst);
        }

        //
        // GET: /Admin/Module/        
        [PageLayout(LayoutSetting.FullMainLayout, "Module")]
        [RestoreModelStateFromTempData]
        public ActionResult Index()
        {
            return View("../Sys/Modules/Index");
        }

        //public ActionResult GetModules(int? moduleTypeId)
        //{
        //    var lst = ModuleService.GetModules((ModuleType?)moduleTypeId, null);
        //    return PartialView("../Sys/Modules/_SearchResult", lst);
        //}


        // GET: /Admin/Module/Create
        [HttpGet]
        [PageTitle("CreateModule")]
        public ActionResult Create()
        {
            var entry = new ModuleEntry();
            ViewBag.ModuleTypeId = ModuleService.PopulateModuleTypeSelectList((int)ModuleType.Admin);
            ViewData["AvailablePages"] = PageService.PopulatePageMultiSelectList(PageType.Admin, null, null);

            return PartialView("../Sys/Modules/_Create", entry);
        }

        // GET: /Admin/Module/Details/5        
        [HttpGet]
        [PageTitle("EditModule")]
        public ActionResult Edit(int id)
        {
            var item = ModuleService.GetModuleDetails(id);
            var entry = new ModuleEditEntry
            {
                ModuleId = item.ModuleId,
                ModuleTypeId = Convert.ToInt32(item.ModuleTypeId),
                ModuleCode = item.ModuleCode,
                ModuleName = item.ModuleName,
                ModuleTitle = item.ModuleTitle,
                Description = item.Description,
                Header = item.Header,
                Footer = item.Footer,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                InheritViewPermissions = item.InheritViewPermissions ?? false,
                AllPages = item.AllPages ?? false,
                IsSecured = item.IsSecured ?? false,
                ExistedModuleCapabilities = ModuleService.GetModuleCapabilities(item.ModuleId)
            };

            ViewBag.Title = LanguageResource.EditModule;
            ViewBag.ModuleTypeId = ModuleService.PopulateModuleTypeSelectList((int)item.ModuleTypeId);
            ViewData["AvailablePages"] = PageService.PopulatePageMultiSelectList(PageType.Admin, null, item.ModuleId);

            //ViewBag.Visibility = ModuleService.PopulateVisibilityList(null);
            //ViewBag.Pane = ModuleService.PopulatePaneList(null);
            //ViewBag.InsertedPosition = ModuleService.PopulateInsertedPositionList(null, true);
            //ViewBag.Alignment = ModuleService.PopulateAlignmentList(null, true);

            return PartialView("../Sys/Modules/_Edit", entry);
        }

        // POST: /Admin/Module/Create
        [HttpPost]
        public ActionResult Create(ModuleEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ModuleService.InsertModule(ApplicationId, RoleId, entry);

                    return Json(new SuccessResult { Status = ResultStatusType.Success,
                        Data = new { Message = LanguageResource.CreateSuccess }
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


        ////
        //// POST: 
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(ModuleEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ModuleService.UpdateModule(RoleId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ModuleId
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
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ModuleService.UpdateModuleStatus(id, status? ModuleStatus.Active : ModuleStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = new {
                        Message = LanguageResource.UpdateStatusSuccess,
                        Id = id
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Admin/Module/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                ModuleService.DeleteModule(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region MODULE PAGES
        /// <summary>
        /// AutoComplete with select2
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageTypeId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAutoCompletePages([System.Web.Http.FromBody]string search, [System.Web.Http.FromUri] int? page=1, [System.Web.Http.FromBody] PageType? pageTypeId = PageType.Admin)
        {
            int recordCount;
            var jsonList = PageService.GetAutoCompletePages(out recordCount, search, pageTypeId, page);
            return new JsonpResult { Data = jsonList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult PopulatePageMultiSelectList(PageType pageTypeId = PageType.Admin, int? moduleId=null)
        {
            var lst = PageService.PopulatePageMultiSelectList(pageTypeId, null, moduleId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditModulePage(int moduleId, PageType typeId)
        {
            ViewData["AvailablePages"] = PageService.PopulatePageMultiSelectList(typeId, null, moduleId);
            ViewData["SelectedPages"] = PageService.PopulatePageByModuleIdMultiSelectList(moduleId);

            return PartialView("../Sys/Modules/ModulePages/_EditModulePage");
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

        #endregion

        #region MODULE PERMISSION

        [HttpGet]
        [PageTitle("EditPermission")]
        public ActionResult EditPermission(int id)
        {
            var item = ModuleService.GetModuleRolePermissionEntry(ApplicationId, id);
            return PartialView("../Sys/Modules/ModulePermission/_EditModulePermission", item);
        }

        [HttpPost]
        public ActionResult UpdatePermissionStatus(Guid roleId, int moduleId, int capabilityId, bool status)
        {
            string message;
            bool flag = false;
            try
            {
                ModuleService.UpdateModulePermissionStatus(roleId, moduleId, capabilityId, status);
                flag = true;
                message = LanguageResource.UpdateSuccess;
            }
            catch (ValidationError ex)
            {
                message = ValidationExtension.ConvertValidateErrorToString(ex);
            }
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region MODULE CAPABILITY
        //GET
        [HttpGet]
        public ActionResult CreateModuleCapability(int moduleId)
        {
            return PartialView("../Sys/Modules/ModuleCapabilities/_CreateModuleCapability");
        }

        public ActionResult GetModuleCapabilitiesByModuleId(int moduleId)
        {
            var lst = ModuleService.GetModuleCapabilitiesByModuleId(moduleId, null);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        // POST: /Admin/ModuleCapability/Create
        [HttpPost]
        public ActionResult CreateModuleCapability(ModuleCapabilityEntry entry)
        {
            bool flag = false;
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    var capability = new CapabilityEntry
                    {
                        CapabilityName = entry.CapabilityName,
                        Description = entry.CapabilityName,
                        IsActive = entry.IsActive,
                    };
                    ModuleService.CreateModuleCapability(entry.ModuleId, capability);
                    flag = true;
                    message = LanguageResource.CreateSuccess;
                }
                catch (ValidationError ex)
                {
                    message = ValidationExtension.ConvertValidateErrorToString(ex);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "<br/>"));
            }
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }

        // POST: /Admin/ModuleCapability/Edit
        [HttpPost]
        public ActionResult EditModuleCapability(ModuleCapabilityEditEntry entry)
        {
            bool flag = false;
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    var item = new ModuleCapabilityEntry
                    {
                        CapabilityName = entry.CapabilityName,
                        Description = entry.Description,
                        ModuleId = entry.ModuleId
                    };

                    ModuleService.UpdateModuleCapability(entry.CapabilityId, item);
                    flag = true;
                    message = LanguageResource.CreateSuccess;
                }
                catch (ValidationError ex)
                {
                    message = ValidationExtension.ConvertValidateErrorToString(ex);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "<br/>"));
            }
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }

        // POST: /Admin/ModuleCapability/Edit
        [HttpPost]
        public ActionResult UpdateModuleCapabilityStatus(int id, ModuleCapabilityStatus status)
        {
            bool flag = false;
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    ModuleService.UpdateModuleCapabilityStatus(id, status);
                    flag = true;
                    message = LanguageResource.UpdateSuccess;
                }
                catch (ValidationError ex)
                {
                    message = ValidationExtension.ConvertValidateErrorToString(ex);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "<br/>"));
            }
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region Dispose

        private bool _disposed;

        [System.Web.Mvc.NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    ModuleService = null;
                    PageService = null;
                    ContentService = null;
                    RoleService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
