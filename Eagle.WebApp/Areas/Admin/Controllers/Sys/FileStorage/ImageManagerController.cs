using System.Web.Mvc;
using Eagle.Services;
using Eagle.Services.Dtos.SystemManagement.FileStorage;
using Eagle.Services.SystemManagement;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys.FileStorage
{
    public class ImageManagerController : BaseController
    {
        private IDocumentService DocumentService { get; }

        public ImageManagerController(IDocumentService documentService) : base(new IBaseService[] { documentService })
        {
            DocumentService = documentService;
        }
        // GET: Admin/ImageManager
        public ActionResult Index(string subFolder)
        {
            // FileViewModel contains the root MyFolder and the selected subfolder if any
            var model = new FileViewModel { Folder = "Images", SubFolder = subFolder };
            return View("../Sys/FileStorage/DocumentImage/Index", model);
        }
    }
}