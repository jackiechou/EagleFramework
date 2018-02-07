using System;
using System.Web;
using System.Web.Mvc;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class SettingController : BaseController
    {
        public SettingController() {
        }

        // GET: Admin/Setting
        public ActionResult Index()
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            int statusCode = (httpException != null) ? httpException.GetHttpCode() : 500;
            string message;
            if (!string.IsNullOrEmpty(exception.Message))
            {
                message = exception.Message;
            }
            else
            {
                message = exception.InnerException != null ? exception.InnerException.Message : exception.StackTrace;
            }
            var exceptionInfo = new ExceptionInfo(statusCode, "Error", "Error", message);
            return View("Error/Index", exceptionInfo);
        }

        public ActionResult BadRequest()
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            int statusCode = (httpException != null) ? httpException.GetHttpCode() : 401;

            string errorMessage;
            if (!string.IsNullOrEmpty(exception.Message))
            {
                errorMessage = exception.Message;
            }
            else
            {
                errorMessage = exception.InnerException != null ? exception.InnerException.Message : exception.StackTrace;
            }
                                 
            string imageUrl = "~/Images/error-page.gif";
            string pageTitle = "Bad request 401";
            string title = "Bad request 401";

            var exceptionInfo = new ExceptionInfo(statusCode, pageTitle, title, errorMessage ?? "The request had bad syntax or was inherently impossible to be satisfied.", imageUrl);
            return View("Error/Index", exceptionInfo);
        }

        public ActionResult GenericErrorPage()
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            int statusCode = (httpException != null) ? httpException.GetHttpCode() : 401;

            string errorMessage;
            if (!string.IsNullOrEmpty(exception.Message))
            {
                errorMessage = exception.Message;
            }
            else
            {
                errorMessage = exception.InnerException != null ? exception.InnerException.Message : exception.StackTrace;
            }

            string imageUrl = "~/Images/error-page.gif";
            string pageTitle = "Generic Error Page";
            string title = "Generic Error Page";

            var exceptionInfo = new ExceptionInfo(statusCode, pageTitle, title, errorMessage?? "The server is experiencing a problem with the page you requested. We apologize for the inconvenience. We will resolve this issue shortly.", imageUrl);
            return View("Error/Index", exceptionInfo);
        }

        public ActionResult InternalServerError()
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            int statusCode = (httpException != null) ? httpException.GetHttpCode() : 500;

            string errorMessage;
            if (!string.IsNullOrEmpty(exception.Message))
            {
                errorMessage = exception.Message;
            }
            else
            {
                errorMessage = exception.InnerException != null ? exception.InnerException.Message : exception.StackTrace;
            }

            string imageUrl = "~/Images/shock.png";
            string pageTitle = "500 Internal Server Error";
            string title = "Page Doesn't Not Exist";

            var exceptionInfo = new ExceptionInfo(statusCode, pageTitle, title, errorMessage?? "The server is experiencing a problem with the page you requested. We apologize for the inconvenience. We will resolve this issue shortly.", imageUrl);
            return View("Error/Index", exceptionInfo);
        }

        public ActionResult Forbidden403()
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            int statusCode = (httpException != null) ? httpException.GetHttpCode() : 403;

            string errorMessage;
            if (!string.IsNullOrEmpty(exception.Message))
            {
                errorMessage = exception.Message;
            }
            else
            {
                errorMessage = exception.InnerException != null ? exception.InnerException.Message : exception.StackTrace;
            }
    
            string imageUrl = "~/Images/access-denied.gif";
            string pageTitle = "Forbidden 403";
            string title = "No Access";

            var exceptionInfo = new ExceptionInfo(statusCode, pageTitle, title, errorMessage?? "The request is for something forbidden. Authorization will not help.", imageUrl);
            return View("Error/Index", exceptionInfo);
        }

        public ActionResult PageNotFound()
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            int statusCode = (httpException != null) ? httpException.GetHttpCode() : 0;

            string errorMessage;
            if (!string.IsNullOrEmpty(exception.Message))
            {
                errorMessage = exception.Message;
            }
            else
            {
                errorMessage = exception.InnerException != null ? exception.InnerException.Message : exception.StackTrace;
            }
            string imageUrl = "~/Images/page-not-found.png";
            string pageTitle = "Page not Found 400";
            string title = "Page not Found";

            var exceptionInfo = new ExceptionInfo(statusCode, pageTitle, title, errorMessage?? "The request is for something forbidden. Authorization will not help.", imageUrl);
            return View("Error/Index", exceptionInfo);
        }

        public ActionResult PageNetworkError()
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            int statusCode = (httpException != null) ? httpException.GetHttpCode() : 0;

            string errorMessage;
            if (!string.IsNullOrEmpty(exception.Message))
            {
                errorMessage = exception.Message;
            }
            else
            {
                errorMessage = exception.InnerException != null ? exception.InnerException.Message : exception.StackTrace;
            }
            string imageUrl = "~/Images/page-not-found.png";
            string pageTitle = "Network Error";
            string title = "Network Error";

            var exceptionInfo = new ExceptionInfo(statusCode, pageTitle, title, errorMessage?? "Network Error.", imageUrl);
            return View("Error/Index", exceptionInfo);
        }

        public ActionResult PermissionDenied()
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            int statusCode = (httpException != null) ? httpException.GetHttpCode() : 403;
            string errorMessage;
            if (!string.IsNullOrEmpty(exception.Message))
            {
                errorMessage = exception.Message;
            }
            else
            {
                errorMessage = exception.InnerException != null ? exception.InnerException.Message : exception.StackTrace;
            }

            string imageUrl = "~/Images/user_security.png";
            string pageTitle = "Unauthorized";
            string title = LanguageResource.Error403Contents;

            var exceptionInfo = new ExceptionInfo(statusCode, pageTitle, title, errorMessage?? "Forbidden - Unauthorized Error", imageUrl);
            return View("Error/Index", exceptionInfo);
        }
        
        public ActionResult UnauthorizedAccess()
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            int statusCode = (httpException != null) ? httpException.GetHttpCode() : 403;
            string errorMessage;
            if (!string.IsNullOrEmpty(exception.Message))
            {
                errorMessage = exception.Message;
            }
            else
            {
                errorMessage = exception.InnerException != null ? exception.InnerException.Message : exception.StackTrace;
            }
        
            string imageUrl = "~/Images/shock.png";
            string pageTitle = "Unauthorized Access";
            string title = "Your request requires authentication";
           
            var exceptionInfo = new ExceptionInfo(statusCode, pageTitle, title, errorMessage?? "Forbidden - Unauthorized Error. Your request requires authentication.", imageUrl);
            return View("Error/Index", exceptionInfo);
        }

        public ActionResult UnavailableServer()
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            int statusCode = (httpException != null) ? httpException.GetHttpCode() : 500;
            string errorMessage;
            if (!string.IsNullOrEmpty(exception.Message))
            {
                errorMessage = exception.Message;
            }
            else
            {
                errorMessage = exception.InnerException != null ? exception.InnerException.Message : exception.StackTrace;
            }
  
            string imageUrl = "~/Images/shock.png";
            string pageTitle = "500 Internal Server Error";
            string title = "500 Internal Server Error";

            var exceptionInfo = new ExceptionInfo(statusCode, pageTitle, title, errorMessage?? "The server encountered an unexpected condition which prevented it from fulfilling the request.", imageUrl);
            return View("Error/Index", exceptionInfo);
        }


        //#region Dispose

        //private bool _disposed;

        //[NonAction]
        //protected override void Dispose(bool isDisposing)
        //{
        //    if (!_disposed)
        //    {
        //        if (isDisposing)
        //        {
        //        }
        //        _disposed = true;
        //    }
        //    base.Dispose(isDisposing);
        //}

        //#endregion
    }
}