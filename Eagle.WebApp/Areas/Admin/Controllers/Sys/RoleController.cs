using System;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class RoleController : BaseController
    {
        private  IRoleService RoleService { get; set; }
    
        public RoleController(IRoleService roleService) : base(new IBaseService[] { roleService })
        {
            RoleService = roleService;
        }

        //
        // GET: /Admin/Role/
        public ActionResult Index()
        {
            return View("../Sys/Roles/Index");
        }

        [HttpGet]
        public ActionResult LoadGroupsByRole(Guid roleId)
        {
            var groups = RoleService.GetRoleGroups(ApplicationId, roleId);
            return PartialView("../Sys/Roles/_GroupsByRole", groups);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Sys/Roles/_Create");
        }

        [HttpGet]
        public ActionResult Edit([System.Web.Http.FromBody]Guid id)
        {
            var item = RoleService.GetRoleDetails(id);
            var editEntry = new RoleEditEntry
            {
                RoleId = item.RoleId,
                RoleName = item.RoleName,
                Description = item.Description,
                IsActive = item.IsActive
            };
            return PartialView("../Sys/Roles/_Edit", editEntry);
        }

        [HttpGet]
        public ActionResult PopulateAvailableGroupMultiSelectList()
        {
            var lst = RoleService.PopulateGroupMultiSelectList(ApplicationId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PopulateSelectedGroupMultiSelectList(Guid roleId)
        {
            var lst = RoleService.PopulateGroupMultiSelectList(ApplicationId, roleId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.Status = RoleService.PopulateStatusDropDownList();
            ViewBag.GroupId = RoleService.PopulateGroupDropDownList(ApplicationId, null, null, true);
            return PartialView("../Sys/Roles/_Search");
        }

        [HttpGet]
        public ActionResult Search([System.Web.Http.FromBody]RoleSearchEntry filter, string sourceEvent, [System.Web.Http.FromUri]int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["RoleSearchRequest"] = filter;
            }
            else
            {
                if (TempData["RoleSearchRequest"] != null)
                {
                    filter = (RoleSearchEntry)TempData["RoleSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = RoleService.SearchRoles(ApplicationId, filter, ref recordCount, "ListOrder ASC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Sys/Roles/_SearchResult", lst);
        }

        [HttpPost]
        //[ValidatePermission(Action = "Create", Model = "Role")]
        public ActionResult Create(RoleEntry entry)
        {
            bool flag = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    RoleService.CreateRole(ApplicationId, entry);
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
            ModelState.AddModelError("", message);
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(RoleEditEntry entry)
        {
            bool flag = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    RoleService.UpdateRole(ApplicationId, entry);
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
            ModelState.AddModelError("", message);
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateStatus(Guid id, bool status)
        {
            var flag = false;
            var message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    RoleService.UpdateRoleStatus(id, status);
                    flag = true;
                    message = LanguageResource.UpdateStatusSuccess;
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
            ModelState.AddModelError("", message);
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveRoleGroup([System.Web.Http.FromBody]Guid roleId, [System.Web.Http.FromBody]Guid groupId)
        {
            var flag = false;
            var message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    RoleService.DeleteRoleGroup(roleId, groupId);
                    flag = true;
                    message = LanguageResource.UpdateStatusSuccess;
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
            ModelState.AddModelError("", message);
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
                    RoleService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
