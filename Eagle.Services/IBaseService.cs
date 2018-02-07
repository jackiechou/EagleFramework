using System;
using System.Security.Claims;
using log4net;

namespace Eagle.Services
{
    public interface IBaseService : IDisposable
    {
        ILog Logger { get; set; }
        Claim GetClaim(string type);
        void SetIdentity(ClaimsPrincipal identity);
        void SetClaim(string claim, string value);
        void SetClaim(Claim claim);
        void RemoveClaim(string claimType);
    }
}
