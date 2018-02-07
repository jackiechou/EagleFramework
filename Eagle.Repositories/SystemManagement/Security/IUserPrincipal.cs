using System.Collections.Generic;
using System.Security.Principal;

namespace Eagle.Repositories.SystemManagement.Security
{
    public interface IUserPrincipal : IPrincipal
    {
        IEnumerable<string> Roles { get; }

        IUserIdentity UserIdentity { get; }
    }
}
