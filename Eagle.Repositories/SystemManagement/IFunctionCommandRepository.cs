using System;
using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IFunctionCommandRepository : IRepositoryExtend<FunctionCommand, Guid>
    {
        IEnumerable<AppClaim> GetAppClaimsByFunctionName(string functionName);
    }
}
