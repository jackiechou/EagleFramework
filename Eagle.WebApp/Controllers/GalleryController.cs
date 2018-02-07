using System;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Services.Contents;

namespace Eagle.WebApp.Controllers
{
    public class GalleryController : BasicController
    {
        private IGalleryService GalleryService { get; set; }

        public GalleryController(IGalleryService galleryService)
        {
            GalleryService = galleryService;
        }

        // GET: Gallery
        public ActionResult Index()
        {
            return View("../Gallery/Index");
        }

        // GET: Gallery
        public ActionResult GetGalleryCollections(int? topicId = 1)
        {
            var sources = GalleryService.GetGalleryCollections(topicId, GalleryCollectionStatus.Active);
            return PartialView("../Gallery/_GalleryCollections", sources);
        }

        public ActionResult GetTopGalleryFiles()
        {
            int recordCount;
            var data = GalleryService.GetGalleryFiles(null, GalleryFileStatus.Active,out recordCount, "ListOrder ASC",1, GlobalSettings.DefaultPageSize).ToList();
            if (!data.Any()) return null;

            var result = (from item in data
                          let fullUrl = item.File.FileUrl
                          let groupId = item.CollectionId
                          let fileTitle = item.File.FileTitle
                          select new
                          {
                              content = "<div class=\'slide_inner\'>" +
                                        "<a class=\'galleryItem photo_link\' data-group='" + item.CollectionId + "' href='" + fullUrl + "'><img class=\'photo\' src=\'" + fullUrl + "' alt='" + fileTitle + "'></a>" +
                                        "<a class='caption' data-group='" + groupId + "' href='" + fullUrl + "'>" + fileTitle + "</a>" +
                                        "</div>",
                              content_button = "<div class='thumb'><img src='" + fullUrl + "' alt='" + fileTitle + "'></div><p>" + fileTitle + "</p>"
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGalleryFilesFromLatestCollection()
        {
            var data = GalleryService.GetGalleryFilesFromLatestCollection(GalleryFileStatus.Active).ToList();
            if (!data.Any()) return null;

            var result = (from item in data
                          let fullUrl = item.File.FileUrl
                          let groupId = item.CollectionId
                          let fileTitle = item.File.FileTitle
                          select new 
                          {
                              content = "<div class=\'slide_inner\'>" +
                                        "<a class=\'galleryItem photo_link\' data-group='" + item.CollectionId + "' href='" + fullUrl + "'><img class=\'photo\' src=\'" + fullUrl + "' alt='" + fileTitle + "'></a>" +
                                        "<a class='caption' data-group='" + groupId + "' href='" + fullUrl + "'>" + fileTitle + "</a>" +
                                        "</div>",
                              content_button = "<div class='thumb'><img src='" + fullUrl + "' alt='" + fileTitle + "'></div><p>" + fileTitle + "</p>"
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGalleryFiles(int collectionId)
        {
            var sources = GalleryService.GetGalleryFiles(Convert.ToInt32(collectionId), GalleryFileStatus.Active);
            return PartialView("../Gallery/_GalleryList", sources);
        }


        public ActionResult PopulateLatestCollectionSlider()
        {
            var files = GalleryService.GetGalleryFilesFromLatestCollection(GalleryFileStatus.Active);
            return PartialView("../Gallery/_GalleryList", files);
        }

        public ActionResult PopulateLatestGallerySlider()
        {
            int recordCount;
            var files = GalleryService.GetGalleryFiles(null, GalleryFileStatus.Active, out recordCount, "ListOrder ASC", 1, GlobalSettings.DefaultPageSize).ToList();
            return PartialView("../Gallery/_GalleryList", files);
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
                    GalleryService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}