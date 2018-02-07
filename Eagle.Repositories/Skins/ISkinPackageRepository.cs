using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Skins;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Skins
{
    public interface ISkinPackageRepository: IRepositoryBase<SkinPackage>
    {
        Theme GetSelectedTheme(Guid applicationId);
        IEnumerable<SkinPackageInfo> GetSkinPackages(Guid applicationId, int? typeId, bool? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<SkinPackage> GetSkinPackages(Guid applicationId, int? typeId, bool? status=null);
        SkinPackageInfo GetDetail(int packageId);
        string GetSkinSrcBySkinId(int packageId);
        string GetCssWithTheme(ViewContext viewContext);
        bool HasDataExisted(Guid applicationId, string skinPackageName);

        SelectList PopulateSkinPackageSelectList(Guid applicationId, int? typeId, bool? status = null, int? selectedValue = null, bool? isShowSelectText = true);
        SelectList PopulateSkinPackageStatus(bool? selectedValue = null, bool? isShowSelectText = true);
    }
}
