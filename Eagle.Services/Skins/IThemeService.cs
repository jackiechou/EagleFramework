using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Services.Dtos.Skins;

namespace Eagle.Services.Skins
{
    public interface IThemeService : IBaseService
    {

        #region Skin Package
        ThemeDetail GetSelectedTheme(Guid applicationId);
        IEnumerable<SkinPackageInfoDetail> GetSkinPackages(Guid applicationId, SkinPackageSearchEntry entry,
            out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<SkinPackageDetail> GetSkinPackages(Guid applicationId, SkinPackageSearchEntry entry);
        SkinPackageInfoDetail GetSkinPackageDetail(int packageId);
        SelectList PopulateSkinPackageSelectList(Guid applicationId, int? typeId, bool? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);
        SelectList PopulateSkinPackageStatus(bool? selectedValue = null, bool? isShowSelectText = true);
        bool HasDataExisted(Guid applicationId, string skinPackageName);
        void InsertSkinPackage(Guid applicationId, SkinPackageEntry entry);
        void UpdateSkinPackage(Guid applicationId, SkinPackageEditEntry entry);
        void UpdateSkinPackageStatus(int packageId, bool status);
        void UpdateSelectedSkin(Guid applicationId, int id);
        #endregion

        #region Skin Package Background

        IEnumerable<SkinPackageBackgroundDetail> GetSkinPackageBackgrounds(SkinPackageBackgroundSearchEntry entry,
            out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        SkinPackageBackgroundInfoDetail GetSkinPackageBackgroundDetail(int backgroundId);
        SelectList PopulateSkinPackageBackgroundStatus(bool? selectedValue = null, bool? isShowSelectText = true);
        void InsertSkinPackageBackground(Guid applicationId, Guid userId, SkinPackageBackgroundEntry entry);
        void UpdateSkinPackageBackground(Guid applicationId, Guid userId, SkinPackageBackgroundEditEntry entry);
        void UpdateSkinPackageBackgroundStatus(int skinPackageBackgroundId, bool status);
        void UpdateSkinPackageBackgroundSortKey(int skinPackageBackgroundId, int listOrder);
        void UpdateSortKeyUpDown(int signal, int currentIdx);
        void DeleteSkinPackageBackground(int skinPackageBackgroundId, string dirPath);
        #endregion

        #region Skin Package Template
        IEnumerable<SkinPackageTemplateInfoDetail> GetSkinPackageTemplates(SkinPackageTemplateSearchEntry entry,
          out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<SkinPackageTemplateDetail> GetSkinPackageTemplatesBySelectedSkin();
        string GetTemplateSrcByPageId(int pageId);
        SelectList PopulateTemplateSelectList(int packageId, bool? status = null, string selectedValue = null, bool? isShowSelectText = true);
        SelectList PopulateTemplateSelectListBySelectedSkin(string selectedValue = null, bool isShowSelectText = true);
        SkinPackageTemplateInfoDetail GetSkinPackageTemplateDetail(int templateId);
        void InsertSkinPackageTemplate(SkinPackageTemplateEntry entry);
        void UpdateSkinPackageTemplate(SkinPackageTemplateEditEntry entry);
        void UpdateSkinPackageTemplateStatus(int templateId, bool status);
        #endregion

        #region Skin Package Type

        IEnumerable<SkinPackageTypeDetail> GetSkinPackageTypes(bool? status);

        SkinPackageTypeDetail GetSkinPackageTypeDetail(int id);

        SelectList PopulateSkinPackageTypeSelectList(int? selectedValue = null, bool? isShowSelectText = true);

        SelectList PopulateSkinPackageTypeStatus(bool? selectedValue = null, bool? isShowSelectText = true);

        SkinPackageTypeDetail InsertSkinPackageType(SkinPackageTypeEntry entry);

        void UpdateSkinPackageType(SkinPackageTypeEditEntry entry);

        void UpdateSkinPackageTypeStatus(int id, bool status);

        #endregion


    }
}
