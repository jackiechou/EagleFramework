using System;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Articles;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Eagle.WebApp.Attributes.Session;


namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.News
{
    public class NewsController : BaseController
    {
        private ICommonService CommonService { get; set; }
        private INewsService NewsService { get; set; }

        public NewsController(ICommonService commonService, INewsService newsService) : base(new IBaseService[] { commonService, newsService })
        {
            CommonService = commonService;
            NewsService = newsService;
        }
        //
        // GET: /Admin/News/
        public ActionResult Index()
        {
            ViewBag.Status = CommonService.GenerateThreeStatusModeList(LanguageResource.All, true);
            return View("../Contents/News/Index");
        }

        [HttpGet]
        public ActionResult Search(NewsSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["SearchNewsRequest"] = Newtonsoft.Json.JsonConvert.SerializeObject(filter);
            }
            else
            {
                if (TempData["SearchNewsRequest"] != null)
                {
                    filter = (NewsSearchEntry)Newtonsoft.Json.JsonConvert.DeserializeObject(TempData["SearchNewsRequest"] as string, typeof(NewsSearchEntry));
                }
            }
            TempData.Keep();

            int recordCount;
            var sources = NewsService.Search(filter, out recordCount, "NewsId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            if (sources != null)
            {
                var pageLst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
                return PartialView("../Contents/News/_SearchResult", new NewsSearchResult(filter, pageLst));
            }
            else
            {
                return PartialView("../Contents/News/_SearchResult", new NewsSearchResult(filter, null));
            }
        }

        //[System.Web.Mvc.HttpGet]
        //public ActionResult SearchPaging([FromUri]int? page = 1)
        //{
        //    int recordCount;
        //    var filter = (TempData["SearchRequest"]==null)? new NewsSearchEntry() : (NewsSearchEntry)Newtonsoft.Json.JsonConvert.DeserializeObject(TempData["SearchRequest"] as string, typeof(NewsSearchEntry));
        //    TempData.Keep();
        //    var sources = NewsService.Search(filter, out recordCount, "NewsId DESC", page, GlobalSettings.DefaultPageSize);
        //    int currentPageIndex = page - 1 ?? 0;
        //    var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
        //    return PartialView("../Contents/News/_SearchResultDetail", lst);
        //}

        //
        // GET: /Admin/News/Create
        [HttpGet]
        public ActionResult Create()
        {
            var newsEntry = new NewsEntry
            {
                PostedDate = DateTime.UtcNow
            };

            ViewBag.UploadNewsImageFolder = GlobalSettings.NewsUploadImagePath;
            ViewBag.UploadNewsFileFolder = GlobalSettings.NewsUploadDocumentPath;
            ViewBag.CategoryId = NewsService.PopulateHierachicalNewsCategoryDropDownList(LanguageCode);
            return PartialView("../Contents/News/_Create", newsEntry);
        }

        //
        // GET: /Admin/News/Details/5        
        [HttpGet]
        [SessionExpiration]
        public ActionResult Edit(int id)
        {
            var item = NewsService.GeNewsDetail(id);
            var detail = new NewsEditEntry
            {
                NewsId = item.NewsId,
                CategoryId = item.CategoryId,
                Title = item.Title,
                Headline = item.Headline,
                Summary = item.Summary,
                Authors = item.Authors,
                NavigateUrl = item.NavigateUrl,
                FrontImage = item.FrontImage,
                MainImage = item.MainImage,
                MainText = item.MainText,
                Source = item.Source,
                PostedDate = item.PostedDate,
                DocumentInfos = item.DocumentInfos
            };

            ViewBag.UploadNewsImageFolder = GlobalSettings.NewsUploadImagePath;
            ViewBag.UploadNewsFileFolder = GlobalSettings.NewsUploadDocumentPath;
            ViewBag.Contents = item.MainText;
            ViewBag.Status = CommonService.GenerateThreeStatusModeList(item.Status.ToString(), null);
            return View("../Contents/News/_Edit", detail);
        }


        //
        // POST: /Admin/News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NewsService.InsertNews(ApplicationId, UserId, VendorId, entry);
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
        //// POST: /Admin/News/Edit/5
        [HttpPost]
        //[IgnoreModelErrors("PostedDate")]
        public ActionResult Edit(NewsEditEntry entry)
        {
            try
            {
                ModelState.Remove("PostedDate");
                if (ModelState.IsValid)
                {
                    NewsService.UpdateNews(ApplicationId, UserId, entry.NewsId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.NewsId
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
        //// PUT: /Admin/NewsCategory/UpdateListOrder
        [HttpPut]
        public ActionResult UpdateNewsListOrder(NewsListOrderEntry listOrderEntry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NewsService.UpdateNewsListOrders(UserId, listOrderEntry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess
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
        // POST: /Admin/News/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, NewsStatus status)
        {
            try
            {
                NewsService.UpdateNewsStatus(UserId, id, status);
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
        
        //
        // POST: /Admin/News/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                NewsService.DeleteNews(id);
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
