using System.Web.Mvc;
using Eagle.Services;
using Eagle.Services.Messaging;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Eagle.WebApp.Attributes.Session;

namespace Eagle.WebApp.Areas.Admin.Controllers.Services.Message
{
    public class MessageManagerController : BaseController
    {
        private IMailService MailService { get; set; }
        public MessageManagerController(IMailService mailService) : base(new IBaseService[] { mailService })
        {
            MailService = mailService;
        }
        //
        // GET: /Admin/MessageManager/
        [SessionExpiration]
        public ActionResult Index()
        {
            return View("../Services/Message/MessageManager/Index");
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
                    MailService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
