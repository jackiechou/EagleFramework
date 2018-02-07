using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Feedbacks;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.WebApp.Controllers
{
    public class ContactUsController : BasicController
    {
        private IContactService ContactService { get; set; }
        private IDocumentService DocumentService { get; set; }
        private ICompanyService CompanyService { get; set; }
        private INewsService NewsService { get; set; }
        private IVendorService VendorService { get; set; }
        private ICacheService CacheService { get; set; }

        public ContactUsController(ICompanyService companyService, INewsService newsService, IVendorService vendorService, 
            IContactService contactService, IDocumentService documentService, ICacheService cacheService)
        {
            CompanyService = companyService;
            NewsService = newsService;
            VendorService = vendorService;
            ContactService = contactService;
            DocumentService = documentService;
            CacheService = cacheService;
        }

        //
        // GET: /Contact/
        public ActionResult Index()
        {
            return View("../Contact/Index");
        }

        public ActionResult GetContactInfo()
        {
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor != null) return PartialView("../Contact/_ContactInfo", vendor);

            vendor = VendorService.GetDefaultVendor();
            CacheService.Set(CacheKeySetting.Vendor, vendor);
            return PartialView("../Contact/_ContactInfo", vendor);
        }

        public ActionResult GetCompanyDetail()
        {
            var item = CompanyService.GetCompanyDetail(GlobalSettings.DefaultCompanyId);
            return Json(item, JsonRequestBehavior.AllowGet);
        }
    
        // POST: /Admin/Feedback/Create
        [HttpPost]
        public ActionResult CreateFeedback(FeedbackEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var applicationId = GlobalSettings.DefaultApplicationId;
                    NewsService.InsertFeedback(applicationId, entry);
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
                    CompanyService = null;
                    NewsService = null;
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
