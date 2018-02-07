using System.Web.Mvc;
using Eagle.Services;
using Eagle.Services.SystemManagement;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys.FileStorage
{
    public class DocumentFileController : BaseController
    {
        private IDocumentService DocumentService { get; }

        public DocumentFileController(IDocumentService documentService) : base(new IBaseService[] { documentService })
        {
            DocumentService = documentService;
        }
        // GET: Admin/DocumentFile
        public ActionResult Index()
        {
            return View();
        }
    }
}