using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class ManufacturerCategoryController : BaseController
    {
        private ISupplierService SupplierService { get; set; }
        public ManufacturerCategoryController(ISupplierService supplierService) : base(new IBaseService[] { supplierService })
        {
            SupplierService = supplierService;
        }
        
        #region GET METHODS =========================================================================

        // GET: Admin/ManufacturerCategory
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/ManufacturerCategory/Index");
        }

        // GET: /Admin/ManufacturerCategory/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Business/Ecommerce/ManufacturerCategory/_Create");
        }

        // GET: /Admin/ManufacturerCategory/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = SupplierService.GetManufacturerCategoryDetail(id);

            var editModel = new ManufacturerCategoryEditEntry
            {
                CategoryId = entity.CategoryId,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
            return PartialView("../Business/Ecommerce/ManufacturerCategory/_Edit", editModel);
        }

        // GET: /Admin/ManufacturerCategory/List
        [HttpGet]
        public ActionResult List(ManufacturerCategorySearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ManufacturerCategorySearchRequest"] = filter;
            }
            else
            {
                if (TempData["ManufacturerCategorySearchRequest"] != null)
                {
                    filter = (ManufacturerCategorySearchEntry)TempData["ManufacturerCategorySearchRequest"];
                }
            }
            TempData.Keep();
            int? recordCount = 0;
            var sources = SupplierService.GeManufacturerCategories(VendorId, filter, ref recordCount,"CategoryId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/ManufacturerCategory/_List", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ManufacturerCategory/Create
        [HttpPost]
        public ActionResult Create(ManufacturerCategoryEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SupplierService.InsertManufacturerCategory(VendorId, entry);
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
        // PUT - /Admin/ManufacturerCategory/Edit
        [HttpPut]
        public ActionResult Edit(ManufacturerCategoryEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SupplierService.UpdateManufacturerCategory(entry);
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
        // POST: /Admin/ManufacturerCategory/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                SupplierService.UpdateManufacturerCategoryStatus(id, status? ManufacturerCategoryStatus.Active: ManufacturerCategoryStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/ManufacturerCategory/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                SupplierService.UpdateManufacturerCategoryStatus(id,ManufacturerCategoryStatus.InActive);
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
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}