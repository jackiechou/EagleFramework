using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Controllers
{
    public class SupplierController : Controller
    {
        private IApplicationService ApplicationService { get; set; }
        private IDocumentService DocumentService { get; set; }
        private ISupplierService SupplierService { get; set; }

        public SupplierController(IApplicationService applicationService, ISupplierService supplierService, IDocumentService documentService)
        {
            ApplicationService = applicationService;
            SupplierService = supplierService;
            DocumentService = documentService;
        }

        // GET: /Admin/Supplier/List
        [HttpGet]
        public ActionResult GetManufacturers(int? page=1)
        {
            int? recordCount = 0;
            int vendorId = GlobalSettings.DefaultVendorId;
            var sources = SupplierService.GetManufacturerList(vendorId, ManufacturerStatus.Active, ref recordCount, "ManufacturerId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Manufacturer/_ManufacturerSlider",lst);
        }

        // GET: /Admin/Supplier/Details/5        
        [HttpGet]
        public ActionResult Details(int id)
        {
            var entity = SupplierService.GetManufacturerDetail(id);

            var model = new ManufacturerInfoDetail
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
            
            return View("../Manufacturer/ManufacturerDetail", model);
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
                    ApplicationService = null;
                    SupplierService = null;
                    SupplierService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}