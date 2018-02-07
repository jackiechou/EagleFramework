using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Messaging;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Services.Message
{
    public class NotificationTypeController : BaseController
    {
        private INotificationService NotificationService { get; set; }
        private IMessageService MessageService { get; set; }
        public NotificationTypeController(INotificationService notificationService, IMessageService messageService)
            : base(new IBaseService[] { notificationService, messageService })
        {
            MessageService = messageService;
            NotificationService = notificationService;
        }

        //
        // GET: /Admin/NotificationType/
        public ActionResult Index(int? mode)
        {
            return View("../Services/Message/NotificationType/Index");
        }

        // Get Hierachical List 
        [HttpGet]
        public ActionResult GetNotificationTypeSelectTree(int? selectedId = null, bool? isRootShowed = false)
        {
            var list = NotificationService.GetNotificationTypeSelectTree(null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: Message Type 
        [HttpGet]
        public ActionResult PopulateMessageTypeSelectList(int? selectedId = null, bool? isShowSelectText = false)
        {
            var list = MessageService.PopulateMessageTypeSelectList(selectedId, isShowSelectText);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNotificationTypeTreeGrid()
        {
            var list = NotificationService.GetNotificationTypeTreeGrid(null, null, null);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Admin/NotificationType/Create
        public ActionResult Create()
        {
            var model = new NotificationTypeEntry
            {
                ParentId = 0,
                Status = NotificationTypeStatus.Active,
                //MessageTypes = MessageService.PopulateMessageTypeMultiSelectList(true)
            };
            ViewBag.MessageTypeIds = MessageService.PopulateMessageTypeMultiSelectList(true);
            ViewData["AvailableNotificationTargetTypes"] = NotificationService.PopulateAvailableNotificationTargetTypes();
            return PartialView("../Services/Message/NotificationType/_Create", model);
        }

        //
        // GET: /Admin/NotificationType/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = NotificationService.GetNotificationTypeDetail(id);
            var selectedMessageTypes = NotificationService.GetNotificationMessageTypes(item.NotificationTypeId).ToList();
            var messageTypeIds = selectedMessageTypes.Select(x => x.MessageTypeId).ToArray();

            var entry = new NotificationTypeEditEntry
            {
                NotificationSenderTypeId = item.NotificationSenderTypeId,
                NotificationTypeId = item.NotificationTypeId,
                NotificationTypeName = item.NotificationTypeName,
                ParentId = item.ParentId,
                Description = item.Description,
                Status = item.Status,
                MessageTypeIds = messageTypeIds,
            };
            ViewBag.MessageTypes = MessageService.PopulateMessageTypeMultiSelectList(null, messageTypeIds);
            ViewData["AvailableNotificationTargetTypes"] = NotificationService.PopulateAvailableNotificationTargetTypes(item.NotificationTypeId);
            ViewData["SelectedNotificationTargetTypes"] = NotificationService.PopulateSelectedNotificationTargetTypes(item.NotificationTypeId);
            return PartialView("../Services/Message/NotificationType/_Edit", entry);
        }

        //
        // POST: /Admin/NotificationType/Create
        [HttpPost]
        public ActionResult Create(NotificationTypeEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NotificationService.InsertNotificationType(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new { Message = LanguageResource.CreateSuccess }
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
        // PUT: /Admin/NotificationType/Edit/5
        [HttpPut]
        public ActionResult Edit(NotificationTypeEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NotificationService.UpdateNotificationType(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.NotificationTypeId
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

        [HttpPost]
        public ActionResult UpdateStatus(int id, NotificationTypeStatus status)
        {
            try
            {
                NotificationService.UpdateNotificationTypeStatus(id, status);
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new { Message = LanguageResource.UpdateStatusSuccess, Id = id }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Admin/NotificationType/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                NotificationService.UpdateNotificationTypeStatus(id, NotificationTypeStatus.InActive);
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new { Message = LanguageResource.DeleteSuccess, Id = id }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //////
        ////// PUT: /Admin/NotificationType/UpdateListOrder
        //[HttpPut]
        //public ActionResult UpdateNotificationTypeListOrder(GalleryTopicListOrderEntry listOrderEntry)
        //{
        //    bool flag = false;
        //    string message = string.Empty;
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            GalleryService.UpdateGalleryTopicListOrders(listOrderEntry);
        //            flag = true;
        //            message = LanguageResource.UpdateStatusSuccess;
        //        }
        //        else
        //        {
        //            var errors = ModelState.Values.SelectMany(v => v.Errors);
        //            message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "<br/>"));
        //        }
        //    }
        //    catch (ValidationError ex)
        //    {
        //        message = ValidationExtension.ConvertValidateErrorToString(ex);
        //    }
        //    return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        //}


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
                    NotificationService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
