using System;
using System.Security.Principal;

namespace Eagle.WebApi.Filters
{
    /// <summary>
    /// Http Context Interface
    /// </summary>
    public interface IHttpContext : IDisposable
    {
        /// <summary>
        /// Sets the header.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        void SetHeader(string p1, string p2);

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        string Status { get; set; }

        /// <summary>
        /// Gets the user host address.
        /// </summary>
        /// <value>
        /// The user host address.
        /// </value>
        string UserHostAddress { get; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        IPrincipal User { get; set; }
    }
}
