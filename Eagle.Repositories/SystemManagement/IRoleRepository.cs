using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IRoleRepository : IRepositoryBase<Role>
    {
        IEnumerable<Role> GetRoles(Guid applicationId, bool? status);
        IEnumerable<RoleInfo> GetRoles(Guid applicationId, Guid userId, bool? status = null);
        IEnumerable<Role> GetRoles(Guid applicationId, bool? status, ref int? recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);

        IEnumerable<RoleInfo> SearchRoles(Guid applicationId, Guid? groupId, string roleName, bool? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        int GetNewListOrder();
        SelectList PopulateRoleToDropDownList(bool? status = null, string selectedValue = null, bool? isShowSelectText = false);
        MultiSelectList PopulateRoleMultiSelectList(bool? status = null, string[] selectedValues = null, bool? isShowSelectText = false);
        MultiSelectList PopulateRoleMultiSelectList(Guid userId, bool? status = null, string[] selectedValues = null,
            bool? isShowSelectText = false);
        bool IsRoleExisted(Guid applicationId, string roleName);
    }
}
