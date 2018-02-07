using System.Web.Mvc;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Messaging;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Services.Message
{
    public class NotificationSenderController : BaseController
    {
        private IMailService MailService { get; set; }
        private INotificationService NotificationService { get; set; }

        public NotificationSenderController(INotificationService notificationService, IMailService mailService) : base(new IBaseService[] { notificationService, mailService })
        {
            NotificationService = notificationService;
            MailService = mailService;
        }

        // GET: Admin/NotificationSender
        public ActionResult Index()
        {
            return View("../Services/Message/NotificationSender/Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var entry = new NotificationSenderEntry();
            ViewBag.MailServerProviderId = MailService.PopulateMailServerProviderSelectList();
            return PartialView("../Services/Message/NotificationSender/_Create", entry);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = NotificationService.GetNotificationSenderDetail(id);
            var model = new NotificationSenderEditEntry
            {
                NotificationSenderId = item.NotificationSenderId,
                MailServerProviderId = item.MailServerProviderId,
                SenderName = item.SenderName,
                ContactName = item.ContactName,
                MailAddress = item.MailAddress,
                Password = item.Password,
                ConfirmedPassword = item.Password,
                Signature = item.Signature,
                IsActive = item.IsActive
            };
            ViewBag.MailServerProviderId = MailService.PopulateMailServerProviderSelectList(item.MailServerProviderId);
            return PartialView("../Services/Message/NotificationSender/_Edit", model);
        }

        // GET: /Admin/NotificationSender/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.ProviderId = MailService.PopulateMailServerProviderSelectList();
            return PartialView("../Services/Message/NotificationSender/_SearchForm");
        }

        [HttpGet]
        public ActionResult Search(int? providerId)
        {
            var lst = NotificationService.GetNotificationSenders(providerId);
            return PartialView("../Services/Message/NotificationSender/_SearchResult", lst);
        }

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/NotificationSender/Create
        [HttpPost]
        public ActionResult Create(NotificationSenderEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NotificationService.InsertNotificationSender(entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = new { Message = LanguageResource.CreateSuccess } }, JsonRequestBehavior.AllowGet);
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


        // PUT - /Admin/NotificationSender/Edit
        [HttpPut]
        public ActionResult Edit(NotificationSenderEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NotificationService.UpdateNotificationSender(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.NotificationSenderId
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

        // PUT: /Admin/NotificationSender/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                NotificationService.UpdateNotificationSenderStatus(id, status);
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new
                    {
                        Message = LanguageResource.UpdateStatusSuccess,
                        Id = id
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        // PUT: /Admin/NotificationSender/UpdateSelectedNotificationSender/5
        [HttpPut]
        public ActionResult UpdateSelectedNotificationSender(int id)
        {
            try
            {
                NotificationService.UpdateSelectedNotificationSender(id);
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new
                    {
                        Message = LanguageResource.UpdateSuccess,
                        Id = id
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion =====================================================================================

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    MailService = null;
                    NotificationService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
