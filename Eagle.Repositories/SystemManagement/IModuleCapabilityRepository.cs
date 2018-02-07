using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IModuleCapabilityRepository : IRepositoryBase<ModuleCapability>
    {
        IEnumerable<ModuleCapability> GetList(ref int? recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);
        IEnumerable<ModuleCapability> GetModuleCapabilitiesByModuleId(int moduleId, int? isActive);
        IEnumerable<ModuleCapabilityInfo> GetModuleCapabilities(int moduleId, ModuleCapabilityStatus? status = null);
        ModuleCapability GetDetails(int moduleId, string capabilityName);

        MultiSelectList PopulateModuleCapabilityMultiSelectList(int moduleId, ModuleCapabilityStatus? isActive,
            string[] selectedValues);
        int GetListOrder();
        bool HasModuleCapabilityNameExisted(string capabilityName, int moduleId);
        bool HasModuleCapabilityCodeExisted(string capabilityCode, int moduleId);
    }
}
