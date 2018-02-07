using System.Web.Mvc;
using Eagle.Services;
using Eagle.Services.SystemManagement;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class ContactController : BaseController
    {
        private IContactService ContactService{ get; set; }

        public ContactController(IContactService contactService) : base(new IBaseService[] { contactService })
        {
            ContactService = contactService;
        }

        // GET: Admin/Contact
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PopulateCountrySelectList(int? selectedValue=null, bool isShowSelectText = true)
        {
            var sources = ContactService.PopulateCountrySelectList(true, selectedValue, isShowSelectText);
            return Json(sources, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PopulateProvinceSelectList(int countryId, int? selectedValue=null, bool isShowSelectText = true)
        {
            var sources = ContactService.PopulateProvinceSelectList(countryId, true, selectedValue, isShowSelectText);
            return Json(sources, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PopulateRegionSelectList(int? provinceId, int? selectedValue=null, bool isShowSelectText = true)
        {
            var sources = ContactService.PopulateRegionSelectList(provinceId, true, selectedValue, isShowSelectText);
            return Json(sources, JsonRequestBehavior.AllowGet);
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
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}