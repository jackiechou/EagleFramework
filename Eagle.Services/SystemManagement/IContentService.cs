using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Services.Dtos.SystemManagement;

namespace Eagle.Services.SystemManagement
{
    public interface IContentService: IBaseService
    {
        #region Content Type----------------------------------------------------------------------------------------------

        IEnumerable<ContentTypeDetail> GetContentTypes(out int recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);

        ContentTypeDetail GetContentTypeDetails(int id);
        void InsertContentType(ContentTypeEntry entry);
        void UpdateContentType(int id, ContentTypeEntry entry);
        void DeleteContentType(int id);
        #endregion ----------------------------------------------------------------------------------------------


        #region Content Items----------------------------------------------------------------------------------------------

        IEnumerable<ContentItemDetail> GetContentItems(int contentTypeId);
        SelectList PopulateContentItemsByPageToDropDownList(string selectedValue, bool isShowSelectText = false);
        SelectList PopulateContentItemsByModuleToDropDownList(string selectedValue, bool isShowSelectText = false);
        ContentItemDetail GetContentItemDetails(int id);
        void InsertContentItem(ContentItemEntry entry);
        void UpdateContentItem(int id, ContentItemEntry entry);
        void DeleteContentItem(int id);
        #endregion ------------------------------------------------------------------------------------------------------

    }
}
