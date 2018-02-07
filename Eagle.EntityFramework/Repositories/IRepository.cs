using System;
using System.Security.Claims;

namespace Eagle.EntityFramework.Repositories
{
    public interface IRepository: IDisposable
    {
        /// <summary>
        /// Gets the instance identifier.
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        Guid InstanceId { get; }
        void SetIdentity(ClaimsPrincipal identity);
    }
}