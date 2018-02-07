using System;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
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
    public class EventController : BaseController
    {
        private IEventService EventService { get; set; }

        public EventController(IEventService eventService) : base(new IBaseService[] { eventService })
        {
            EventService = eventService;
        }

        // GET: Admin/Event
        public ActionResult Index()
        {
            return View("../Services/Events/Event/Index");
        }

        // GET: /Admin/Event/LoadSearchForm
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Services/Events/Event/_SearchForm");
        }

        [HttpGet]
        public ActionResult Search(EventSearchEntry filter, string sourceEvent, int? page)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["SearchEventRequest"] = filter;
            }
            else
            {
                if (TempData["SearchEventRequest"] != null)
                {
                    filter = (EventSearchEntry)(TempData["SearchEventRequest"]);
                }
            }
            TempData.Keep();

            int recordCount;
            var sources = EventService.Search(filter, out recordCount, "EventId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var pageLst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Services/Events/Event/_SearchResult", pageLst);
        }

      
        // GET: Hierachical List 
        [HttpGet]
        public ActionResult GetEventTypeSelectTree(int? selectedId = null, bool? isRootShowed = true)
        {
            var list = EventService.GetEventTypeSelectTree(null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GenerateCode()
        {
            var code = EventService.GenerateCode(15);
            return Json(code, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Admin/Event/Create
        [HttpGet]
        public ActionResult Create()
        {
            var code = EventService.GenerateCode(15);
            var model = new EventEntry
            {
                EventCode = code,
                TimeZone = GlobalSettings.DefaultTimeZone,
                StartDate = DateTime.UtcNow
            };
            ViewBag.TimeZone = EventService.PoplulateTimeZoneSelectList();
            ViewBag.UploadEventImageFolder = GlobalSettings.EventUploadImagePath;
            ViewBag.UploadEventFileFolder = GlobalSettings.EventUploadDocumentPath;
            return PartialView("../Services/Events/Event/_Create", model);
        }

        //
        // GET: /Admin/Event/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = EventService.GeEventDetail(id);
            var detail = new EventEditEntry
            {
                TypeId = item.TypeId,
                EventId = item.EventId,
                EventCode = item.EventCode,
                EventTitle = item.EventTitle,
                EventMessage = item.EventMessage,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                TimeZone = item.TimeZone,
                Location = item.Location,
                SmallPhoto = item.SmallPhoto,
                LargePhoto = item.LargePhoto,
                IsNotificationUsed = item.IsNotificationUsed,
                Status = item.Status,
                DocumentInfos = item.DocumentInfos
            };
            ViewBag.TimeZone = EventService.PoplulateTimeZoneSelectList(item.TimeZone);
            ViewBag.UploadEventImageFolder = GlobalSettings.EventUploadImagePath;
            ViewBag.UploadEventFileFolder = GlobalSettings.EventUploadDocumentPath;
            ViewBag.Contents = item.EventMessage;
            return View("../Services/Events/Event/Edit", detail);
        }
        
        //
        // POST: /Admin/Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EventService.InsertEvent(ApplicationId, UserId, VendorId, entry);
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
        //// POST: /Admin/Event/Edit/5
        [HttpPut]
        public ActionResult Edit(EventEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EventService.UpdateEvent(ApplicationId, UserId, VendorId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.EventId
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
        // POST: /Admin/Event/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, EventStatus status)
        {
            try
            {
                EventService.UpdateEventStatus(ApplicationId, UserId, VendorId, id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success,
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