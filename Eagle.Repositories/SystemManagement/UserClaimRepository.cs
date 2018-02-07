using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class UserClaimRepository : RepositoryBase<UserClaim>, IUserClaimRepository
    {
        public UserClaimRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<UserClaim> GetUserClaimsByUserId(Guid userId)
        {
            return DataContext.Get<UserClaim>().Where(x => x.UserId == userId).AsEnumerable();
        }

       
    }
}
