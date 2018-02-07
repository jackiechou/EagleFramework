using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;

namespace Eagle.Services.SystemManagement
{
    public interface IMenuService : IBaseService
    {
        #region Desktop
        string LoadDesktopMegaMenu(Guid applicationId, MenuPositionSetting position);
        string LoadDesktopBootstrapMenu(Guid applicationId, MenuPositionSetting position);
        #endregion

        SelectList GenerateMenuStatuses(int? selectedValue = null, bool? isShowSelectText = false);

        SelectList PopulateLinkTargets(string selectedValue, bool isShowSelectText = false);

        string LoadMenu(Guid applicationId, Guid userId, Guid roleId);

        IEnumerable<MenuPageDetail> GetListByPosition(Guid applicationId, Guid userId, int positionId, int? status);
        IEnumerable<MenuPageDetail> GetMenuListByStatus(Guid applicationId, int? menuTypeId, int positionId, MenuStatus? status);
        IEnumerable<MenuPageDetail> GetMenuListByParentId(int parentId, MenuStatus? status);
        IEnumerable<MenuTreeDetail> GetTreeList(int typeId, int? status=null);
        IEnumerable<MenuTreeNodeDetail> GetHierachicalList(int typeId, int? status = null, bool? isRootShowed = false);

        MenuDetail GetMenuDetails(int id);
        int Insert(Guid applicationId, Guid userId, Guid roleId, int vendorId, MenuEntry entry);
        void Update(Guid applicationId, Guid userId, Guid roleId, int vendorId, MenuEditEntry entry);
        void UpdateListOrder(int id, int? parentId, int listOrder);
        void Delete(int id);

        IEnumerable<MenuPageDetail> GetListByRoleId(int typeId, MenuStatus? status, Guid roleId);
        IEnumerable<MenuPageDetail> GetListByUserId(Guid userId, int typeId, MenuStatus? status);
        bool IsMenuAllowedAccess(Guid userId, int menuId);

        #region Site Map

        IEnumerable<TreeDetail> GetSiteMapSelectTree(bool? status, int? selectedId, bool? isRootShowed = false);
        IEnumerable<SiteMapDetail> GetSiteMap(string controller = "home", string action = "index");
        string GetSiteMapDocument(IEnumerable<SiteMapDetail> sitemapNodes);

        string PopulateSiteMapByMenuCode(string menuCode);
        string PopulateSiteMapByMenuId(string menuId);
        string PopulateSiteMap(string controller = "home", string action = "index");
        SiteMapDetail GetSiteMapDetail(int id);
        void InsertSiteMap(SiteMapEntry entry);
        void UpdateSiteMap(SiteMapEditEntry entry);
        void UpdateSiteMapStatus(int id, bool status);


        #endregion

        #region Menu Permission

        MenuRolePermissionEntry GetMenuRolePermissionEntry(Guid applicationId, int menuId);
        IEnumerable<MenuPermissionLevelDetail> GetMenuPermissionLevels(MenuPermissionLevelStatus? isActive);
        void InsertMenuPermission(MenuPermissionEntry entry);
        void UpdateMenuPermission(int id, MenuPermissionEntry entry);
        void UpdateMenuPermissionStatus(Guid roleId, int menuId, int permissionId, bool status);
        void DeleteMenuPermission(int levelId, int menuId, Guid roleId);
        void DeleteMenuPermissions(IEnumerable<MenuPermissionDetail> permissionLst);
        void DeleteMenuPermissionByRoleId(Guid roleId);

        #endregion

        #region Menu Position

        MultiSelectList PopulateMenuPositionMultiSelectList(int? typeId = null, bool? isActive = null, string positionId = null, string[] selectedValues = null);
        MultiSelectList PopulateMenuPositionMultiSelectedList(string positionId, bool? isActive = null);
        MultiSelectList PopulateMenuPositionMultiSelectedListByMenuId(int? menuId, bool? isActive = null);

        #endregion

        #region Menu Type

        SelectList PopulateMenuTypeSelectList(bool? isActive=null, int? selectedValue = null, bool? isShowSelectText = false);

        #endregion

       
    }
}
