using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Skins;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Skins
{
    public interface ISkinPackageTemplateRepository : IRepositoryBase<SkinPackageTemplate>
    {
        IEnumerable<SkinPackageTemplateInfo> GetSkinPackageTemplates(int? packageId, bool? status, out int recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<SkinPackageTemplate> GetListByPackageId(int skinPackageId);
        IEnumerable<SkinPackageTemplate> GetListBySelectedSkin();
        SkinPackageTemplateInfo GetDetail(int templateId);
        SelectList PopulateTemplateSelectList(int packageId, bool? status = null, string selectedValue = null, bool? isShowSelectText = true);
        SkinPackageTemplate GetLayoutInfoByPageId(int? pageId);
        string GetTemplateSrcByPageId(int? pageId);
        SelectList PopulateTemplateSelectListBySelectedSkin(string selectedValue = null, bool isShowSelectText = true);
        bool HasDataExists(int packageId, string templateName);
    }
}