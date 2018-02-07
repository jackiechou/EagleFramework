using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Skins;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Skins
{
    public interface ISkinPackageBackgroundRepository : IRepositoryBase<SkinPackageBackground>
    {
        IEnumerable<SkinPackageBackground> GetSkinPackageBackgrounds(int? packageId, bool? status, out int recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<SkinPackageBackground> GetListBySkinPackageIdWithQty(int qty, int skinPackageId);
        IEnumerable<SkinPackageBackground> GetActiveListByQty(int skinPackageId, int? recordCount);
        SkinPackageBackgroundInfo GetDetail(int backgroundId);
        int GetNewListOrder();
        SkinPackageBackground GetCurrentBackground(int currentIdx);
        SkinPackageBackground GetPreviousBackground(int currentIdx);
        SkinPackageBackground GetNextBackground(int currentIdx);
        bool HasDataExists(int packageId, string backgroundName);
        SelectList PopulateSkinPackageBackgroundStatus(bool? selectedValue = null, bool? isShowSelectText = true);
    }
}
