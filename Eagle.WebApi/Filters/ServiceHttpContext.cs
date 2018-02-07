using System;
using System.Security.Principal;
using System.Web;
using Eagle.Core.Common;

namespace Eagle.WebApi.Filters
{
    /// <summary>
    /// Encapsulates all HTTP-specific information about an individual HTTP request.
    /// </summary>
    public sealed class ServiceHttpContext : DisposableObject, IHttpContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHttpContext"/> class.
        /// </summary>
        public ServiceHttpContext()
            : this(HttpContext.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHttpContext"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ServiceHttpContext(HttpContext context)
        {
            Context = context;

            SetTheHeader = (key, value) => ((HttpContext)Context).Response.AddHeader(key, value);
            SetTheStatusCode = status => ((HttpContext)Context).Response.StatusCode = status;
            GetTheStatusCode = () => ((HttpContext)Context).Response.StatusCode;
            SetTheStatus = s => ((HttpContext)Context).Response.Status = s;
            GetTheStatus = () => ((HttpContext)Context).Response.Status;
            GetTheUserHostAddress = () => ((HttpContext)Context).Request.UserHostAddress;
            GetThePrincipal = () => ((HttpContext)Context).User;
            SetThePrincipal = principal => ((HttpContext)Context).User = principal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHttpContext"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ServiceHttpContext(HttpContextBase context)
        {
            Context = context;
            SetTheHeader = (k, v) => ((HttpContextBase)Context).Response.AddHeader(k, v);
            SetTheStatusCode = s => ((HttpContextBase)Context).Response.StatusCode = s;
            GetTheStatusCode = () => ((HttpContextBase)Context).Response.StatusCode;
            SetTheStatus = s => ((HttpContextBase)Context).Response.Status = s;
            GetTheStatus = () => ((HttpContextBase)Context).Response.Status;
            GetTheUserHostAddress = () => ((HttpContextBase)Context).Request.UserHostAddress;
            GetThePrincipal = () => ((HttpContextBase)Context).User;
            SetThePrincipal = p => ((HttpContextBase)Context).User = p;
        }

        /// <summary>
        /// Sets the header.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetHeader(string key, string value)
        {
            SetTheHeader(key, value);
        }

        public int StatusCode
        {
            get { return GetTheStatusCode(); }
            set { SetTheStatusCode(value); }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status
        {
            get { return GetTheStatus(); }
            set { SetTheStatus(value); }
        }

        /// <summary>
        /// Gets the user host address.
        /// </summary>
        /// <value>
        /// The user host address.
        /// </value>
        public string UserHostAddress
        {
            get { return GetTheUserHostAddress(); }
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public IPrincipal User
        {
            get { return GetThePrincipal(); }
            set { SetThePrincipal(value); }
        }

        private Action<string, string> SetTheHeader { get; set; }
        private Action<int> SetTheStatusCode { get; set; }
        private Func<int> GetTheStatusCode { get; set; }

        private Action<string> SetTheStatus { get; set; }
        private Func<string> GetTheStatus { get; set; }

        private Func<string> GetTheUserHostAddress { get; set; }

        private Func<IPrincipal> GetThePrincipal { get; set; }
        private Action<IPrincipal> SetThePrincipal { get; set; }

        IServiceProvider Context { get; set; }
    }
}