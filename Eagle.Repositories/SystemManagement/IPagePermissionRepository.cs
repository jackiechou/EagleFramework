using System;
using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IPagePermissionRepository: IRepositoryBase<PagePermission>
    {
        IEnumerable<Permission> GetPagePermissionLevels();
        IEnumerable<PagePermission> GetPagePermissions(bool? allowAccess);
        IEnumerable<PagePermission> GetPagePermissionsByRoleId(Guid roleId, bool? allowAccess = null);
        IEnumerable<PagePermission> GetPagePermissionsByPageId(int pageId);
        IEnumerable<PagePermission> GetDetails(Guid roleId, int pageId);
        bool HasDataExisted(Guid roleId, int pageId);
    }
}
