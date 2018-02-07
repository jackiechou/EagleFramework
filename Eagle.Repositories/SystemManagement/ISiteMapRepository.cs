using System.Collections.Generic;
using Eagle.Entities.Common;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface ISiteMapRepository : IRepositoryBase<SiteMap>
    {
        IEnumerable<SiteMap> GetAllChildrenNodesOfSelectedNode(int id, bool? status = null);
        IEnumerable<SiteMap> GetAllParentNodesOfSelectedNode(int id, bool? status = null);
        IEnumerable<TreeEntity> GetSiteMapSelectTree(bool? status, int? selectedId, bool? isRootShowed = false);
        SiteMap GetDetail(string controller, string action);
        bool HasDataExisted(string name, int? parentId);
        bool HasDataExisted(string controller, string action, int? parentId);
        bool HasChild(int id);
        int GetNewListOrder();
    }
}