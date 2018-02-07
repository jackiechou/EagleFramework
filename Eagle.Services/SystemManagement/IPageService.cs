using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;

namespace Eagle.Services.SystemManagement
{
    public interface IPageService : IBaseService
    {
        #region PAGE  ======================================================================================

        SelectList PopulatePageSelectList(PageType? pageTypeId, PageStatus? status=null, int? selectedValue=null,
            bool? isShowSelectText = false);
        string LoadPageList(PageType pageTypeId);
        IEnumerable<PageDetail> GetListByPageTypeId(PageType pageTypeId, PageStatus? status);

        IEnumerable<PageDetail> GetListByPageTypeIdAndKeywords(string keywords, PageType? pageTypeId,
            PageStatus? status);
        IEnumerable<PageDetail> Search(string keywords, PageType? pageTypeId, PageStatus? status, out int recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<PageTree> GetPageTree(PageType pageTypeId);
        AutoCompleteDetail GetAutoCompleteDetails(int id);
        Select2PagedResult GetAutoCompletePages(out int recordCount, string searchTerm, PageType? pageTypeId, int? page);
        PageDetail GetDetails(int id);
        void Insert(Guid applicationId, Guid userId, Guid roleId, string languageCode, PageEntry entry);
        void Update(Guid userId, PageEditEntry entry);
        void UpdateStatus(Guid userId, int id, PageStatus status);
        void UpdateListOrder(Guid userId, int id, int listOrder);
        void Delete(int pageId);
        #endregion ====================================================================================================

        #region PAGE TYPE ======================================================================================
        SelectList PopulatePageTypeSelectList(int? selectedValue, bool isShowSelectText = false);
        #endregion ====================================================================================================

        #region PAGE MODULES

        IEnumerable<PageModuleDetail> GetPagePagesByModuleId(int moduleId, bool? isVisible);
        IEnumerable<PageModuleDetail> GetPageModulesByPage(int pageId, bool? isVisible);

        MultiSelectList PopulatePageMultiSelectList(PageType pageTypeId, PageStatus? status = null,
            List<string> selectedValues = null);
        MultiSelectList PopulatePageMultiSelectList(PageType pageTypeId, PageStatus? status=null, int? moduleId = null, List<string> selectedValues =null);
        MultiSelectList PopulatePageByModuleIdMultiSelectList(int moduleId, bool? isVisible = null, string[] selectedValues=null);

        MultiSelectList PopulatePagesByModuleIdMultiSelectList(int moduleId, PageType? pageTypeId = null, bool? isVisible = null,
            string[] selectedValues = null);
        MultiSelectList PopulateModulesByPageMultiSelectList(int? pageId, ModuleType? moduleTypeId = null,
            string[] selectedValues = null);
        SelectList PopulateModulesByPage(int? pageId, ModuleType? moduleTypeId = null, string selectedValue = null, bool? isShowSelectText = false);
        void InsertPageModule(PageModuleEntry entry);
        void UpdatePageModule(int id, PageModuleEntry entry);
        void UpdatePageModules(List<PageModuleEntry> modulesOnPage);
        void UpdatePageModuleVisible(int pageId, bool isVisible);
        void UpdateModuleOrder(int pageId, int moduleId, int moduleOrder);
        void DeletePageModule(int pageModuleId);
        void DeletePageModuleByPageId(int pageId);

        #endregion
    }
}
