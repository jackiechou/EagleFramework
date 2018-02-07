using System;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Controllers
{
    public class CompanyInfoController : BasicController
    {
        private ICompanyService CompanyService { get; set; }
        private IContactService ContactService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public CompanyInfoController(ICompanyService companyService, IContactService contactService, IDocumentService documentService)
        {
            CompanyService = companyService;
            ContactService = contactService;
            DocumentService = documentService;
        }

        //
        // GET: /DesktopCompany/
        public ActionResult Index()
        {
            var item = GetCompanyInfo();
            return View("../Company/Index", item);
        }

        public ActionResult GetCompanyOnHeader()
        {
            var item = GetCompanyInfo();
            return PartialView("../Company/_CompanyOnHeader", item);
        }
        public ActionResult GetCompanyOnFooter()
        {
            var item = GetCompanyInfo();
            return PartialView("../Company/_CompanyOnHeader", item);
        }

        public ActionResult GetCompanyDetail()
        {
            var item = CompanyService.GetCompanyDetail(GlobalSettings.DefaultCompanyId);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private CompanyInfoDetail GetCompanyInfo()
        {
            var model = new CompanyInfoDetail();
            var item = CompanyService.GetCompanyDetail(GlobalSettings.DefaultFrontCompanyId);
            if (item != null)
            {
                model.CompanyId = item.CompanyId;
                model.ParentId = item.ParentId;
                model.Depth = item.Depth;
                model.Lineage = item.Lineage;
                model.HasChild = item.HasChild;
                model.CompanyName = item.CompanyName;
                model.Slogan = item.Slogan;
                model.Fax = item.Fax;
                model.Hotline = item.Hotline;
                model.Mobile = item.Mobile;
                model.Telephone = item.Telephone;
                model.Email = item.Email;
                model.Website = item.Website;
                model.SupportOnline = item.SupportOnline;
                model.CopyRight = item.CopyRight;
                model.TaxCode = item.TaxCode;
                model.Description = HttpUtility.HtmlDecode(item.Description);
                model.Logo = item.Logo;
                model.Status = item.Status;
                model.AddressId = item.AddressId;

                if (item.Logo != null)
                {
                    var fileInfo = DocumentService.GetFileInfoDetail((int) item.Logo);
                    if (fileInfo != null)
                    {
                        model.FileUrl = fileInfo.FileUrl;
                    }
                }

                if (item.AddressId != null)
                {
                    var address = ContactService.GetAddressDetails(Convert.ToInt32(item.AddressId));
                    if (address != null)
                    {
                        model.Address = new AddressInfoDetail
                        {
                            AddressTypeId = address.AddressTypeId,
                            Street = address.Street,
                            PostalCode = address.PostalCode,
                            Description = address.Description,
                            CountryId = address.CountryId,
                            ProvinceId = address.ProvinceId,
                            RegionId = address.RegionId
                        };

                        if (address.CountryId != null)
                        {
                            model.Address.Country = ContactService.GetCountryDetails(Convert.ToInt32(address.CountryId));
                        }

                        if (address.ProvinceId != null)
                        {
                            model.Address.Province =
                                ContactService.GetProvinceDetails(Convert.ToInt32(address.ProvinceId));
                        }

                        if (address.RegionId != null)
                        {
                            model.Address.Region = ContactService.GetRegionDetails(Convert.ToInt32(address.RegionId));
                        }
                    }
                }
            }
            return model;
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
                    CompanyService = null;
                    ContactService = null;
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
