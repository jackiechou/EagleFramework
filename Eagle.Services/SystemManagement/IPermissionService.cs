using System;
using System.Collections.Generic;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Identity;

namespace Eagle.Services.SystemManagement
{
    public interface IPermissionService : IBaseService
    {
        #region Permission
        IEnumerable<PermissionDetail> GetPermissions(bool? status);
        PermissionDetail GetDetails(int id);
        void InsertPermission(PermissionEntry entry);
        void UpdatePermission(int id, PermissionEntry entry);
        void DeletePermission(int id);
        #endregion

        #region Page Permission

        List<PagePermissionEntry> GetPagePermissions(Guid applicationId, int pageId);
        IEnumerable<PagePermissionDetail> GetPagePermissions(bool? allowAccess);
        IEnumerable<PagePermissionDetail> GetPagePermissionsByPageId(int pageId);
        IEnumerable<PagePermissionDetail> GetPagePermissionsByRoleId(Guid roleId);

        void InsertPagePermission(PagePermissionEntry entry);
        void UpdatePagePermission(PagePermissionEntry entry);
        void UpdatePermission(int pageId, List<PagePermissionEntry> pagePermissionEntries);
        void DeletePagePermissionByPageId(int pageId);
        void DeletePagePermission(Guid roleId, int pageId);

        #endregion
    }
}
