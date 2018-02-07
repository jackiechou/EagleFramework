using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;

namespace Eagle.WebApi.Filters
{  /// <summary>
   /// Service Exception Handler
   /// </summary>
    public class ServiceExceptionHandler : ExceptionHandler
    {
        /// <summary>
        /// Handles the core.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Handle(ExceptionHandlerContext context)
        {
            var exception = new JsonExceptionResult(context);
            context.Result = exception;
        }

        /// <summary>
        /// Json Exception
        /// </summary>
        public class JsonExceptionResult : IHttpActionResult
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="JsonException"/> class.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="statusCode">The HTTP status code.</param>
            public JsonExceptionResult(ExceptionHandlerContext context, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            {
                StatusCode = statusCode;

                if (context == null) return;

                Request = context.Request;
                Content = context.Exception.Message;
            }

            /// <summary>
            /// Gets or sets the request.
            /// </summary>
            /// <value>
            /// The request.
            /// </value>
            [JsonIgnore]
            public HttpRequestMessage Request { get; set; }

            /// <summary>
            /// Gets or sets the status code.
            /// </summary>
            /// <value>
            /// The status code.
            /// </value>
            public HttpStatusCode StatusCode { get; set; }

            /// <summary>
            /// Gets or sets the content.
            /// </summary>
            /// <value>
            /// The content.
            /// </value>
            [JsonProperty("Message")]
            public string Content { get; set; }

            private Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(StatusCode)
                {
                    Content = new StringContent(Content),
                    RequestMessage = Request
                };

                return Task.FromResult(response);
            }

            Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
            {
                return ExecuteAsync(cancellationToken);
            }
        }
    }
}