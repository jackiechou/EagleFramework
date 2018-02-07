using System;
using System.Web.Mvc;
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
    public class CompanyController : BaseController
    {
        private IContactService ContactService { get; set; }
        private ICompanyService CompanyService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public CompanyController(ICompanyService companyService, IContactService contactService, IDocumentService documentService)
            : base(new IBaseService[] { companyService, contactService, documentService })
        {
            CompanyService = companyService;
            ContactService = contactService;
            DocumentService = documentService;
        }

        #region GET METHODS =======================================================

        // GET: Admin/Company
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Company/Index");
        }

        // GET: /Admin/Company/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new CompanyEditEntry();
            var item = CompanyService.GetCompanyDetail(id);
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
                model.Description = item.Description;
                model.Logo = item.Logo;
                model.Status = item.Status;
                model.AddressId = item.AddressId;

                if (item.Logo != null)
                {
                    var fileInfo = DocumentService.GetFileInfoDetail((int)item.Logo);
                    if (fileInfo != null)
                    {
                        model.FileUrl = fileInfo.FileUrl;
                    }
                }

                if (item.AddressId != null)
                {
                    var address = ContactService.GetAddressDetails(Convert.ToInt32(item.AddressId));
                    model.Address = new CompanyAddressEditEntry
                    {
                        AddressId = address.AddressId,
                        AddressTypeId = address.AddressTypeId,
                        Street = address.Street,
                        PostalCode = address.PostalCode,
                        Description = address.Description,
                        CountryId = address.CountryId,
                        ProvinceId = address.ProvinceId,
                        RegionId = address.RegionId,
                    };
                }
            }

            return PartialView("../Business/Ecommerce/Company/_Edit", model);
        }
     
        // Get Hierachical List 
        [HttpGet]
        public ActionResult GetCompanyTree(int? selectedId=null, bool? isRootShowed=true)
        {
            var list = CompanyService.GetCompanyTreeGrid(null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion ==============================================================================
        
        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/Company/Create
        [HttpPost]
        //[HttpPost, ValidateInput(false)]
        public ActionResult Create(CompanyEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyService.InsertCompany(ApplicationId, UserId, entry);
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
        // PUT - /Admin/Company/Edit
        [HttpPut]
        public ActionResult Edit(CompanyEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyService.UpdateCompany(ApplicationId, UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.CompanyId
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
        // POST: /Admin/Company/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, CompanyStatus status)
        {
            try
            {
                CompanyService.UpdateCompanyStatus(UserId, id, status);
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
                    ContactService = null;
                    CompanyService = null;
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}