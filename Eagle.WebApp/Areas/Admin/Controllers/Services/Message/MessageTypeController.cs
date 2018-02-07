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
    public class MessageTypeController : BaseController
    {
        private IMessageService MessageService { get; set; }
        public MessageTypeController(IMessageService messageService) : base(new IBaseService[] { messageService })
        {
            MessageService = messageService;
        }

        // GET: Admin/MessageType
        public ActionResult Index()
        {
            return View("../Services/Message/MessageType/Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var entry = new MessageTypeEntry();
            return PartialView("../Services/Message/MessageType/_Create", entry);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = MessageService.GetMessageTypeDetail(id);
            var model = new MessageTypeEditEntry
            {
                MessageTypeId = item.MessageTypeId,
                MessageTypeName = item.MessageTypeName,
                Description = item.Description,
                Status = item.Status
            };
            return PartialView("../Services/Message/MessageType/_Edit", model);
        }

        // GET: /Admin/Language/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.Status = MessageService.PopulateMessageTypeStatus();
            return PartialView("../Services/Message/MessageType/_SearchForm");
        }

        [HttpGet]
        public ActionResult Search(bool? status)
        {
            var lst = MessageService.GetMessageTypes(status);
            return PartialView("../Services/Message/MessageType/_SearchResult", lst);
        }

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/MessageType/Create
        [HttpPost]
        public ActionResult Create(MessageTypeEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MessageService.InsertMessageType(entry);
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


        // PUT - /Admin/MessageType/Edit
        [HttpPut]
        public ActionResult Edit(MessageTypeEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MessageService.UpdateMessageType(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.MessageTypeId
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


        // PUT: /Admin/MessageType/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                MessageService.UpdateMessageTypeStatus(id, status);
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