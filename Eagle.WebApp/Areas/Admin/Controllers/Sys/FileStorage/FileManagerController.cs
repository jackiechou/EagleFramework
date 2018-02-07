using System.Web.Mvc;
using Eagle.Services;
using Eagle.Services.Dtos.SystemManagement.FileStorage;
using Eagle.Services.SystemManagement;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys.FileStorage
{
    public class FileManagerController : BaseController
    {
        private IDocumentService DocumentService { get; }

        public FileManagerController(IDocumentService documentService) : base(new IBaseService[] { documentService })
        {
            DocumentService = documentService;
        }

        // GET: Admin/Document
        public ActionResult Index(string subFolder)
        {
            // FileViewModel contains the root MyFolder and the selected subfolder if any
            var model = new FileViewModel() { Folder = "Documents", SubFolder = subFolder };
            return View("../Sys/FileStorage/Index", model);
        }
    }
}