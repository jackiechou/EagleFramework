using System;
using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IUserRoleGroupRepository : IRepositoryBase<UserRoleGroup>
    {
        IEnumerable<UserRoleGroupInfo> GetUserRoleGroups(Guid userId);
        IEnumerable<UserRoleGroup> GetUserRoleGroupsByUserId(Guid userId);
        UserRoleGroup GetDetail(Guid userId, int roleGroupId);
        bool HasDataExisted(Guid userId, int roleGroupId);
    }
}
