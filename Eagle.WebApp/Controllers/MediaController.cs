using System;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Contents.Media;

namespace Eagle.WebApp.Controllers
{
    public class MediaController : BasicController
    {
        private IMediaService MediaService { get; set; }

        public MediaController(IMediaService mediaService)
        {
            MediaService = mediaService;
        }

        // GET: Media
        public ActionResult Index()
        {
            return View("../Media/Index");
        }

        // GET: /Admin/Media/GetLatestVideos
        [HttpGet]
        public ActionResult GetLatestVideos()
        {
            var filter = new MediaFileSearchEntry
            {
                SearchText =null,
                SearchTypeId = Convert.ToInt32(MediaTypeSetting.Video),
                SearchTopicId =null,
                SearchStatus = DocumentFileStatus.Published
            };
            int? recordCount = 0;
            var sources = MediaService.GetMediaFiles(filter, ref recordCount, null, 1, GlobalSettings.DefaultPageSize);
            return PartialView("../Media/_LatestVideos", sources);
        }

        // GET: /Admin/Media/GetVideoAlbums
        [HttpGet]
        public ActionResult GetVideoAlbums()
        {
            int typeId = Convert.ToInt32(MediaTypeSetting.Video);
            var lst = MediaService.GetAlbums(typeId,null, MediaAlbumStatus.Active);
            return PartialView("../Media/_VideoAlbums", lst);
        }

        // GET: /Admin/Media/GetVideoAlbums
        [HttpGet]
        public ActionResult GetVideoPlayLists()
        {
            int typeId = Convert.ToInt32(MediaTypeSetting.Video);
            var lst = MediaService.GetPlayLists(typeId, null, MediaPlayListStatus.Active);
            return PartialView("../Media/_VideoPlayLists", lst);
        }

        // GET: /Admin/Media/GetMediaFilesByAlbum
        [HttpGet]
        public ActionResult GetMediaFilesByAlbum(int albumId)
        {
            var lst = MediaService.GetMediaFilesByAlbumId(albumId, MediaAlbumFileStatus.Active);
            return PartialView("../Media/_VideoAlbumFiles", lst);
        }

        // GET: /Admin/Media/GetMediaFilesByPlayList
        [HttpGet]
        public ActionResult GetMediaFilesByPlayList(int playListId)
        {
            var lst = MediaService.GetMediaFilesByPlayListId(playListId, MediaPlayListFileStatus.Active);
            return PartialView("../Media/_VideoPlayListFiles", lst);
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
                    MediaService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}