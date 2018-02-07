using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Eagle.WebApi.Results
{
    /// <summary>
    /// Ok With Headers
    /// </summary>
    public class OkWithHeaders<TData> : OkNegotiatedContentResult<TData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OkWithHeaders{TData}"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="contentNegotiator">The content negotiator.</param>
        /// <param name="request">The request.</param>
        /// <param name="formatters">The formatters.</param>
        public OkWithHeaders(TData content, IContentNegotiator contentNegotiator, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
            : base(content, contentNegotiator, request, formatters)
        {
            Initialize(new List<KeyValuePair<string, string>>());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OkWithHeaders{TData}" /> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="headers">The headers.</param>
        public OkWithHeaders(TData content, ApiController controller, IEnumerable<KeyValuePair<string, string>> headers)
            : base(content, controller)
        {
            Initialize(headers);
        }

        private void Initialize(IEnumerable<KeyValuePair<string, string>> headers)
        {
            Headers = headers;
        }

        public async override Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.ExecuteAsync(cancellationToken);

            foreach (var header in Headers)
            {
                response.Headers.Add(header.Key, header.Value);
            }

            return response;
        }

        private IEnumerable<KeyValuePair<string, string>> Headers { get; set; }
    }
}