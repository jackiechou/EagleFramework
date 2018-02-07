using System.Web.Mvc;
using Eagle.Core.Extension;
using Eagle.Core.Pagination;
using Eagle.Core.Search;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Articles;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Eagle.WebApp.Attributes.Session;


namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.News
{
    public class NewsCategoryController : BaseController
    {
        private ICommonService CommonService { get; set; }
        private INewsService NewsService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public NewsCategoryController(ICommonService commonService, IDocumentService documentService, INewsService newsService)
            : base(new IBaseService[] { commonService, documentService, newsService })
        {
            CommonService = commonService;
            DocumentService = documentService;
            NewsService = newsService;
        }
    
        //
        // GET: /Admin/NewsCategory/
        [SessionExpiration]
        public ActionResult Index()
        {
            return View("../Contents/NewsCategory/Index");
        }

        [SessionExpiration]
        public ActionResult List()
        {
            var sources = new SearchDataResult<TreeGrid>()
            {
              PaginatedList  = new PaginatedList()
            };
            return PartialView("../Contents/NewsCategory/_List", sources);
        }

        // Get Hierachical List 
        [HttpGet]
        public ActionResult GetNewsCategoryTree(int? selectedId, bool? isRootShowed)
        {
            var list = NewsService.GetNewsCategoryTree(LanguageCode, null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNewsCategoryTreeGrid(int? page=1, int? pageSize=10)
        {
            int recordCount;
            var list = NewsService.GetNewsCategoryTreeGrid(LanguageCode, null, out recordCount, null, page, pageSize);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PopulateHierachicalCategory()
        {
            var lst = NewsService.PopulateHierachicalNewsCategoryDropDownList(LanguageCode);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Admin/NewsCategory/Create
        [SessionExpiration]
        public ActionResult Create()
        {
            ViewBag.Status = CommonService.GenerateThreeStatusModeList(null, true);
            return PartialView("../Contents/NewsCategory/_Create");
        }

        //
        // GET: /Admin/NewsCategory/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = NewsService.GetNewsCategoryDetail(id);
            ViewBag.Status = CommonService.GenerateThreeStatusModeList(item.Status.ToString(), null);
            //ViewData["ImagePath"] = DocumentService.GetFilePath(item.CategoryImage);

            var entry = new NewsCategoryEditEntry
            {
                CategoryId = item.CategoryId,
                CategoryName = item.CategoryName,
                ParentId = item.ParentId,

                CategoryImage = item.CategoryImage,
                Description = item.Description,
                NavigateUrl = item.NavigateUrl,
                Status = item.Status
            };

            return PartialView("../Contents/NewsCategory/_Edit", entry);
        }


        //
        // POST: /Admin/NewsCategory/Create-
        [HttpPost]
        public ActionResult Create(NewsCategoryEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NewsService.InsertNewsCategory(UserId, LanguageCode, entry);
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
        //// PUT: /Admin/NewsCategory/Edit/5
        [HttpPut]
        public ActionResult Edit(NewsCategoryEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NewsService.UpdateNewsCategory(UserId, LanguageCode, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.CategoryId
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
        public ActionResult UpdateNewsCategoryListOrder(NewsCategoryListOrderEntry listOrderEntry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NewsService.UpdateNewsCategoryListOrders(UserId, listOrderEntry);
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
        // POST: /Admin/NewsCategory/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                NewsService.UpdateNewsCategoryStatus(UserId, id, NewsCategoryStatus.InActive);
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
                    CommonService = null;
                    NewsService = null;
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
