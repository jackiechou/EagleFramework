using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Services.Event;
using Eagle.Services.Service;

namespace Eagle.WebApp.Controllers
{
    public class EventInfoController : BasicController
    {
        private IEventService EventService { get; set; }

        public EventInfoController(IEventService eventService)
        {
            EventService = eventService;
        }

        //
        // GET: /Event/Detail/5        
        [HttpGet]
        public ActionResult Details(int id)
        {
            ViewBag.EventId = id;
            return View("../Event/Detail");
        }

        [HttpGet]
        public ActionResult GetDetails(int id)
        {
            var item = EventService.GeEventDetail(id);
            return PartialView("../Event/_Detail", item);
        }

        [HttpGet]
        public ActionResult GetLatestEvents(EventSearchEntry filter, int? page = 1, int? pageSize = 10)
        {
            int recordCount;
            var sources = EventService.Search(filter, out recordCount, "EventId DESC", page, pageSize??GlobalSettings.DefaultPageSizeEvent);
            int currentPageIndex = page - 1 ?? 0;
            var pageLst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSizeEvent, recordCount);
            return PartialView("../Event/_LastestEvent", pageLst);
        }
        [HttpGet]
        public ActionResult GetLatestEventsRight(int? typeId=null, EventStatus? status=null, int? page = 1, int? pageSize = 10)
        {
            int recordCount;
            var sources = EventService.GetEvents(typeId, status, out recordCount, "EventId DESC", page, pageSize ?? GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var pageLst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Event/_LastestEventRight", pageLst);
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