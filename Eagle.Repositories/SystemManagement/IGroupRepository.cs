using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IGroupRepository : IRepositoryBase<Group>
    {
        IEnumerable<Group> GetList(Guid applicationId, string groupName, ref int? recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);
        IEnumerable<Group> GetList(Guid applicationId, bool? status = null);
        IEnumerable<Group> GetList(Guid applicationId, Guid roleId, bool? status = null);
        int GetNewListOrder();
        SelectList PopulateGroupDropDownList(Guid applicationId, bool? status = null, string selectedValue = null,
            bool? isShowSelectText = false);
        SelectList PopulateGroupDropDownList(Guid applicationId, Guid roleId, bool? status = null, bool? isShowSelectText = false);
        MultiSelectList PopulateGroupMultiSelectList(Guid applicationId, bool? status = null, string[] selectedValues = null,
            bool? isShowSelectText = false);
    
        bool HasGroupExisted(Guid applicationId, string groupName);
        
    }
}
