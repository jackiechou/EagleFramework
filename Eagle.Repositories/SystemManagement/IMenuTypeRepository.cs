using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IMenuTypeRepository : IRepositoryBase<MenuType>
    {
        SelectList PopulateMenuTypeSelectList(bool? isActive=null, int? selectedValue=null, bool? isShowSelectText = false);
    }
}
