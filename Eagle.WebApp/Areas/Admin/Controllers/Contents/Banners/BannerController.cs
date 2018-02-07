using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Banners;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Eagle.WebApp.Attributes.Session;
using System.Linq;
using System.Web.Mvc;
using Eagle.Services;

namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.Banners
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class BannerController : BaseController
    {
        private IBannerService BannerService { get; set; }
        private IPageService PageService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public BannerController(IBannerService bannerService, IDocumentService documentService, IPageService pageService)
            : base(new IBaseService[] { bannerService, documentService, pageService })
        {
            BannerService = bannerService;
            DocumentService = documentService;
            PageService = pageService;
        }

        #region GET METHODS

        //
        // GET: /Admin/Banner/
        [HttpGet]
        [SessionExpiration]
        public ActionResult Index()
        {
            ViewBag.Title = LanguageResource.BannerManagement;
            if (Request.IsAjaxRequest())
                return PartialView("../Contents/Banners/_Reset");
            else
                return View("../Contents/Banners/Index");
        }

        [HttpGet]
        [SessionExpiration]
        public ActionResult LoadSearchForm()
        {
            ViewBag.PageId = PageService.PopulatePageSelectList(PageType.Site, null, null, true);
            ViewBag.BannerPositionId = BannerService.PoplulateBannerPositionSelectList(true);
            ViewBag.BannerTypeId = BannerService.PopulateBannerTypeSelectList(null, true);
            return PartialView("../Contents/Banners/_SearchForm");
        }

        /// <summary>
        /// List of Banners
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [SessionExpiration]
        public ActionResult Search(BannerSearchEntry entry, int? page = 1)
        {
            if (page == 1)
            {
                if (entry != null)
                {
                    TempData["BannerSearchRequest"] = entry;
                }
            }
            else
            {
                if (TempData["BannerSearchRequest"] != null)
                {
                    entry = (BannerSearchEntry)TempData["BannerSearchRequest"];
                }
            }
            TempData.Keep();

            int recordCount;
            var sources = BannerService.Search(VendorId, LanguageCode, entry, out recordCount, "ListOrder DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Contents/Banners/_SearchResults", lst);
        }

        // GET: /Admin/Banner/Create
        [HttpGet]
        [SessionExpiration]
        public ActionResult Create()
        {
            ViewBag.BannerTypes = BannerService.GetActiveBannerTypes().ToList();
            ViewBag.BannerScopes = BannerService.GetActiveBannerScopes().ToList();
            ViewBag.AvailablePages = PageService.PopulatePageMultiSelectList(PageType.Site, null, null);
            ViewBag.AvailablePositions = BannerService.PoplulateBannerPositionSelectList(false);
            ViewBag.Target = BannerService.PopulateLinkTargets(null);
            return View("../Contents/Banners/Create");
        }

        // GET: /Admin/Banner/Details/5        
        [HttpGet]
        [SessionExpiration]
        public ActionResult Edit(int id)
        {
            var model = new BannerEditEntry();
            var item = BannerService.GetBannerDetails(id);
            if (item != null)
            {
                if (item.FileId != null)
                {
                    var fileInfo = DocumentService.GetFileInfoDetail((int)item.FileId);
                    if (fileInfo != null)
                    {
                        model.FileUrl = fileInfo.FileUrl;
                    }
                }

                model.BannerId = item.BannerId;
                model.TypeId = item.TypeId;
                model.ScopeId = item.ScopeId;
                model.BannerTitle = item.BannerTitle;
                model.Description = item.Description;
                model.BannerContent = item.BannerContent;
                model.AltText = item.AltText;
                model.FileId = item.FileId;
                model.Link = item.Link;
                model.Target = item.Target;
                model.Width = item.Width;
                model.Height = item.Height;
                model.Tags = item.Tags;
                model.ClickThroughs = item.ClickThroughs;
                model.Impressions = item.Impressions;
                model.Advertiser = item.Advertiser;
                model.Status = item.Status;
                model.StartDate = item.StartDate;
                model.EndDate = item.EndDate;
            }

            ViewBag.BannerTypes = BannerService.GetActiveBannerTypes().ToList();
            ViewBag.BannerScopes = BannerService.GetActiveBannerScopes().ToList();
            ViewBag.AvailablePositions = BannerService.PopulateBannerPositionMultiSelectList(id);
            ViewBag.AvailablePages = BannerService.PopulateBannerPageMultiSelectList(id);
            ViewBag.Target = BannerService.PopulateLinkTargets(item.Target);

            return View("../Contents/Banners/Edit", model);
        }

        [HttpGet]
        [SessionExpiration]
        public ActionResult Reset()
        {
            return PartialView("../Contents/Banners/_Reset");
        }

        #endregion

        #region POST - PUT METHODS

        // POST: /Admin/Banner/Create
        [HttpPost]
        public ActionResult Create(BannerEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BannerService.Insert(ApplicationId, UserId, VendorId, LanguageCode, entry);
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

        /// <summary>
        /// PUT - Edit Banner
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        [HttpPut]
        public ActionResult Edit(BannerEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BannerService.Update(ApplicationId, UserId, LanguageCode, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.BannerId
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

        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                BannerService.UpdateBannerStatus(UserId, id, status ? BannerStatus.Active : BannerStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/Banner/Delete/5

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                BannerService.DeleteBanner(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
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
                    BannerService = null;
                    PageService = null;
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
