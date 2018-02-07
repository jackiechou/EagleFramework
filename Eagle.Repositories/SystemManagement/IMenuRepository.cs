using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IMenuRepository: IRepositoryBase<Menu>
    {
        List<int> GetListByParentId(int parentId);
        IEnumerable<MenuPage> GetParentNodesOfSelectedNodeByPagePath(string pagePath);
        IEnumerable<MenuPage> GetListByRoleIdStatus(Guid applicationId, Guid roleId, bool? isSecured, MenuStatus? status);
        IEnumerable<MenuPage> GetListByRoleIdUserIdStatus(Guid roleId, Guid userId, MenuStatus status);
        IEnumerable<MenuPage> GetParentNodesOfSelectedNodeByMenuId(int? menuId, MenuStatus? status);
        IEnumerable<MenuPage> GetParentNodesOfSelectedNodeByMenuCode(Guid menuCode);
        IEnumerable<MenuPage> GetPublishedList(Guid applicationId, int? menuTypeId, string languageCode);

        IEnumerable<MenuPage> GetListByPosition(Guid applicationId, int positionId, List<Guid> roleIds,
            MenuStatus? status);
        IEnumerable<MenuPage> GetListByStatus(Guid applicationId, int? menuTypeId, int positionId, MenuStatus? status);
        IEnumerable<MenuPage> GetListByRoles(Guid applicationId, int typeId, List<Guid> roleIds, MenuStatus? status);
        IEnumerable<MenuTree> GetList();
        IEnumerable<MenuTree> GetTreeList(int typeId, MenuStatus? status = null);
        IEnumerable<MenuTreeNode> GetHierachicalList(int typeId, MenuStatus? status = null, bool? isRootShowed = false);
        IEnumerable<Menu> GetMenuListByStatus(MenuStatus? menuStatus);
        IEnumerable<MenuPage> GetListByParentIdStatus(int parentId, MenuStatus? status);
        List<Menu> GetChildren(int menuId, MenuStatus? status=null);
        IEnumerable<Menu> GetAllChildrenNodesOfSelectedNode(int? menuId, NewsCategoryStatus? status=null);
        int GetMenuDepth(int? menuId);
        int GetNewListOrder();
        bool HasChildren(int menuId);
        SelectList PopulateIsSecuredSelectList(int? selectedValue, bool? isShowSelectText = false);
    }
}
