using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Skins;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Skins
{
    public interface ISkinPackageTypeRepository : IRepositoryBase<SkinPackageType>
    {
        IEnumerable<SkinPackageType> GetSkinPackageTypes(bool? status);
        bool HasDataExists(string typeName);
        SelectList PopulateSkinPackageTypeSelectList(int? selectedValue = null, bool? isShowSelectText = true);
        SelectList PopulateSkinPackageTypeStatus(bool? selectedValue = null, bool? isShowSelectText = true);
    }
}
