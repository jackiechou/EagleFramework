using System.IO;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Services;
using Eagle.Services.Dtos.SystemManagement.FileStorage;
using Eagle.Services.SystemManagement;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys.FileStorage
{
    public class FileBrowserController : BaseController
    {
        private IDocumentService DocumentService { get; }
        public FileBrowserController(IDocumentService documentService) : base(new IBaseService[] { documentService })
        {
            DocumentService = documentService;
        }
        public JsonResult Read(string path)
        {
            try
            {
                var applicationId = GlobalSettings.DefaultApplicationId;
                var result = DocumentService.GetFileBrowser(applicationId, "~/" + path);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (DirectoryNotFoundException)
            {
                throw new HttpException(404, "File Not Found");
            }
        }

        [OutputCache(Duration = 3600, VaryByParam = "path")]
        public virtual ActionResult Thumbnail(string path)
        {
            return CreateThumbnail("~/" + path);
        }

        private FileContentResult CreateThumbnail(string relativelPath)
        {
            if (string.IsNullOrEmpty(relativelPath)) throw new DirectoryNotFoundException();
            var physicalPath = Server.MapPath(relativelPath);
            if (!System.IO.File.Exists(physicalPath))
            {
                throw new HttpException(404, "File Not Found");
            }
            Response.AddFileDependency(physicalPath);
            var fileInfo = new FileInfo(physicalPath);
            var thumbImage = DocumentService.CreateThumbnail(relativelPath);
            return File(thumbImage, MimeMapping.GetMimeMapping(fileInfo.Name));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Destroy(string path, string name, string type)
        {
            DocumentService.Destroy("~/" + path, name, type);
            return Json(null);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult CreateDirectory(string path, FileBrowserEntry entry)
        {
            DocumentService.CreateDirectory("~/" + path, entry);
            return Json(null);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Upload(string path, HttpPostedFileBase file)
        {
            string fileName = DocumentService.Upload("~/" + path, file);
            return Json(new
            {
                size = file.ContentLength,
                name = fileName,
                type = "f"
            }, "text/plain");
        }

        [OutputCache(Duration = 360, VaryByParam = "path")]
        public ActionResult File(string path)
        {
            var virtualPath = "~/" + path;
            var stream = DocumentService.OpenReadFile(virtualPath);
            var fileInfo = new FileInfo(Server.MapPath(virtualPath));
            string contentType = MimeMapping.GetMimeMapping(fileInfo.Name);
            return File(stream, contentType);
        }
    }
}