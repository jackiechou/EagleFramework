using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IModuleRepository : IRepositoryBase<Module>
    {
        IEnumerable<Module> Search(string keywords, ModuleType moduleTypeId, ModuleStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<Module> GetList(ModuleType? moduleTypeId, ModuleStatus? status);
        MultiSelectList PopulateModuleMultiSelectList(ModuleType? moduleTypeId, ModuleStatus? status, string[] selectedValues);
        SelectList PopulateModuleList(ModuleType? moduleTypeId, ModuleStatus? status, string selectedValue, bool isShowSelectText = false);
        SelectList PopulateModuleTypeSelectList(int? selectedValue, bool isShowSelectText = false);
        SelectList PopulateAlignmentList(string selectedValue, bool isShowSelectText = false);
        SelectList PopulateInsertedPositionList(string selectedValue, bool isShowSelectText = false);
        SelectList PopulateVisibilityList(string selectedValue, bool isShowSelectText = false);
        SelectList PopulatePaneList(string selectedValue, bool isShowSelectText = false);
        bool HasModuleNameExisted(string moduleName);
        bool HasModuleTitleExisted(string moduleTitle);
        bool HasModuleCodeExisted(string moduleCode);
        bool HasIdExisted(int moduleId);
        int GetListOrder();
    }
}
