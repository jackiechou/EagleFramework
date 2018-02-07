using System.Linq;
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
    public class VendorController : BaseController
    {
        private IContactService ContactService { get; set; }
        private IDocumentService DocumentService { get; set; }
        private IVendorService VendorService { get; set; }

        public VendorController(IVendorService vendorService, IContactService contactService, IDocumentService documentService)
            : base(new IBaseService[] { vendorService, contactService, documentService })
        {
            VendorService = vendorService;
            ContactService = contactService;
            DocumentService = documentService;
        }
        #region GET METHODS =========================================================================
        //
        // GET: /Admin/Vendor/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Vendor/Index");
        }

        // GET: /Admin/Vendor/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = VendorService.GetVendorDetail(id);

            var model = new VendorEditEntry
            {
                VendorId = item.VendorId,
                VendorName = item.VendorName,
                StoreName = item.StoreName,
                AccountNumber = item.AccountNumber,
                Logo = item.Logo,
                Slogan = item.Slogan,
                Telephone = item.Telephone,
                Mobile = item.Mobile,
                Fax = item.Fax,
                Email = item.Email,
                Hotline = item.Hotline,
                SupportOnline = item.SupportOnline,
                Website = item.Website,
                ClickThroughs = item.ClickThroughs,
                CreditRating = item.CreditRating,
                TermsOfService = item.TermsOfService,
                Keywords = item.Keywords,
                Summary = item.Summary,
                Description = item.Description,
                RefundPolicy = item.RefundPolicy,
                ShoppingHelp = item.ShoppingHelp,
                OrganizationalStructure = item.OrganizationalStructure,
                FunctionalAreas = item.FunctionalAreas,
            };

            if (item.Logo != null)
            {
                var fileInfo = DocumentService.GetFileInfoDetail((int)item.Logo);
                if (fileInfo != null)
                {
                    model.FileUrl = fileInfo.FileUrl;
                }
            }
           
            var vendorAddress = VendorService.GetVendorAddresses(item.VendorId).FirstOrDefault();
            if (vendorAddress!=null)
            {
                model.Address = new VendorAddressEditEntry
                {
                    AddressId = vendorAddress.Address.AddressId,
                    AddressTypeId = vendorAddress.Address.AddressTypeId,
                    CountryId = vendorAddress.Address.CountryId,
                    ProvinceId = vendorAddress.Address.ProvinceId,
                    RegionId = vendorAddress.Address.RegionId,
                    Street = vendorAddress.Address.Street,
                    PostalCode = vendorAddress.Address.PostalCode,
                    Description = vendorAddress.Address.Description
                };
            }

            return PartialView("../Business/Ecommerce/Vendor/_Edit", model);
        }

        // GET: /Admin/Vendor/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/Vendor/_SearchForm");
        }

        // GET: /Admin/Vendor/Search
        [HttpGet]
        public ActionResult Search(VendorSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["VendorSearchRequest"] = filter;
            }
            else
            {
                if (TempData["VendorSearchRequest"] != null)
                {
                    filter = (VendorSearchEntry)TempData["VendorSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = VendorService.GetVendors(filter, ref recordCount, "VendorId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Vendor/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/Vendor/Create
        [HttpPost]
        public ActionResult Create(VendorEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    VendorService.InsertVendor(ApplicationId, UserId, entry);
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
        // PUT - /Admin/Vendor/Edit
        [HttpPut]
        public ActionResult Edit(VendorEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    VendorService.UpdateVendor(ApplicationId, UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.VendorId
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
        // POST: /Admin/Vendor/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                VendorService.UpdateVendorStatus(UserId, id, status ? VendorStatus.Active : VendorStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/Vendor/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                VendorService.UpdateVendorStatus(UserId, id, VendorStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
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
                    ContactService = null;
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