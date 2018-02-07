using System.Web.Mvc;
using Eagle.Services;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class ScheduleController : BaseController
    {
        private ICommonService CommonService { get; set; }
        public ScheduleController(ICommonService commonService) : base(new IBaseService[] { commonService })
        {
            CommonService = commonService;
        }

        // GET: Admin/Schedule
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
                    CommonService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}