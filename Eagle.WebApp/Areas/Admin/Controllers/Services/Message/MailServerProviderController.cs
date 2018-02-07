using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Messaging;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Services.Message
{
    public class MailServerProviderController : BaseController
    {
        private IMailService MailService { get; set; }

        public MailServerProviderController(IMailService mailService) : base(new IBaseService[] { mailService })
        {
            MailService = mailService;
        }

        //
        // GET: /Admin/MailServerProvider/
        [HttpGet]
        public ActionResult Index()
        {
            return View("../Services/Message/MailServerProvider/Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var entry = new MailServerProviderEntry();
            ViewBag.MailServerProtocol = MailService.PopulateMailServerProtocol();
            return PartialView("../Services/Message/MailServerProvider/_Create", entry);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = MailService.GetMailServerProviderDetail(id);
            var model = new MailServerProviderEditEntry
            {
                MailServerProviderId = item.MailServerProviderId,
                MailServerProviderName = item.MailServerProviderName,
                MailServerProtocol = item.MailServerProtocol,
                IncomingMailServerHost = item.IncomingMailServerHost,
                IncomingMailServerPort = item.IncomingMailServerPort,
                OutgoingMailServerHost = item.OutgoingMailServerHost,
                OutgoingMailServerPort = item.OutgoingMailServerPort,
                Ssl = item.Ssl,
                Tls = item.Tls
            };
            ViewBag.MailServerProtocol = MailService.PopulateMailServerProtocol(item.MailServerProtocol);
            return PartialView("../Services/Message/MailServerProvider/_Edit", model);
        }

        [HttpGet]
        public ActionResult List(int? page)
        {
            int recordCount;
            var sources = MailService.GetMailServerProviders(out recordCount, null, page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Services/Message/MailServerProvider/_List", lst);
        }

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/MailServerProvider/Create
        [HttpPost]
        public ActionResult Create(MailServerProviderEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MailService.InsertMailServerProvider(entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = new { Message = LanguageResource.CreateSuccess } }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var modeStateErrors = ModelState.GetModelStateErrors();
                    return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }


        // PUT - /Admin/MailServerProvider/Edit
        [HttpPut]
        public ActionResult Edit(MailServerProviderEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MailService.UpdateMailServerProvider(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.MailServerProviderId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var modeStateErrors = ModelState.GetModelStateErrors();
                    return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }


        // PUT: /Admin/MailServerProvider/UpdateSslStatus/5
        [HttpPut]
        public ActionResult UpdateSslStatus(int id, bool status)
        {
            try
            {
                MailService.UpdateSslStatus(id, status);
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new
                    {
                        Message = LanguageResource.UpdateStatusSuccess,
                        Id = id
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        // PUT: /Admin/MailServerProvider/UpdateSslStatus/5
        [HttpPut]
        public ActionResult UpdateTlsStatus(int id, bool status)
        {
            try
            {
                MailService.UpdateTlsStatus(id, status);
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new
                    {
                        Message = LanguageResource.UpdateStatusSuccess,
                        Id = id
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion =====================================================================================
    
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
