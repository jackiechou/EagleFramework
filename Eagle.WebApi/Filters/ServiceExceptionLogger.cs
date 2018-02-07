using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.ExceptionHandling;
using Eagle.Core.Logging;
using Newtonsoft.Json;

namespace Eagle.WebApi.Filters
{
    /// <summary>
    /// NuGet: Install-Package Microsoft.AspNet.WebApi -Version 5.1.0
    /// </summary>
    public class ServiceExceptionLogger : ExceptionLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceExceptionLogger"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="exceptionsToLog">The exceptions to log.</param>
        /// <param name="exceptionsToIgnore">The exceptions to ignore.</param>
        public ServiceExceptionLogger(HashSet<Type> exceptionsToLog = null, HashSet<Type> exceptionsToIgnore = null)
        {
            ExceptionsToLog = exceptionsToLog ?? new HashSet<Type>();
            ExceptionsToIgnore = exceptionsToIgnore ?? new HashSet<Type>();
        }

        public override void Log(ExceptionLoggerContext context)
        {
            var exceptionLogData = new ExceptionLogData(context);
        }

        public override bool ShouldLog(ExceptionLoggerContext context)
        {
            if (context.Exception == null) return base.ShouldLog(context);

            var exceptionType = context.Exception.GetType();

            var result = (ExceptionsToLog.Count > 0 ? ExceptionsToLog.Contains(exceptionType) : base.ShouldLog(context)) && !ExceptionsToIgnore.Contains(exceptionType);

            return result;
        }

        private HashSet<Type> ExceptionsToIgnore { get; set; }
        private HashSet<Type> ExceptionsToLog { get; set; }

        internal sealed class RequestLogData
        {
            public RequestLogData(HttpRequestMessage request)
            {
                Method = request.Method.ToString();
                Headers = request.Headers;
                RequestUri = request.RequestUri.ToString();
                Content = request.Content;
            }

            public string Method { get; set; }
            public HttpRequestHeaders Headers { get; set; }
            public string RequestUri { get; set; }
            public HttpContent Content { get; set; }
        }

        private sealed class ExceptionLogData : Exception
        {
            public ExceptionLogData(ExceptionLoggerContext context)
            {
                var request = JsonConvert.SerializeObject(new RequestLogData(context.Request));
                Data.Add("Request", request);

                Context = context;
            }

            public override string Message
            {
                get { return Context.Exception != null ? Context.Exception.Message : string.Empty; }
            }

            public override string StackTrace
            {
                get { return Context.Exception != null ? Context.Exception.StackTrace : string.Empty; }
            }

            public override string Source
            {
                get { return Context.Exception != null ? Context.Exception.Source : _source; }
                set
                {
                    if (Context.Exception != null)
                    {
                        Context.Exception.Source = _source;
                    }
                    _source = value;
                }
            }
            private string _source = string.Empty;

            public override Exception GetBaseException()
            {
                if (Context.Exception != null) return Context.Exception.GetBaseException();

                return base.GetBaseException();
            }

            private ExceptionLoggerContext Context { get; set; }
        }
    }
}