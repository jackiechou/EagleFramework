using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Contents.Banners;

namespace Eagle.Services.Contents
{
    public interface IBannerService : IBaseService
    {
        #region Banner Position

        IEnumerable<BannerPositionDetail> GetBannerPositions(BannerPositionStatus? status, ref int? recordCount,
            int? page, int? pageSize);
        IEnumerable<BannerPositionDetail> GetBannerPositions(BannerPositionStatus? status);
        BannerPositionDetail GetBannerPositionDetail(int id);
        SelectList PoplulateBannerPositionSelectList(bool? isShowSelectText=null, int? selectedValue=null);

        MultiSelectList PopulateBannerPositionMultiSelectList(int bannerId, bool? isShowSelectText = null, BannerPositionStatus? status = null);
        BannerPositionDetail InsertBannerPosition(BannerPositionEntry entity);
        void UpdateBannerPosition(BannerPositionEditEntry entry);
        void UpdateBannerPositionStatus(int id, BannerPositionStatus status);
        void UpdateBannerPositionListOrder(int id, int listOrder);
        void DeleteBannerPosition(int id);

        #endregion

        #region Banner Scope
        IEnumerable<BannerScopeDetail> GetBannerScopes(ref int? recordCount, int? page, int? pageSize);
        IEnumerable<BannerScopeDetail> GetActiveBannerScopes();
        BannerScopeDetail GetBannerScopeDetails(int id);
        SelectList PopulateBannerScopeSelectList(int? selectedValue, bool? isShowSelectText);
        BannerScopeDetail InsertBannerScope(BannerScopeEntry entry);
        void UpdateBannerScope(BannerScopeEditEntry entry);
        void UpdateBannerScopeStatus(int id, BannerScopeStatus status);
        void DeleteBannerScope(int id);
        #endregion

        #region Banner Type
        IEnumerable<BannerTypeDetail> GetBannerTypes(ref int? recordCount, int? page, int? pageSize);
        IEnumerable<BannerTypeDetail> GetActiveBannerTypes();
        BannerTypeDetail GetBannerTypeDetails(int id);
        SelectList PopulateBannerTypeSelectList(int? selectedValue, bool? isShowSelectText);
        BannerTypeDetail InsertBannerType(BannerTypeEntry entry);
        void UpdateBannerType(BannerTypeEditEntry entry);
        void UpdateBannerTypeStatus(int id, BannerTypeStatus status);
        void DeleteBannerType(int id);
        #endregion

        #region Banner Page

        IEnumerable<BannerPageDetail> GetBannerPagesByBannerId(int bannerId);
        IEnumerable<BannerPageDetail> GetBannerPagesByPageId(int pageId);
        MultiSelectList PopulateBannerPageMultiSelectList(int bannerId);
        void CreateBannerPages(int bannerId, List<int> pageIds);
        void DeleteBannerPages(int bannerId, List<int> pageIds);
        #endregion


        #region Banner Zone

        IEnumerable<BannerZoneInfoDetail> GetBannerZonesByBannerId(int bannerId, BannerStatus? status = null);
        IEnumerable<BannerZoneDetail> GetBannerZonesByPositionId(int positionId, BannerStatus? status = null);

        #endregion


        #region Banner

        SelectList PopulateLinkTargets(string selectedValue, bool isShowSelectText = false);
        IEnumerable<BannerInfoDetail> Search(int vendorId, string languageCode, BannerSearchEntry entry, out int recordCount, string order = null, int? page = null, int? pageSize = null);
        IEnumerable<BannerInfoDetail> GetBanners(int vendorId, BannerTypeSetting type, BannerPositionSetting position, int? quantity, BannerStatus? status);
        BannerDetail GetBannerDetails(int id);
        BannerDetail Insert(Guid applicationId, Guid userId, int vendorId, string languageCode, BannerEntry entry);
        void Update(Guid applicationId, Guid userId, string languageCode, BannerEditEntry entry);
        void UpdateBannerStatus(Guid userId, int id, BannerStatus status);
        void DeleteBanner(int id);

        #endregion
    }
}
