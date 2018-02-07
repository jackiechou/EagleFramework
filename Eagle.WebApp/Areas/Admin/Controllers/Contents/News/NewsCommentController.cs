using System.Web.Mvc;
using Eagle.Core.Pagination;
using Eagle.Core.Search;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Contents.Articles;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Eagle.WebApp.Attributes.Session;

namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.News
{
    public class NewsCommentController : BaseController
    {
        private ICommonService CommonService { get; set; }
        private INewsService NewsService { get; set; }

        public NewsCommentController(ICommonService commonService, INewsService newsService)
            : base(new IBaseService[] { commonService, newsService })
        {
            CommonService = commonService;
            NewsService = newsService;
        }

        //
        // GET: /Admin/News/
        public ActionResult Index()
        {
            ViewBag.Status = CommonService.GenerateThreeStatusModeList(LanguageResource.All, true);
            return View("../Contents/NewsComment/Index");
        }

        [SessionExpiration]
        public ActionResult List(SearchDataRequest<string> request)
        {
            int recordCount;
            var list = NewsService.GetNewsList(request.Filter, null, out recordCount, null, request.PageIndex, request.PageSize);

            var result = new SearchDataResult<NewsInfoDetail>
            {
                List = list,
                PaginatedList = new PaginatedList
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                }
            };

            return PartialView("../Contents/NewsComment/_List", result);
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
                    CommonService = null;
                    NewsService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}