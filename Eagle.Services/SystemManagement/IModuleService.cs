using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.SystemManagement;

namespace Eagle.Services.SystemManagement
{
    public interface IModuleService: IBaseService
    {
        #region MODULE

        IEnumerable<ModuleDetail> Search(ModuleSearchEntry filter, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<ModuleDetail> GetModules(ModuleType? moduleTypeId, ModuleStatus? status);
        MultiSelectList PopulateModuleMultiSelectList(ModuleType? moduleTypeId, ModuleStatus? status, string[] selectedValues);
        SelectList PopulateModuleList(ModuleType? moduleTypeId, ModuleStatus? status, string selectedValue,
            bool isShowSelectText = false);
        SelectList PopulateModuleTypeSelectList(int? selectedValue, bool isShowSelectText = false);
        SelectList PopulateAlignmentList(string selectedValue, bool isShowSelectText = false);
        SelectList PopulateInsertedPositionList(string selectedValue, bool isShowSelectText = false);
        SelectList PopulateVisibilityList(string selectedValue, bool isShowSelectText = false);
        SelectList PopulatePaneList(string selectedValue, bool isShowSelectText = false);
        ModuleDetail GetModuleDetails(int id);

        void InsertModule(Guid applicationId, Guid roleId, ModuleEntry entry);
        void UpdateModule(Guid roleId, ModuleEditEntry entry);
        void UpdateModuleStatus(int moduleId, ModuleStatus status);
        void DeleteModule(int moduleId);

        #endregion

        #region Module Capability
        List<ModuleCapabilityEditEntry> GetModuleCapabilities(int moduleId);
        IEnumerable<ModuleCapabilityDetail> GetModuleCapabilitiesByModuleId(int moduleId, int? isActive);

        MultiSelectList PopulateModuleCapabilityMultiSelectList(int moduleId, ModuleCapabilityStatus? isActive,
            string[] selectedValues);

        ModuleCapabilityDetail CreateModuleCapability(int moduleId, CapabilityEntry entry);
        void CreateModuleCapabilities(Guid roleId, int moduleId, List<CapabilityEntry> moduleCapabilities);
        void UpdateModuleCapability(int id, ModuleCapabilityEntry entry);
        void UpdateModuleCapabilityListOrder(int id, int listOrder);
        void UpdateModuleCapabilityStatus(int id, ModuleCapabilityStatus status);
        void DeleteModuleCapability(int id);
        void DeleteModuleCapabilityByModuleId(int moduleId);

        #endregion

        #region Module Permission

        ModuleRolePermissionEntry GetModuleRolePermissionEntry(Guid applicationId, int moduleId);
        List<ModuleRolePermissionDetail> GetModuleRolePermission(Guid applicationId, int moduleId);

        IEnumerable<ModulePermissionDetail> GetModulePermissions(int moduleId);
        IEnumerable<ModulePermissionDetail> GetModulePermissions(int moduleId, int capabilityId);
        IEnumerable<ModulePermissionDetail> GetModulePermissionsByRoleId(Guid roleId);
        IEnumerable<ModulePermissionDetail> GetModulePermissionsByRoleId(Guid roleId, int moduleId);

        ModulePermissionDetail GetModulePermissionDetail(int id);
        void CreateModulePermission(ModulePermissionEntry entry);
        void EditModulePermission(int id, ModulePermissionEntry entry);
        void UpdateModulePermissionStatus(Guid roleId, int moduleId, int capabilityId, bool status);
        void DeleteModulePermission(int id);
        void DeleteModulePermissionByModuleId(int moduleId);

        #endregion
    }
}
