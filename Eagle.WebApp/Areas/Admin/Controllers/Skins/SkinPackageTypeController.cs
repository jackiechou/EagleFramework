using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Skins;
using Eagle.Services.Skins;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Skins
{
    public class SkinPackageTypeController : BaseController
    {
        private IThemeService ThemeService { get; set; }
    
        public SkinPackageTypeController(IThemeService themeService)
            : base(new IBaseService[] { themeService })
        {
            ThemeService = themeService;
        }
        // GET: Admin/SkinPackageType
        public ActionResult Index()
        {
            return View("../Skins/SkinPackageType/Index");
        }


        [HttpGet]
        public ActionResult Create()
        {
            var entry = new SkinPackageTypeEntry();
            return PartialView("../Skins/SkinPackageType/_Create", entry);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = ThemeService.GetSkinPackageTypeDetail(id);
            var model = new SkinPackageTypeEditEntry
            {
                TypeId = item.TypeId,
                TypeName = item.TypeName,
                IsActive = item.IsActive
            };
            return PartialView("../Skins/SkinPackageType/_Edit", model);
        }

        // GET: /Admin/SkinPackageType/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.Status = ThemeService.PopulateSkinPackageTypeStatus();
            return PartialView("../Skins/SkinPackageType/_SearchForm");
        }

        [HttpGet]
        public ActionResult Search(bool? status)
        {
            var lst = ThemeService.GetSkinPackageTypes(status);
            return PartialView("../Skins/SkinPackageType/_SearchResult", lst);
        }

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/SkinPackageType/Create
        [HttpPost]
        public ActionResult Create(SkinPackageTypeEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ThemeService.InsertSkinPackageType(entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = new { Message = LanguageResource.CreateSuccess } }, JsonRequestBehavior.AllowGet);
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


        // PUT - /Admin/SkinPackageType/Edit
        [HttpPut]
        public ActionResult Edit(SkinPackageTypeEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ThemeService.UpdateSkinPackageType(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.TypeId
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


        // PUT: /Admin/SkinPackageType/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ThemeService.UpdateSkinPackageTypeStatus(id, status);
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new
                    {
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

        #endregion =====================================================================================

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    ThemeService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}