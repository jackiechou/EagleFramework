using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
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
    public class VendorPartnerController : BaseController
    {
        private IDocumentService DocumentService { get; set; }
        private IVendorService VendorService { get; set; }

        public VendorPartnerController(IVendorService vendorService, IDocumentService documentService)
            : base(new IBaseService[] { vendorService, documentService })
        {
            VendorService = vendorService;
            DocumentService = documentService;
        }

        #region GET METHODS =========================================================================
        // GET: Admin/VendorPartner
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/VendorPartner/Index");
        }

        // GET: /Admin/Vendor/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = VendorService.GetPartnerDetail(id);

            var model = new VendorPartnerEditEntry
            {
                PartnerId = item.PartnerId,
                PartnerName = item.PartnerName,
                Logo = item.Logo,
                Telephone = item.Telephone,
                Mobile = item.Mobile,
                Fax = item.Fax,
                Email = item.Email,
                Description = item.Description,
                Status = item.Status
            };

            if (item.Logo != null)
            {
                var fileInfo = DocumentService.GetFileInfoDetail((int)item.Logo);
                if (fileInfo != null)
                {
                    model.FileUrl = fileInfo.FileUrl;
                }
            }
            
            return PartialView("../Business/Ecommerce/VendorPartner/_Edit", model);
        }

        // GET: /Admin/VendorPartner/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.Status = VendorService.PopulatePartnerStatus(null,true);
            return PartialView("../Business/Ecommerce/VendorPartner/_SearchForm");
        }

        // GET: /Admin/VendorPartner/Search
        [HttpGet]
        public ActionResult Search(VendorPartnerSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["VendorPartnerSearchRequest"] = filter;
            }
            else
            {
                if (TempData["VendorPartnerSearchRequest"] != null)
                {
                    filter = (VendorPartnerSearchEntry)TempData["VendorPartnerSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = VendorService.GetPartners(filter, ref recordCount, "PartnerId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/VendorPartner/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/VendorPartner/Create
        [HttpPost]
        public ActionResult Create(VendorPartnerEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    VendorService.InsertPartner(ApplicationId, UserId, VendorId, entry);
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
        // PUT - /Admin/VendorPartner/Edit
        [HttpPut]
        public ActionResult Edit(VendorPartnerEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    VendorService.UpdatePartner(ApplicationId, UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.PartnerId
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
        // POST: /Admin/VendorPartner/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                VendorService.UpdatePartnerStatus(id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion ==============================================================================

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    DocumentService = null;
                    VendorService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}