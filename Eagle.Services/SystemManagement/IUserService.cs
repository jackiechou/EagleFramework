using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Services.Dtos.SystemManagement.Identity;

namespace Eagle.Services.SystemManagement
{
    public interface IUserService : IBaseService
    {
        IEnumerable<UserContactDetail> SearchUsers(Guid applicationId, UserSearchEntry filter, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<UserContactDetail> GetUsers(Guid applicationId, out int recordCount, string orderBy = null, int? page = null,
          int? pageSize = null);

        IEnumerable<UserDetail> GetUserOnlines(Guid applicationId, int minutes);
        UserDetail GetDetailsByUserName(string userName);
        IEnumerable<UserProfileDetail> GetProfiles(List<Guid> userIds);
        
        SelectList PopulateQuestionsSelectList(string selectedValue = null, bool? isShowSelectText = false);
        SelectList PopulateSexSelectList(int? selectedValue = null, bool? isShowSelectText = false);
        SelectList PopulateTitleSelectList(int? selectedValue = null, bool? isShowSelectText = false);
        SelectList PopulateStatusSelectList(int? selectedValue = null, bool? isShowSelectText = false);
        UserDetail CreateUser(Guid applicationId, Guid userId, int vendorId, UserEntry entry);
        void EditUser(Guid applicationId, Guid userId, UserEditEntry entry);
        void Approve(Guid userId);
        void UnApprove(Guid userId);
        void LockUser(Guid userId);
        void UnLockUser(Guid userId);
        void UnLockAccount(string userName);


        #region Profile
        UserInfoDetail GetUserProfile(Guid userId);
        UserInfoDetail GetDetailsByEmail(string email);
        UserProfileInfoDetail GetProfileDetails(Guid userId);
        UserProfileDetail CreateProfile(UserProfileEntry entry);

        #endregion

        #region User Role

        IEnumerable<UserRoleDetail> GetUserRoles(Guid userId);
        void AssignRolesForUser(Guid userId, List<Guid> roleIds);
        void CreateUserRole(UserRoleEntry entry);
        void CreateRolesForUser(Guid userId, List<UserRoleCreate> roles);
        void DeleteRoleForUser(Guid userId, List<Guid> roleIds);
        void DeleteUserRoleByUserId(Guid userId);
        void DeleteUserRoleByRoleId(Guid roleId);

        #endregion

        #region User Group

        IEnumerable<UserRoleGroupEdit> GetUserRoleGroups(Guid userId);
        IEnumerable<UserRoleGroupEdit> GeUserRoleGroups(Guid applicationId, Guid roleId, Guid userId, bool? status = null);
        void CreateUserRoleGroup(UserRoleGroupEntry entry);
        void CreateRoleGroupsForUser(Guid userId, List<UserRoleGroupCreate> groups);
        void DeleteRoleGroupsForUser(Guid userId, List<int> roleGroupIds);

        #endregion

        #region User Vendor

        void InsertUserVendor(Guid userId, int vendorId);
        
        #endregion
    }
}
