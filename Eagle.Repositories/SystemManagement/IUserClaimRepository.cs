using System;
using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IUserClaimRepository : IRepositoryBase<UserClaim>
    {
        IEnumerable<UserClaim> GetUserClaimsByUserId(Guid userId);
    }
}
