using System.Net;
using System.Net.Http;

namespace Eagle.WebApi.Filters
{
    internal static class HttpRequestMessageExtensions
    {
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private const string OwinContext = "MS_OwinContext";

        /// <summary>
        /// Gets the client ip address.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static IPAddress GetClientIpAddress(this HttpRequestMessage request)
        {
            IPAddress result = null;

            // Web-hosting. Needs reference to System.Web.dll
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    IPAddress.TryParse(ctx.Request.UserHostAddress, out result);
                }
            }

            // Self-hosting. Needs reference to System.ServiceModel.dll. 
            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    IPAddress.TryParse(remoteEndpoint.Address, out result);
                }
            }

            // Self-hosting using Owin. Needs reference to Microsoft.Owin.dll. 
            if (request.Properties.ContainsKey(OwinContext))
            {
                dynamic owinContext = request.Properties[OwinContext];
                if (owinContext != null)
                {
                    IPAddress.TryParse(owinContext.Request.RemoteIpAddress, out result);
                }
            }

            return result;
        }
    }
}