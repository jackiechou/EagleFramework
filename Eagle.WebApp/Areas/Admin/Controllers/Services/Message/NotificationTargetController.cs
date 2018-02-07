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
    public class NotificationTargetController : BaseController
    {
        private IMailService MailService { get; set; }
        private INotificationService NotificationService { get; set; }

        public NotificationTargetController(INotificationService notificationService, IMailService mailService)
            : base(new IBaseService[] { notificationService, mailService })
        {
            NotificationService = notificationService;
            MailService = mailService;
        }

        // GET: Admin/NotificationTarget
        public ActionResult Index()
        {
            return View("../Services/Message/NotificationTarget/Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var entry = new NotificationTargetDefaultEntry();
            ViewBag.MailServerProviderId = MailService.PopulateMailServerProviderSelectList();
            return PartialView("../Services/Message/NotificationTarget/_Create", entry);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = NotificationService.GetNotificationTargetDefaultDetail(id);
            var model = new NotificationTargetDefaultEditEntry
            {
                NotificationTargetDefaultId = item.NotificationTargetDefaultId,
                MailServerProviderId = item.MailServerProviderId,
                TargetName = item.TargetName,
                ContactName = item.ContactName,
                MailAddress = item.MailAddress,
                Password = item.Password,
                ConfirmedPassword = item.Password,
                IsActive = item.IsActive
            };
            ViewBag.MailServerProviderId = MailService.PopulateMailServerProviderSelectList(item.MailServerProviderId);
            return PartialView("../Services/Message/NotificationTarget/_Edit", model);
        }

        // GET: /Admin/NotificationTarget/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.ProviderId = MailService.PopulateMailServerProviderSelectList();
            return PartialView("../Services/Message/NotificationTarget/_SearchForm");
        }

        [HttpGet]
        public ActionResult Search(int? providerId)
        {
            var lst = NotificationService.GetNotificationTargetDefaults(providerId);
            return PartialView("../Services/Message/NotificationTarget/_SearchResult", lst);
        }

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/NotificationSender/Create
        [HttpPost]
        public ActionResult Create(NotificationTargetDefaultEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NotificationService.InsertNotificationTargetDefault(entry);
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


        // PUT - /Admin/NotificationTarget/Edit
        [HttpPut]
        public ActionResult Edit(NotificationTargetDefaultEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NotificationService.UpdateNotificationTargetDefault(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.NotificationTargetDefaultId
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

        // PUT: /Admin/NotificationTarget/UpdateStatus/5
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

        // PUT: /Admin/NotificationTarget/UpdateSelectedNotificationTargetDefault/5
        [HttpPut]
        public ActionResult UpdateSelectedNotificationTargetDefault(int id)
        {
            try
            {
                NotificationService.UpdateSelectedNotificationTargetDefault(id);
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