using System;
using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Identity;

namespace Eagle.Services.SystemManagement
{
    public interface ISecurityService : IBaseService
    {
        #region CLAIMS
        ClaimsIdentity FindByUserId(Guid userId);
        void DeleteAllClaimsOfUser(Guid userId);

        #endregion

        UserInfoDetail GetUserProfile(Guid userId);
        string GetPassword(string userName);
        void ChangePassword(ChangePasswordModel entry);
        bool ResetPassword(string email, out string newPassword);
        bool CheckLogin(string userName, string password);
        UserInfoDetail GetUserDetail(string userName);
        ClaimsPrincipal CreateUserClaims(UserInfoDetail userData, string redirectUri, bool isPersistent);
        bool CheckUserPermissioned(Guid userId, int capabilityId);
        bool HasPermission(string capabilityName);
        List<PageDetail> GetPagesForUser(Guid userId, bool? allowAccess = null);
        List<ModuleDetail> GetModulesForUser(Guid userId, bool? allowAccess = null);
        List<UserRoleInfoDetail> GetRolesForUser(Guid userId, bool? status = null);
        List<ModuleCapabilityInfoDetail> GetModuleCapabilitiesForUser(Guid userId, bool? allowAccess = null);


        IEnumerable<ModulePermissionDetail> GetModulePermissionCapabilityAccesses(Guid userId);
        IEnumerable<ModulePermissionDetail> GetModulePermissionCapabilityAccessesByRole(Guid roleId);
        IEnumerable<ModulePermissionDetail> GetModulePermissionCapabilityAccessesByModule(int moduleId);
        IEnumerable<PagePermissionDetail> GetPagePermissionsByRoleId(Guid roleId);
        IEnumerable<PagePermissionDetail> GetPagePermissionsByPageId(int pageId);
    }
}
