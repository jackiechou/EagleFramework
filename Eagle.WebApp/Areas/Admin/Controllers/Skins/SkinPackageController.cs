using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Skins;
using Eagle.Services.Skins;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Skins
{
    public class SkinPackageController : BaseController
    {
        private IThemeService ThemeService { get; set; }

        public SkinPackageController(IThemeService themeService)
            : base(new IBaseService[] { themeService })
        {
            ThemeService = themeService;
        }
       
        //
        // GET: /Admin/SkinPackage/
        public ActionResult Index()
        {
            return View("../Skins/SkinPackage/Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var entry = new SkinPackageEntry();
            ViewBag.TypeId = ThemeService.PopulateSkinPackageTypeSelectList();
            return PartialView("../Skins/SkinPackage/_Create", entry);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = ThemeService.GetSkinPackageDetail(id);
            var model = new SkinPackageEditEntry
            {
                TypeId = item.TypeId,
                PackageId = item.PackageId,
                PackageName = item.PackageName,
                PackageSrc = item.PackageSrc,
                IsActive = item.IsActive
            };
            ViewBag.TypeId = ThemeService.PopulateSkinPackageTypeSelectList(item.TypeId);
            return PartialView("../Skins/SkinPackage/_Edit", model);
        }

        // GET: /Admin/SkinPackage/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.SearchTypeId = ThemeService.PopulateSkinPackageTypeSelectList();
            ViewBag.SearchStatus = ThemeService.PopulateSkinPackageStatus();
            return PartialView("../Skins/SkinPackage/_SearchForm");
        }

        [HttpGet]
        public ActionResult Search([System.Web.Http.FromBody]SkinPackageSearchEntry filter, string sourceEvent, [System.Web.Http.FromUri]int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["SkinPackageSearchRequest"] = filter;
                TempData.Keep();
            }
            else
            {
                if (TempData["SkinPackageSearchRequest"] != null && page > 1)
                {
                    filter = (SkinPackageSearchEntry)TempData["SkinPackageSearchRequest"];
                    TempData.Keep();
                }
            }

            int recordCount;
            var sources = ThemeService.GetSkinPackages(ApplicationId, filter, out recordCount, "PackageId ASC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Skins/SkinPackage/_SearchResult", lst);
        }

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/SkinPackage/Create
        [HttpPost]
        public ActionResult Create(SkinPackageEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ThemeService.InsertSkinPackage(ApplicationId, entry);
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
        
        // PUT - /Admin/SkinPackage/Edit
        [HttpPut]
        public ActionResult Edit(SkinPackageEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ThemeService.UpdateSkinPackage(ApplicationId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.PackageId
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
        
        // PUT: /Admin/SkinPackage/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ThemeService.UpdateSkinPackageStatus(id, status);
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

        // PUT: /Admin/Skin/UpdateSelectedPackage/5
        [HttpPut]
        public ActionResult UpdateSelectedPackage(int id)
        {
            try
            {
                ThemeService.UpdateSelectedSkin(ApplicationId, id);
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
