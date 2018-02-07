using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Messaging;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Services.Message
{
    public class MessageTemplateController : BaseController
    {
        private IMessageService MessageService { get; set; }
        public MessageTemplateController(IMessageService messageService) : base(new IBaseService[] { messageService })
        {
            MessageService = messageService;
        }

        //
        // GET: /Admin/MailTemplate/
        public ActionResult Index()
        {
            return View("../Services/Message/MessageTemplate/Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var entry = new MessageTemplateEntry();
            return PartialView("../Services/Message/MessageTemplate/_Create", entry);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = MessageService.GetMessageTemplateDetail(id);
            var model = new MessageTemplateEditEntry
            {
                MessageTypeId = item.MessageTypeId,
                NotificationTypeId = item.NotificationTypeId,
                TemplateId = item.TemplateId,
                TemplateName = item.TemplateName,
                TemplateSubject = item.TemplateSubject,
                TemplateBody = item.TemplateBody,
                TemplateCode = item.TemplateCode,
                Status = item.Status
            };
            return PartialView("../Services/Message/MessageTemplate/_Edit", model);
        }

        // GET: /Admin/MessageTemplate/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.TypeId = MessageService.PopulateMessageTypeSelectList();
            ViewBag.IsActive = MessageService.PopulateMessageTemplateStatus();
            return PartialView("../Services/Message/MessageTemplate/_SearchForm");
        }

        [HttpGet]
        public ActionResult Search(MessageTemplateSearchEntry entry, int? page)
        {
            int recordCount;
            var lst = MessageService.GetMessageTemplates(entry, out recordCount, "TemplateId DESC", page, GlobalSettings.DefaultPageSize);
            return PartialView("../Services/Message/MessageTemplate/_SearchResult", lst);
        }

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/MessageTemplate/Create
        [HttpPost]
        public ActionResult Create(MessageTemplateEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MessageService.InsertMessageTemplate(entry);
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


        // PUT - /Admin/MessageTemplate/Edit
        [HttpPut]
        public ActionResult Edit(MessageTemplateEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MessageService.UpdateMessageTemplate(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.TemplateId
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


        // PUT: /Admin/MessageTemplate/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                MessageService.UpdateMessageTemplateStatus(id, status);
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
                    MessageService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
