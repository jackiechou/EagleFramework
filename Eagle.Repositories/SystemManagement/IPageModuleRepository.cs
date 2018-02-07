using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IPageModuleRepository : IRepositoryBase<PageModule>
    {
        IEnumerable<PageModuleInfo> GetListByPageId(string keywords, int? pageId, ModuleType? moduleTypeId = ModuleType.Admin);
        IEnumerable<PageModule> GetPageModulesByPageId(int pageId, bool? isVisible);
        IEnumerable<PageModule> GetPageModulesByModuleId(int moduleId, bool? isVisible);
        IEnumerable<Module> GetModules(int pageId, bool? isVisible);
        PageModule GetDetailsByPageIdModuleId(int pageId, int moduleId);
        MultiSelectList PopulatePageByModuleIdMultiSelectList(int moduleId, bool? isVisible, string[] selectedValues);

        MultiSelectList PopulatePagesByModuleIdMultiSelectList(int moduleId, PageType? pageTypeId, bool? isVisible,
            string[] selectedValues);
        MultiSelectList PopulateModulesByPageMultiSelectList(int? pageId, ModuleType? moduleTypeId, string[] selectedValues);
        SelectList PopulateModulesByPage(int? pageId, ModuleType? moduleTypeId, string selectedValue, bool? isShowSelectText = false);
        bool HasDataExisted(int pageId, int moduleId);
        int GetNewModuleOrder();
        PageModuleInfo Details(int id);
    }
}
