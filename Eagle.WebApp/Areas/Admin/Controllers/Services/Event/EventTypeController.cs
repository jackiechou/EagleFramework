using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Event;
using Eagle.Services.Service;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Services.Event
{
    public class EventTypeController : BaseController
    {
        private IEventService EventService { get; set; }

        public EventTypeController(IEventService eventService) : base(new IBaseService[] { eventService })
        {
            EventService = eventService;
        }

        // GET: Admin/EventType
        public ActionResult Index()
        {
            return View("../Services/Events/EventType/Index");
        }

        // GET: Hierachical List 
        [HttpGet]
        public ActionResult GetEventTypeSelectTree(int? selectedId = null, bool? isRootShowed = true)
        {
            var list = EventService.GetEventTypeSelectTree(null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Admin/EventType/Create       
        [HttpGet]
        public ActionResult Create()
        {
            //ViewBag.TypeId = EventService.PoplulateEventTypeSelectList(null, true, MediaTypeStatus.Active);
            return PartialView("../Services/Events/EventType/_Create");
        }
        //
        // GET: /Admin/EventType/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = EventService.GetEventTypeDetail(id);

            var entry = new EventTypeEditEntry
            {
                TypeId = item.TypeId,
                TypeName = item.TypeName,
                ParentId = item.ParentId,
                Status = item.Status
            };
            return PartialView("../Services/Events/EventType/_Edit", entry);
        }


        //
        // POST: /Admin/EventType/Create-
        [HttpPost]
        public ActionResult Create(EventTypeEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EventService.InsertEventType(UserId, entry);
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

        ////
        //// PUT: /Admin/EventType/Edit/5
        [HttpPut]
        public ActionResult Edit(EventTypeEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EventService.UpdateEventType(UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.TypeId
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

        ////
        //// PUT: /Admin/EventType/UpdateListOrder
        [HttpPut]
        public ActionResult UpdateEventTypeListOrder(int id, int listOrder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EventService.UpdateEventTypeListOrder(UserId, id, listOrder);
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

        // POST: /Admin/EventType/UpdateStatus/5
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                EventService.UpdateEventTypeStatus(UserId, id, status ? EventTypeStatus.Active : EventTypeStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Admin/EventType/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                EventService.UpdateEventTypeStatus(UserId, id, EventTypeStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);

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
                    EventService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}