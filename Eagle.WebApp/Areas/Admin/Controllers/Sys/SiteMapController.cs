using System.Web.Mvc;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class SiteMapController : BaseController
    {
        // GET: Admin/SiteMap
        private IMenuService MenuService { get; set; }

        public SiteMapController(IMenuService menuService) : base(new IBaseService[] { menuService })
        {
            MenuService = menuService;
        }

        // GET: Admin/SiteMap
        public ActionResult Index()
        {
            return View("../Sys/SiteMap/Index");
        }

        // GET: Hierachical List 
        [HttpGet]
        public ActionResult GetSiteMapSelectTree(int? selectedId = null, bool? isRootShowed = true)
        {
            var list = MenuService.GetSiteMapSelectTree(null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
       
        //
        // GET: /Admin/SiteMap/Create       
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Sys/SiteMap/_Create");
        }
        //
        // GET: /Admin/SiteMap/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = MenuService.GetSiteMapDetail(id);

            var entry = new SiteMapEditEntry
            {
                SiteMapId = item.SiteMapId,
                ParentId = item.ParentId,
                Title = item.Title,
                Action = item.Action,
                Controller = item.Controller,
                Url = item.Url,
                Frequency = item.Frequency,
                Priority = item.Priority,
                Status = item.Status
            };
            return PartialView("../Sys/SiteMap/_Edit", entry);
        }


        //
        // POST: /Admin/SiteMap/Create-
        [HttpPost]
        public ActionResult Create(SiteMapEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MenuService.InsertSiteMap(entry);
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

        ////
        //// PUT: /Admin/SiteMap/Edit/5
        [HttpPut]
        public ActionResult Edit(SiteMapEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MenuService.UpdateSiteMap(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.SiteMapId
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

      // POST: /Admin/SiteMap/UpdateStatus/5
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                MenuService.UpdateSiteMapStatus(id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Admin/SiteMap/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                MenuService.UpdateSiteMapStatus(id,false);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);

            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
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