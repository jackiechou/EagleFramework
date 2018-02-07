using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IMenuPermissionRepository: IRepositoryBase<MenuPermission>
    {
        IEnumerable<MenuPage> GetListByRoleId(Guid roleId);
        IEnumerable<MenuPage> GetListByRoleId(int typeId, MenuStatus? status, Guid roleId);
        IEnumerable<MenuPage> GetListByUserId(int typeId, MenuStatus? status, Guid userId);
        IEnumerable<MenuPermission> GetListByMenuId(int menuId);
        IEnumerable<MenuPermissionInfo> GetListByMenuId(int menuId, Guid roleId);
        MenuPermission GetDetails(int menuId, int levelId, Guid roleId);
        bool IsMenuAllowedAccessByRoles(int menuId, List<Guid> roles);
        bool IsMenuAllowedAccessByUserId(int menuId, Guid userId);
        bool HasDataExisted(int menuId, Guid roleId);
        void DeleteMenuPermissions(IEnumerable<MenuPermission> permissionLst);
    }
}
