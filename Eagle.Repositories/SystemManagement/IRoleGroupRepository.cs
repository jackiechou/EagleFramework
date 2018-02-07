using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IRoleGroupRepository : IRepositoryBase<RoleGroup>
    {
        IEnumerable<RoleGroup> GetRoleGroupsByRoleId(Guid applicationId, Guid roleId, bool? status=null);
        RoleGroup GetDetails(Guid roleId, Guid groupId);
        RoleGroupInfo GetRoleGroupDetail(int roleGroupId);
        RoleGroupInfo GetRoleGroupDetails(Guid roleId, Guid groupId);
        MultiSelectList PopulateGroupMultiSelectList(Guid applicationId, Guid? roleId, bool? status = null, string[] selectedValues = null, bool? isShowSelectText = false);

        bool HasDataExisted(Guid roleId, Guid groupId);
    }
}
