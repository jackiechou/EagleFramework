using System;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Common;

namespace Eagle.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        private ExceptionInfo GetErrorMessage(StatusCode? statusCode = StatusCode.InternalServerError)
        {
            var exception = Server.GetLastError();
            string message = "Error", pageTile, title;
            if (exception != null)
            {
                if (!string.IsNullOrEmpty(exception.Message))
                {
                    message = exception.Message;
                }
                else if (exception.InnerException != null)
                {
                    message = exception.InnerException.Message;
                }
                else if (!string.IsNullOrEmpty(exception.StackTrace))
                {
                    message = exception.StackTrace;
                }
                else
                {
                    message = exception.ToString();
                }
            }

            var httpException = (HttpException)exception;
            var code = httpException != null ? (StatusCode)httpException.GetHttpCode() : statusCode;

            switch (code)
            {
                case StatusCode.NetworkError:
                    pageTile = "NetworkError";
                    title = "NetworkError";
                    break;
                case StatusCode.RequestTypeViolation:
                    pageTile = "Request Type Violation";
                    title = "Moved Temporarily - Error in chaging request type from POST to Get";
                    break;
                case StatusCode.Unauthorized:
                    pageTile = "Unauthorized";
                    title = "Forbidden - Unauthorized Error";
                    break;
                case StatusCode.SessionExpired:
                    pageTile = "SessionExpired";
                    title = "SessionExpired";
                    break;
                case StatusCode.PageNotFound:
                    pageTile = "NotFound";
                    title = "Page not Found! Sorry about this.";
                    break;
                case StatusCode.InternalServerError:
                    pageTile = "Internal Server Error";
                    title = "Internal Server Error";
                    break;
                case StatusCode.ServiceUnavailable:
                    pageTile = "Service Unavailable";
                    title = "Service Unavailable";
                    break;
                case StatusCode.TimeOut:
                    pageTile = "TimeOut";
                    title = "Unexpected time-out";
                    break;
                default:
                    pageTile = "Unknown";
                    title = "Unknown";
                    break;
             }

            var sb = new StringBuilder();
            sb.AppendFormat("We will work on fixing that right away.<br />Please check the url or go to main page and see if you can locate what you are looking for");
            sb.AppendLine(message);

            var exceptionInfo = new ExceptionInfo(Convert.ToInt32(statusCode), pageTile, title, sb.ToString());
            return exceptionInfo;
        }

        public ActionResult Index()
        {
            var exceptionInfo = GetErrorMessage();
            return View("Error/Index", exceptionInfo);
        }
       
        public ActionResult NetworkError()
        {
            var exceptionInfo = GetErrorMessage(StatusCode.NetworkError);
            return View("Error/Index", exceptionInfo);
        }
        public ActionResult RequestTypeViolation()
        {
            var exceptionInfo = GetErrorMessage(StatusCode.RequestTypeViolation);
            return View("Error/Index", exceptionInfo);
        }
        public ActionResult Unauthorized()
        {
            var exceptionInfo = GetErrorMessage(StatusCode.Unauthorized);
            return View("Error/Index", exceptionInfo);
        }
        public ActionResult SessionExpired()
        {
            var exceptionInfo = GetErrorMessage(StatusCode.SessionExpired);
            return View("Error/Index", exceptionInfo);
        }
        public ActionResult NotFound()
        {
            // deal with idiotic issues from IIS: 
            Response.TrySkipIisCustomErrors = true;
            var exceptionInfo = GetErrorMessage(StatusCode.PageNotFound);
            return View("Error/Index", exceptionInfo);
        }

        public ActionResult InternalServerError()
        {
            var exceptionInfo = GetErrorMessage(StatusCode.InternalServerError);
            return View("Error/Index", exceptionInfo);
        }

        public ActionResult ServiceUnavailable()
        {
            var exceptionInfo = GetErrorMessage(StatusCode.ServiceUnavailable);
            return View("Error/Index", exceptionInfo);
        }

        public ActionResult TimeOut()
        {
            var exceptionInfo = GetErrorMessage(StatusCode.TimeOut);
            return View("Error/Index", exceptionInfo);
        }

        public ActionResult AjaxClearSession()
        {
            Thread.Sleep(500);
            Session.Clear();
            string msg = "Web session is cleared on @ " + DateTime.UtcNow.ToLongTimeString();
            return Json(new { Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxError()
        {
            Thread.Sleep(500);
            // Return the famous 500 staus to create an artificial error
            Response.StatusCode = 500;
            Response.End();
            return new EmptyResult();
        }
    }
}