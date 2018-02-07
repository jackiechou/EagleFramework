using System;
using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IUserRoleRepository : IRepositoryBase<UserRole>
    {
        IEnumerable<UserRoleInfo> GetUserRolesByUserId(Guid userId);
        IEnumerable<UserRoleInfo> GetUserRolesByUserName(string userName, bool? isActive);
        IEnumerable<UserRole> GetUsersByRoleId(Guid roleId);
        IEnumerable<User> GetUsers(Guid roleId);
        IEnumerable<UserRoleInfo> GetRoles(Guid userId, bool? isActive);
        UserRole GetDetails(Guid userId, Guid roleId);
        bool HasDataExisted(Guid userId, Guid roleId);
    }
}
