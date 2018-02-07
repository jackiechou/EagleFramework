using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.Feedback
{
    public class FeedbackController : BaseController
    {
        private INewsService NewsService { get; set; }

        public FeedbackController(INewsService newsService) : base(new IBaseService[] { newsService })
        {
            NewsService = newsService;
        }

        // GET: Admin/Feedback
        public ActionResult Index()
        {
            return View("../Contents/Feedback/Index");
        }

        // GET: /Admin/Feedback/List
        [HttpGet]
        public ActionResult List(int? page = 1)
        {
            int? recordCount = 0;
            var sources = NewsService.GetFeedbacks(null, ref recordCount, "FeedbackId DESC" , page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Contents/Feedback/_List", lst);
        }

        //
        // PUT: /Admin/Feedback/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                NewsService.UpdateFeedbackStatus(id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
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
                    NewsService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}