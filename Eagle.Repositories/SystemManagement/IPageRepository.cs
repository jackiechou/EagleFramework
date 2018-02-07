using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IPageRepository : IRepositoryBase<Page>
    {
        IEnumerable<Page> GetList(PageStatus? status);
        IEnumerable<Page> GetListByPageTypeIdAndKeywords(string keywords, PageType? pageTypeId, PageStatus? status);
        IEnumerable<Page> GetListByPageTypeId(PageType pageTypeId, PageStatus? status);
        IEnumerable<AutoComplete> GetAutoCompleteList(out int recordCount, string searchTerm, PageType? pageTypeId, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<Page> Search(string keywords, PageType? pageTypeId, PageStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        Page GetDetails(int id);
        SelectList PopulatePageTypeSelectList(int? selectedValue, bool isShowSelectText = false);

        MultiSelectList PopulatePageMultiSelectList(PageType pageTypeId, PageStatus? status = null,
            List<string> selectedValues = null);
        MultiSelectList PopulatePageMultiSelectList(PageType pageTypeId, PageStatus? status = null, int? moduleId = null, List<string> selectedValues = null);
        SelectList PopulatePageSelectList(PageType? pageTypeId, int? selectedValue = null,
            bool isShowSelectText = false);
        SelectList PopulateActivePageSelectList(PageType? pageTypeId, string languageCode, int? selectedValue = null,
            bool isShowSelectText = false);
        SelectList PopulatePageSelectListByPageTypeId(PageType? pageTypeId, PageStatus? status, int? selectedValue = null,
          bool? isShowSelectText = false);

        bool HasPageNameExisted(string pageName);
        bool HasPageTitleExisted(string pageTitle);
        bool HasPageCodeExisted(string pageCode);
        int GetNewListOrder();
    }
}
