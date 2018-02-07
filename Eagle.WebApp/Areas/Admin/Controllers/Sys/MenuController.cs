using System;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Eagle.WebApp.Attributes.ThemeLayout;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class MenuController : BaseController
    {
        private IMenuService MenuService { get; set; }
        private IPageService PageService { get; set; }

        public MenuController(IMenuService menuService, IPageService pageService) : base(new IBaseService[] { menuService, pageService })
        {
            MenuService = menuService;
            PageService = pageService;
        }

        //
        // GET: /Admin/Menu/
        public ActionResult Index()
        {
            ViewData["TypeId"] = MenuService.PopulateMenuTypeSelectList();
            return View("../Sys/Menu/Index");
        }

        #region LOAD MENU =====================================================================================================

        [HttpGet]
        public JsonResult PopulateSiteMapByMenuCode(string menuCode)
        {
            var result = MenuService.PopulateSiteMapByMenuCode(menuCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PopulateSiteMapByMenuId(string menuId)
        {
            var result = MenuService.PopulateSiteMapByMenuId(menuId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult PopulateListBox([System.Web.Http.FromBody]int? typeId)
        {
            var sources = MenuService.GetTreeList(typeId ?? Convert.ToInt32(MenuTypeSetting.Admin)).ToList();
            return Json(sources, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load Admin Main Menu
        /// </summary>
        /// <returns></returns>
        //[ChildActionOnly]
        public ActionResult LoadAdminMainMenu()
        {
            string strHtml = MenuService.LoadMenu(ApplicationId, UserId, GlobalSettings.DefaultRoleId);
            return PartialView("_MenuPartial", strHtml);
        }

        //[ChildActionOnly]
        public ActionResult LoadLeftMainMenu()
        {
            string strHtml = MenuService.LoadMenu(ApplicationId, UserId, GlobalSettings.DefaultRoleId);
            return PartialView("_LeftMainMenu", strHtml);
        }

        [HttpGet]
        public JsonResult GetHierachicalList(int typeId, bool? isRootShowed = true)
        {
            var list = MenuService.GetHierachicalList(typeId, null, isRootShowed).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion LOAD MENU ==================================================================================================

        #region INSERT UPDATE DELETE MENU ==================================================================================================
        [HttpGet]
        public ActionResult Create()
        {
            ViewData["TypeId"] = MenuService.PopulateMenuTypeSelectList();
            ViewBag.PageId = PageService.PopulatePageSelectList(PageType.Admin, null, null, true);
            ViewBag.MenuStatus = MenuService.GenerateMenuStatuses(null, true);
            ViewBag.Target = MenuService.PopulateLinkTargets(null);
            ViewBag.AvailablePositions = MenuService.PopulateMenuPositionMultiSelectList();
            ViewBag.SelectedPositions = MenuService.PopulateMenuPositionMultiSelectedList(null);

            return PartialView("../Sys/Menu/_Create", new MenuEntry());
        }


        [HttpGet]
        public ActionResult Edit([System.Web.Http.FromBody]int id)
        {
            var model = new MenuEditEntry();
            var item = MenuService.GetMenuDetails(id);
            if (item != null)
            {
                model.MenuId = item.MenuId;
                model.PageId = item.PageId;
                model.TypeId = item.TypeId;
                model.ParentId = item.ParentId;
                model.MenuName = item.MenuName;
                model.MenuTitle = item.MenuTitle;
                model.Target = item.Target;
                model.IconClass = item.IconClass;
                model.IconFile = item.IconFile;
                model.Color = item.Color;
                model.CssClass = item.CssClass;
                model.Description = item.Description;
                model.Status = item.Status;
                model.IsSecured = item.IsSecured;
                model.DocumentFileInfo = item.DocumentFileInfo;
            }

            //var pageType = (item.IsSecured != null && item.IsSecured == true) ? PageType.Admin : PageType.Site;
            ViewBag.TypeId = MenuService.PopulateMenuTypeSelectList(null, item.TypeId);
            ViewBag.PageId = PageService.PopulatePageSelectList((PageType)item.TypeId, null, item.PageId, true);
            ViewBag.Status = MenuService.GenerateMenuStatuses(Convert.ToInt32(item.Status));
            ViewBag.Target = MenuService.PopulateLinkTargets(item.Target);

            ViewBag.AvailablePositions = MenuService.PopulateMenuPositionMultiSelectList(item.TypeId, null, item.PositionId);
            ViewBag.SelectedPositions = MenuService.PopulateMenuPositionMultiSelectedList(item.PositionId);

            return PartialView("../Sys/Menu/_Edit", model);
        }

        // POST: /Admin/Menu/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MenuEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int id = MenuService.Insert(ApplicationId, UserId, RoleId, VendorId, entry);
                    if (id > 0)
                    {
                        return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.CreateSuccess }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new SuccessResult { Status = ResultStatusType.Fail, Data = LanguageResource.CreateFailure }, JsonRequestBehavior.AllowGet);
                    }
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

        // POST: /Admin/Menu/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MenuService.Update(ApplicationId, UserId, RoleId, VendorId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.MenuId
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
        public ActionResult UpdateListOrder(int id, int parentId, int listOrder)
        {
            string message;
            bool flag = false;
            try
            {
                MenuService.UpdateListOrder(id, parentId, listOrder);
                flag = true;
                message = LanguageResource.UpdateSuccess;
            }
            catch (ValidationError ex)
            {
                message = ValidationExtension.ConvertValidateErrorToString(ex);
            }
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Admin/Menu/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = false;
            string message;
            try
            {
                MenuService.Delete(id);
                result = true;
                message = "Update Service Request successfully!";
            }
            catch (ValidationError ex)
            {
                message = ValidationExtension.ConvertValidateErrorToString(ex);
            }
            return Json(JsonUtils.SerializeResult(result, message), JsonRequestBehavior.AllowGet);
        }

        #endregion XU LY INSERT UPDATE DELETE MENU ==================================================================================================

        #region MENU PERMISSION
        [HttpGet]
        public JsonResult PopulateMenuPositionMultiSelectList([System.Web.Http.FromBody]int? typeId)
        {
            var sources = MenuService.PopulateMenuPositionMultiSelectList(typeId);
            return Json(sources, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PopulateMenuPositionMultiSelectedList([System.Web.Http.FromBody]int? menuId)
        {
            var sources = MenuService.PopulateMenuPositionMultiSelectedListByMenuId(menuId);
            return Json(sources, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [PageTitle("EditPermission")]
        public ActionResult EditPermission(int id)
        {
            var item = MenuService.GetMenuRolePermissionEntry(ApplicationId, id);
            return PartialView("../Sys/Menu/_EditMenuPermission", item);
        }

        [HttpPost]
        public ActionResult UpdatePermissionStatus(Guid roleId, int menuId, int permissionId, bool status)
        {
            string message;
            bool flag = false;
            try
            {
                MenuService.UpdateMenuPermissionStatus(roleId, menuId, permissionId, status);
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
                    PageService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
