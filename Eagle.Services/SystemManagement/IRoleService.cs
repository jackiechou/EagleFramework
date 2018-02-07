using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Services.Dtos.SystemManagement.Identity;

namespace Eagle.Services.SystemManagement
{
    public interface IRoleService : IBaseService
    {
        #region ROLE

        IEnumerable<RoleDetail> GetRoles(Guid applicationId, bool? status);
        IEnumerable<RoleDetail> GetRoles(Guid applicationId, bool? status, ref int? recordCount, string orderBy = null,
           int? page = null, int? pageSize = null);
        IEnumerable<RoleInfoDetail> SearchRoles(Guid applicationId, RoleSearchEntry filter,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<RoleInfoDetail> GetRolesByUserId(Guid applicationId, Guid userId, bool? status=null);

        SelectList PopulateStatusDropDownList(string selectedValue = null, bool? isShowSelectText = true);
        SelectList PopulateRoleDropDownList(bool? status = null, string selectedValue = null, bool? isShowSelectText = false);
        MultiSelectList PopulateRoleMultiSelectList(bool? status = null, string[] selectedValues = null, bool? isShowSelectText = false);
        MultiSelectList PopulateSelectedRoleMultiSelectList(Guid userId, bool? status = null, string[] selectedValues = null,
            bool? isShowSelectText = false);
        RoleDetail GetRoleDetails(Guid roleId);
        void CreateRole(Guid applicationId, RoleEntry entry);
        void UpdateRole(Guid applicationId, RoleEditEntry entry);
        void UpdateRoleStatus(Guid id, bool status);
        #endregion

        #region USER ROLE

        IEnumerable<UserRoleInfoDetail> GetUserRolesByUserId(Guid applicationId, Guid userId, bool? status = null);
        IEnumerable<UserRoleInfoDetail>
            GetUserRolesByUserName(Guid applicationId, string userName, bool? status = null);
        IEnumerable<UserRoleDetail> GetUsersByRoleId(Guid roleId);
        IEnumerable<UserDetail> GetUsers(Guid roleId);
        #endregion

        #region GROUP

        SelectList PopulateGroupDropDownList(Guid applicationId, bool? status = null, string selectedValue = null,
            bool? isShowSelectText = false);
        SelectList PopulateGroupDropDownList(Guid applicationId, Guid roleId, bool? status=null, bool? isShowSelectText = false);
        MultiSelectList PopulateGroupMultiSelectList(Guid applicationId, bool? status = null, string[] selectedValues = null, bool? isShowSelectText = false);
        MultiSelectList PopulateGroupMultiSelectList(Guid applicationId, Guid roleId, bool? status = null, string[] selectedValues = null, bool? isShowSelectText = false);
        IEnumerable<RoleGroupDetail> GetGroups(Guid applicationId, Guid roleId, bool? status = null);
        IEnumerable<GroupDetail> GetGroups(Guid applicationId, GroupSearchEntry entry, ref int? recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);
        GroupDetail GetGroupDetails(Guid roleGroupId);
        void CreateGroup(Guid applicationId, GroupEntry entry);
        void UpdateGroup(Guid applicationId, Guid id, GroupEntry entry);
        void UpdateGroupStatus(Guid roleGroupId, bool status);
        #endregion

        #region ROLE GROUP

        IEnumerable<RoleGroupDetail> GetRoleGroups(Guid applicationId, Guid roleId, bool? status = null);
        RoleGroupDetail GetRoleGroupDetail(int roleGroupId);
        RoleGroupInfoDetail GetRoleGroupDetails(Guid roleId, Guid groupId);
        void CreateRoleGroups(Guid roleId, List<Guid> groupIds);
        void DeleteRoleGroup(Guid roleId, Guid groupId);
        void DeleteRoleGroups(Guid roleId, List<Guid> groupIds);
        void UpdateRoleGroups(Guid applicationId, Guid roleId, List<Guid> selectedGroupIds);

        #endregion

    }
}
