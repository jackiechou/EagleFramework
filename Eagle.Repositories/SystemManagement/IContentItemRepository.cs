using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IContentItemRepository : IRepositoryBase<ContentItem>
    {
        IEnumerable<ContentTreeModel> GetTreeList(int contentTypeId);
        IEnumerable<ContentItem> GetList(int contentTypeId);
        SelectList PopulateContentItemsByPageToDropDownList(string selectedValue, bool isShowSelectText = false);
        SelectList PopulateContentItemsByModuleToDropDownList(string selectedValue, bool isShowSelectText = false);
        ContentItem GetDetails(int contentItemId);
        
        bool IsIdExisted(int id);
        bool HasDataExisted(int contentTypeId, string itemName);
    }
}
