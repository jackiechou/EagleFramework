using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IPermissionRepository : IRepositoryBase<Permission>
    {
        IEnumerable<Permission> GetPermissions(bool? status);
        bool HasDataExisted(string permissionName);
        int GetNewListOrder();
    }
}
