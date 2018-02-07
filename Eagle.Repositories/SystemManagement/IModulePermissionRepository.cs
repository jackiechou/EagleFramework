using System;
using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IModulePermissionRepository : IRepositoryBase<ModulePermission>
    {
        IEnumerable<ModulePermission> GetModulePermissions(int moduleId);
        IEnumerable<ModulePermission> GetModulePermissions(int moduleId, int capabilityId);
        IEnumerable<ModulePermission> GetModulePermissionsByRoleId(Guid roleId);
        IEnumerable<ModulePermissionInfo> GetModulePermissionsByRoleId(Guid roleId, int moduleId);
        
        ModulePermission GetDetails(Guid roleId, int capabilityId);
    }
}
