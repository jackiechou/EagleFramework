using System.Collections.Generic;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Identity;

namespace Eagle.Services.SystemManagement
{
    public interface IFunctionCommandService
    {
        IEnumerable<AppClaimDetail> GetClaimsAuthorization(string functionName);
    }
}
