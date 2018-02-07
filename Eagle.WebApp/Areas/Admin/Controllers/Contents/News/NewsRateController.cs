using System.Web.Mvc;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Articles;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.News
{
    public class NewsRateController : BaseController
    {
        private INewsService NewsService { get; set; }

        public NewsRateController(INewsService newsService) : base(new IBaseService[] { newsService })
        {
            NewsService = newsService;
        }

        [HttpPost]
        public ActionResult Create(NewsRatingEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var averageRates = NewsService.InsertNewsRating(entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data =
                    {
                        AverageRates = averageRates,
                        Message = LanguageResource.CreateSuccess
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