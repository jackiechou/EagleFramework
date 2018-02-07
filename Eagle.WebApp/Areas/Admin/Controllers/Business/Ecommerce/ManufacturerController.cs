using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class ManufacturerController : BaseController
    {
        private ISupplierService SupplierService { get; set; }
        private IDocumentService DocumentService { get; set; }
        public ManufacturerController(ISupplierService supplierService, IDocumentService documentService)
            : base(new IBaseService[] { supplierService, documentService })
        {
            SupplierService = supplierService;
            DocumentService = documentService;
        }

        #region GET METHODS =========================================================================

        // GET: Admin/Manufacturer
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Manufacturer/Index");
        }

        // GET: /Admin/Manufacturer/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CategoryId = SupplierService.PoplulateManufacturerCategorySelectList(ManufacturerCategoryStatus.Active);
            return PartialView("../Business/Ecommerce/Manufacturer/_Create");
        }

        // GET: /Admin/Manufacturer/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = SupplierService.GetManufacturerDetail(id);

            var model = new ManufacturerEditEntry
            {
                CategoryId = entity.CategoryId,
                ManufacturerId = entity.ManufacturerId,
                ManufacturerName = entity.ManufacturerName,
                Email = entity.Email,
                Phone = entity.Phone,
                Fax = entity.Fax,
                Address = entity.Address,
                IsActive = entity.IsActive,
                Photo = entity.Photo
            };

            if (entity.Photo != null)
            {
                var fileInfo = DocumentService.GetFileInfoDetail((int)entity.Photo);
                if (fileInfo != null)
                {
                    model.FileUrl = fileInfo.FileUrl;
                }
            }

            ViewBag.CategoryId = SupplierService.PoplulateManufacturerCategorySelectList(null,entity.CategoryId);

            return PartialView("../Business/Ecommerce/Manufacturer/_Edit", model);
        }

        // GET: /Admin/Manufacturer/List
        [HttpGet]
        public ActionResult List(ManufacturerSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ManufacturerSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ManufacturerSearchRequest"] != null)
                {
                    filter = (ManufacturerSearchEntry)TempData["ManufacturerSearchRequest"];
                }
            }
            TempData.Keep();
            int? recordCount = 0;
            var sources = SupplierService.GetManufacturers(VendorId, filter, ref recordCount, "ManufacturerId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Manufacturer/_List", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/Manufacturer/Create
        [HttpPost]
        public ActionResult Create(ManufacturerEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SupplierService.InsertManufacturer(ApplicationId, UserId, VendorId, entry);
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

        //
        // PUT - /Admin/Manufacturer/Edit
        [HttpPut]
        public ActionResult Edit(ManufacturerEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SupplierService.UpdateManufacturer(ApplicationId, UserId, VendorId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.CategoryId
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

        //
        // POST: /Admin/Manufacturer/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                SupplierService.UpdateManufacturerStatus(id, status? ManufacturerStatus.Active: ManufacturerStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/Manufacturer/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                SupplierService.UpdateManufacturerStatus(id, ManufacturerStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion =============================================================================================================

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    SupplierService = null;
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}