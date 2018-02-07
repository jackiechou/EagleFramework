using System.Web.Mvc;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class ReportController : BaseController
    {
        private IReportService ReportService { get; set; }
        public ReportController(IReportService reportService) : base(new IBaseService[] { reportService })
        {
            ReportService = reportService;
        }
        // GET: Admin/Report
        public ActionResult Index()
        {
            return View();
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
                    ReportService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}