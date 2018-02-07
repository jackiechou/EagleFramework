using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Controllers
{
    public class VendorInfoController : BasicController
    {
        private IContactService ContactService { get; set; }
        private IDocumentService DocumentService { get; set; }
        private IVendorService VendorService { get; set; }
        private ICacheService CacheService { get; set; }
        public VendorInfoController(IVendorService vendorService, IContactService contactService, IDocumentService documentService, ICacheService cacheService)
        {
            VendorService = vendorService;
            ContactService = contactService;
            DocumentService = documentService;
            CacheService = cacheService;
        }

        [HttpGet]
        public ActionResult GetVendorOnHeader()
        {
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor != null) return PartialView("../VendorInfo/_VendorOnHeader", vendor);

            vendor = VendorService.GetDefaultVendor();
            CacheService.Set(CacheKeySetting.Vendor, vendor);
            return PartialView("../VendorInfo/_VendorOnHeader", vendor);
        }

        [HttpGet]
        public ActionResult GetVendorOnFooter()
        {
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor != null) return PartialView("../VendorInfo/_VendorOnFooter", vendor);

            vendor = VendorService.GetDefaultVendor();
            CacheService.Set(CacheKeySetting.Vendor, vendor);
            return PartialView("../VendorInfo/_VendorOnFooter", vendor);
        }

        [HttpGet]
        public ActionResult GetVendorOnFixBottom()
        {
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor != null) return PartialView("../VendorInfo/_VendorOnFixBottom", vendor);

            vendor = VendorService.GetDefaultVendor();
            CacheService.Set(CacheKeySetting.Vendor, vendor);
            return PartialView("../VendorInfo/_VendorOnFixBottom", vendor);
        }

        public ActionResult GetVendorDetail(int vendorId)
        {
            var item = VendorService.GetVendorDetail(vendorId);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int vendorId)
        {
            var item = VendorService.GetVendorDetail(vendorId);

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
                Description = item.Description,
                IsAuthorized = item.IsAuthorized
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
            if (vendorAddress != null)
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

            return PartialView("../VendorInfo/_VendorDetail", model);
        }

        public ActionResult GetVendorInfoOnFooter()
        {
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor != null) return PartialView("../VendorInfo/_VendorInfoOnFooter", vendor);

            vendor = VendorService.GetDefaultVendor();
            CacheService.Set(CacheKeySetting.Vendor, vendor);
            return PartialView("../VendorInfo/_VendorInfoOnFooter", vendor);
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
                    ContactService = null;
                    DocumentService = null;
                    VendorService = null;
                    CacheService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}