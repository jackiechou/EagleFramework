using System;
using System.Linq;
using System.Net;
using System.Web.Http;
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
    public class GroupController : BaseController
    {
        private IUserService UserService { get; set; }
        private IRoleService RoleService { get; set; }

        public GroupController(IRoleService roleService, IUserService userService)
            : base(new IBaseService[] { roleService , userService })
        {
            UserService = userService;
            RoleService = roleService;
        }
        //
        // GET: /Admin/Group/
        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            return View("../Sys/Group/Index");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Search([FromBody] GroupSearchEntry filter, string sourceEvent, [FromUri]int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["GroupSearchRequest"] = filter;
            }
            else
            {
                if (TempData["GroupSearchRequest"] != null)
                {
                    filter = (GroupSearchEntry)TempData["GroupSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount=0;
            var sources = RoleService.GetGroups(ApplicationId, filter, ref recordCount, "ListOrder DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            var searchResult = new GroupSearchResult
            {
                PagedList = lst,
                Filter = filter
            };

            return PartialView("../Sys/Group/_SearchResult", searchResult);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult _Create()
        {
            return PartialView("../Sys/Group/_Create");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult _Edit([FromBody]Guid id)
        {
            var item = RoleService.GetGroupDetails(id);
            var editEntry = new GroupEditEntry
            {
                GroupId = item.GroupId,
                GroupName = item.GroupName,
                Description = item.Description,
                IsActive = item.IsActive
            };
            return PartialView("../Sys/Group/_Edit", editEntry);
        }


        [System.Web.Mvc.HttpPost]
        //[ValidatePermission(Action = "Create", Model = "Role")]
        public ActionResult Create(GroupEntry entry)
        {
            bool flag = false;
            string message = string.Empty;
            HttpStatusCode statusCode;

            if (ModelState.IsValid)
            {
                try
                {
                    RoleService.CreateGroup(ApplicationId, entry);
                    flag = true;
                    message = LanguageResource.CreateSuccess;
                    statusCode = HttpStatusCode.Created;
                }
                catch (ValidationError ex)
                {
                    message = ValidationExtension.ConvertValidateErrorToString(ex);
                    statusCode = HttpStatusCode.ExpectationFailed;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "<br/>"));
                statusCode = HttpStatusCode.BadRequest;
            }
            ModelState.AddModelError("", message);
            return Json(new { status = statusCode, flag = flag, message = message }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        //[AjaxValidateAntiForgeryToken]
        public ActionResult Edit(GroupEditEntry entry)
        {
            bool flag = false;
            string message = string.Empty;
            HttpStatusCode statusCode;

            if (ModelState.IsValid)
            {
                try
                {
                    var roleGroupEntry = new GroupEntry
                    {
                        GroupName = entry.GroupName,
                        Description = entry.Description,
                        IsActive = entry.IsActive
                    };

                    RoleService.UpdateGroup(ApplicationId, entry.GroupId, roleGroupEntry);
                    flag = true;
                    message = LanguageResource.UpdateSuccess;
                    statusCode = HttpStatusCode.OK;
                }
                catch (ValidationError ex)
                {
                    message = ValidationExtension.ConvertValidateErrorToString(ex);
                    statusCode = HttpStatusCode.ExpectationFailed;
                    ModelState.AddModelError("", message);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "<br/>"));
                statusCode = HttpStatusCode.BadRequest;
                ModelState.AddModelError("", message);
            }
            
            return Json(new { status = statusCode, flag = flag, message = message }, JsonRequestBehavior.AllowGet);
        }
        
        [System.Web.Mvc.HttpPost]
        public ActionResult UpdateStatus(Guid id, bool status)
        {
            var result = false;
            var message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    RoleService.UpdateGroupStatus(id, status);

                    result = true;
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
                message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "-"));
            }

            ModelState.AddModelError("", message);
            return Json(JsonUtils.SerializeResult(result, message), JsonRequestBehavior.AllowGet);
           
        }

        ///// <summary>
        ///// Dùng cho viec binding du lieu cho dropdownlist autocomplete
        ///// </summary>
        ///// <param name="searchTerm"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="pageNum"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult DropdownList(string searchTerm, int pageSize, int pageNum)
        //{
        //    RoleRepository _repository = new RoleRepository();
        //    List<AutoComplete> sources = _repository.ListDropdown(searchTerm, pageSize, pageNum).ToList();
        //    int iTotal = sources.Count;

        //    //Translate the list into a format the select2 dropdown expects
        //    Select2PagedResultViewModel pagedList = BaseRepository.ConvertListToSelect2Format(sources, iTotal);

        //    //Return the data as a jsonp result
        //    return new JsonpResult
        //    {
        //        Data = pagedList,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
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
                    UserService = null;
                    RoleService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
