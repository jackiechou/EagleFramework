using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Exceptions;
using Eagle.Services.Validations;

namespace Eagle.WebApi.Filters
{
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var logException = true;
            //create error
            var error = new Error
            {
                ErrorCode = ErrorCode.Error,
                ErrorMessage = ErrorMessage.Messages[ErrorCode.Error]
            };

            var exception = context.Exception as BaseException;
            if (exception != null)
            {
                var exp = exception;
                error.ErrorCode = exp.ErrorCode;

                if (error.ErrorCode != null)
                {
                    error.ErrorMessage = ErrorMessage.Messages[(ErrorCode)error.ErrorCode];
                }
               
                logException = exp.LogException;

                if (context.Exception is ValidationError)
                {
                    foreach (DictionaryEntry de in exp.Data)
                    {
                        var list = de.Value as List<RuleViolation>;
                        if (list != null)
                        {
                            var extraInfos = list;
                            foreach (var extraInfo in extraInfos)
                            {
                                error.ExtraInfos.Add(new RuleViolation(extraInfo.ErrorCode, extraInfo.PropertyName, extraInfo.PropertyValue,ErrorMessage.Messages[extraInfo.ErrorCode]));
                            }
                        }
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(context.Exception.Message))
                {
                    error.ExtraInfos.Add(new RuleViolation(ErrorCode.Error, context.Exception.Source, context.Exception.InnerException, context.Exception.Message));
                }
            }

            if (logException)
                Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(context.Exception));

            context.Response = context.Request.CreateResponse(HttpStatusCode.OK, new FailResult(error));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AiExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            //if (context != null && context.Exception != null)
            //{
            //    //or reuse instance (recommended!). see note above 
            //    var ai = new TelemetryClient();
            //    ai.TrackException(context.Exception);
            //}
            base.Log(context);
        }
    }
}