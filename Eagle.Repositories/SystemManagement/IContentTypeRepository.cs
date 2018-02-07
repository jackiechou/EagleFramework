using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IContentTypeRepository : IRepositoryBase<ContentType>
    {
        SelectList PopulateContentTypesToDropDownList(string selectedValue, bool isShowSelectText = false);
        bool HasDataExisted(string contentTypeName);
    }
}
