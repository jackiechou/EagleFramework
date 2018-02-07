using System.Web.Mvc;
using Eagle.Services;
using Eagle.Services.SystemManagement;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class ResourceController : BaseController
    {
        private IResourceService ResourceService { get; set; }

        public ResourceController(IResourceService resourceService) : base(new IBaseService[] { resourceService })
        {
            ResourceService = resourceService;
        }

        #region GET METHODS =========================================================================
        //
        // GET: /Admin/Resource/
        public ActionResult Index()
        {
            return View("../Sys/Resource/Index");
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
                    ResourceService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}