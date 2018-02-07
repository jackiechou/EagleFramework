using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IMenuPositionRepository : IRepositoryBase<MenuPosition>
    {
        MultiSelectList PopulateMenuPositionMultiSelectList(int typeId, bool? isActive = null, string positionId = null, string[] selectedValues = null);
        MultiSelectList PopulateMenuPositionMultiSelectedList(string positionId, bool? isActive = null);
        MultiSelectList PopulateMenuPositionMultiSelectedListByMenuId(int? menuId, bool? isActive = null);
    }
}
