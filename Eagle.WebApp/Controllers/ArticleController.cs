using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Articles;
using Eagle.Services.Validations;

namespace Eagle.WebApp.Controllers
{
    public class ArticleController : BasicController
    {
        private INewsService NewsService { get; set; }

        public ArticleController(INewsService newsService)
        {
            NewsService = newsService;
        }
        // GET: Article
        public ActionResult Index()
        {
            return View("../Article/Index");
        }

        [HttpGet]
        public ActionResult Search(NewsSearchEntry filter, int? page = 1)
        {
            int pageSize = 10;
            int recordCount;
            var sources = NewsService.Search(filter, out recordCount, "NewsId DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var pageLst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Article/_SearchResult", pageLst);
        }

        [HttpGet]
        public ActionResult GetTopNews(int number = 4)
        {
            var lst = NewsService.GetNews(number);
            return PartialView("../Article/_NewsTopThumbnails", lst);
        }

        [HttpGet]
        public ActionResult GetHotNews(int number = 4)
        {
            var lst = NewsService.GetNews(number);
            return PartialView("../Article/_HotNews", lst);
        }

        // GET: /Admin/Media/GetMostViewedNews
        [HttpGet]
        public ActionResult GetMostViewedNews(int number = 4)
        {
            var lst = NewsService.GetListByTotalViews(number);
            return PartialView("../Article/_MostViewedList", lst);
        }


        // GET: /Admin/Media/GetPopularPosts
        [HttpGet]
        public ActionResult GetPopularPosts(int number = 4)
        {
            var lst = NewsService.GetListByTotalViews(number);
            return PartialView("../Article/Tabs/_PopularPosts", lst);
        }

        // GET: /Admin/Media/GetLatestPosts
        [HttpGet]
        public ActionResult GetLatestPosts(int number = 4)
        {
            var lst = NewsService.GetNews(number);
            return PartialView("../Article/Tabs/_LatestPosts", lst);
        }

        // GET: /Admin/Media/GetLatestPosts
        [HttpGet]
        public ActionResult GetLatestComments(int number = 4)
        {
            int? recordCount=0;
            var lst = NewsService.GetNewsComments(NewsCommentStatus.Active, ref recordCount, "CommentId DESC",1, number);
            return PartialView("../Article/Tabs/_LatestComments", lst);
        }

        //
        // GET: /Admin/Article/Details/5        
        [HttpGet]
        public ActionResult Details(int id)
        {
            UpdateTotalViewById(id);
            var item = NewsService.GeNewsDetail(id);
            return View("../Article/NewsDetail", item);
        }
        
        // Get Hierachical List 
        [HttpGet]
        public ActionResult GetNewsCategoryTree(int? selectedId, bool? isRootShowed)
        {
            string languageCode = GlobalSettings.DefaultLanguageCode;
            var list = NewsService.GetNewsCategoryTree(languageCode, NewsCategoryStatus.Published, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetListByCategoryCode(string CategoryCode, int Num)
        //{
        //    var lst = NewsService.GetListByFixedNumCode(CategoryCode, Convert.ToInt32(ThreeStatusString.Published), Num, LanguageCode);
        //    return PartialView("../News/_News",lst);
        //}

        [HttpPut]
        public ActionResult UpdateTotalViewById(int id)
        {
            try
            {
                NewsService.UpdateNewsTotalViews(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateNewTotalViewSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #region RATINGS

        [HttpPost]
        public ActionResult CreateNewsRating(int? targetId, int id, int rate)
        {
            try
            {
                var rateEntry = new NewsRatingEntry
                {
                    TargetId = targetId,
                    NewsId = id,
                    Rate = rate,
                    TotalRates = NewsService.GetDefaultNewsRating(GlobalSettings.DefaultApplicationId)
                };

                var averageRates = NewsService.InsertNewsRating(rateEntry);
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new
                    {
                        AverageRates = averageRates,
                        Message = LanguageResource.CreateSuccess
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        
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