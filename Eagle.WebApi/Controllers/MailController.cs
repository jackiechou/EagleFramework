using System.Web.Http;
using System.Web.Http.Description;
using Eagle.Services;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Messaging;

namespace Eagle.WebApi.Controllers
{
    /// <summary>
    /// Mail Controller
    /// </summary>
    [RoutePrefix("api/mail")]
    public class MailController : ApiControllerBase
    {
        private IMailService MailService { get; set; }

        /// <summary>
        ///  Initializes a new instance of the <see cref="MailController" /> class.
        /// </summary>
        /// <param name="mailService">The job service.</param>
        public MailController(IMailService mailService): base(new IBaseService[] { mailService })
        {
            MailService = mailService;
        }

        ///// <summary>
        ///// Gets a list of Skills
        ///// </summary>
        //[Route("mailtemplates")]
        //[ResponseType(typeof(MailTemplateDetailListResult))]
        //public IHttpActionResult GetMailTemplates()
        //{
        //    var dtos = MailService.GetMailTemplates();
        //    return CreateGetResult<MailTemplateDetail, MailTemplateDetailItemResult, MailTemplateDetailListResult>(dtos);
        //}

        //// GET: Mail
        //public ActionResult Index()
        //{
        //    //var message = new EmailMessage
        //    //{
        //    //    Subject = "MSMQ Test Mail " + DateTime.UtcNow.Millisecond,
        //    //    Body = "howdy from asp.net mvc",
        //    //    From = "phiung1983@gmail.com",
        //    //    To = "minhrhett@gmail.com"
        //    //};

        //    //new MsmqService().SendMessageToQueue(@".\private$\MailQueue", message);

        //    //return View("../Mail/Index");
        //}


        //[HttpGet]
        //public ActionResult List()
        //{
        //    List<MailMessageQueueViewModel> list = new List<MailMessageQueueViewModel>();
        //    list = MailMessageQueueRepository.GetList().ToList();

        //    return PartialView("../Mail/_List", list);
        //}

        //public ActionResult Create(MailMessageQueueViewModel model)
        //{
        //    return PartialView("../Mail/_Edit", model);
        //}

        //public ActionResult Edit(int id)
        //{
        //    MailMessageQueueViewModel model = new MailMessageQueueViewModel();
        //    model = MailMessageQueueRepository.GetDetails(id);
        //    return PartialView("../Mail/_Edit", model);
        //}

        //[HttpPost]
        //public ActionResult Insert(MailMessageQueueViewModel addModel)
        //{
        //    bool flag = false;
        //    string message = string.Empty;

        //    try
        //    {
        //        if (String.IsNullOrEmpty(addModel.ReceiverMail))
        //            ModelState.AddModelError("ReceiverMail", Eagle.Resources.LanguageResource.Required);

        //        if (ModelState.IsValid)
        //        {
        //            if (addModel.SentDate != null)
        //            {
        //                if (DateTime.Compare(DateTime.UtcNow, addModel.SentDate.Value) > 0)
        //                {
        //                    message = Eagle.Resources.LanguageResource.LessThanNowDateTime;
        //                    return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        //                }


        //                if (addModel.SentDate == DateTime.UtcNow)
        //                {
        //                    addModel.SentDate= DateTime.UtcNow.AddMinutes(1);
        //                }

        //            }
        //            else
        //            {
        //                addModel.SentDate = DateTime.UtcNow.AddMinutes(1);
        //            }

        //            flag = MailMessageQueueRepository.Insert(addModel, out message);
        //        }
        //        else
        //        {
        //            var errors = ModelState.Values.SelectMany(v => v.Errors);
        //            message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "-"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = ex.ToString();
        //        flag = false;
        //    }

        //    return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult UpdateStatus(int id)
        //{
        //    string message = string.Empty;
        //    var flag = MailMessageQueueRepository.UpdateStatus(id, out message);
        //    return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        //}
    }
}
