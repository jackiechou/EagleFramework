using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Contents.Media;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class SiteMapRepository: RepositoryBase<SiteMap>, ISiteMapRepository
    {
        public SiteMapRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<SiteMap> Search(bool? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var result = from p in DataContext.Get<SiteMap>()
                         where status == null || p.Status == status
                         select p;
            return result.WithRecordCount(out recordCount)
               .WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<SiteMap> GetAllChildrenNodesOfSelectedNode(int id, bool? status = null)
        {
            const string strCommand = @"EXEC dbo.SiteMap_GetAllChildrenNodesOfSelectedNode @id = {0}, @status = {1}";
            return DataContext.Get<SiteMap>(strCommand, id, status);
        }

        public IEnumerable<SiteMap> GetAllParentNodesOfSelectedNode(int id, bool? status = null)
        {
            const string strCommand = @"EXEC dbo.SiteMap_GetAllParentNodesOfSelectedNode @id = {0}, @status = {1}";
            return DataContext.Get<SiteMap>(strCommand, id, status);
        }

        public SiteMap GetDetail(string controller, string action)
        {
            return DataContext.Get<SiteMap>().FirstOrDefault(c => c.Controller.ToLower() == controller.ToLower() && c.Action.ToLower() == action.ToLower());
        }

        #region SELECT TREE
        public IEnumerable<TreeEntity> GetSiteMapSelectTree(bool? status, int? selectedId, bool? isRootShowed = false)
        {
            var list = (from p in DataContext.Get<SiteMap>()
                        where status == null || p.Status == status
                        orderby p.Priority ascending
                        select new TreeEntity
                        {
                            id = p.SiteMapId,
                            key = p.SiteMapId,
                            parentid = p.ParentId,
                            depth = p.Depth,
                            name = p.Title,
                            title = p.Title,
                            text = p.Title,
                            tooltip = p.Title,
                            hasChild = p.HasChild ?? false,
                            folder = p.HasChild ?? false,
                            lazy = p.HasChild ?? false,
                            expanded = p.HasChild ?? false,
                            selected = (selectedId != null && p.SiteMapId == selectedId),
                            state = (p.Status == true),
                        }).ToList();

            var recursiveObjects = RecursiveFillTopicSelectTree(list, 0);

            if (isRootShowed != null && isRootShowed == true)
            {
                recursiveObjects.Insert(0, new TreeEntity
                {
                    id = 0,
                    key = 0,
                    parentid = 0,
                    depth = 1,
                    name = LanguageResource.Root,
                    title = LanguageResource.Root,
                    text = LanguageResource.Root,
                    tooltip = LanguageResource.Root,
                    hasChild = recursiveObjects.Any(),
                    folder = true,
                    lazy = true,
                    expanded = true,
                    selected = (selectedId != null && selectedId == 0),
                    state = true
                });
            }
            return recursiveObjects;
        }
        private List<TreeEntity> RecursiveFillTopicSelectTree(List<TreeEntity> elements, int? parentid)
        {
            if (elements == null) return null;
            List<TreeEntity> items = new List<TreeEntity>();
            List<TreeEntity> children = elements.Where(p => p.parentid == parentid).Select(
               p => new TreeEntity
               {
                   id = p.id,
                   key = p.key,
                   parentid = p.parentid,
                   depth = p.depth,
                   name = p.name,
                   title = p.title,
                   text = p.text,
                   tooltip = p.tooltip,
                   hasChild = p.hasChild,
                   folder = p.folder,
                   lazy = p.lazy,
                   expanded = p.expanded,
                   selected = p.selected,
                   state = p.state
               }).ToList();

            if (children.Count > 0)
            {
                items.AddRange(children.Select(child => new TreeEntity
                {
                    id = child.id,
                    key = child.key,
                    parentid = child.parentid,
                    depth = child.depth,
                    name = child.name,
                    title = child.title,
                    text = child.text,
                    tooltip = child.tooltip,
                    hasChild = child.hasChild,
                    folder = child.folder,
                    lazy = child.lazy,
                    expanded = child.expanded,
                    selected = child.selected,
                    state = child.state,
                    children = RecursiveFillTopicSelectTree(elements, child.id)
                }));
            }
            return items;
        }

        #endregion

        public bool HasDataExisted(string name, int? parentId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var query = DataContext.Get<SiteMap>().FirstOrDefault(c => c.ParentId == parentId &&
                c.Title.Equals(name, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }

        public bool HasDataExisted(string controller, string action, int? parentId)
        {
            if (string.IsNullOrEmpty(controller) || string.IsNullOrEmpty(action)) return false;
            var query = DataContext.Get<SiteMap>().FirstOrDefault(c => c.ParentId == parentId &&
                c.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase)
                && c.Action.Equals(action, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }

        public bool HasChild(int id)
        {
            var query = DataContext.Get<SiteMap>().FirstOrDefault(c => c.ParentId == id);
            return (query != null);
        }

        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<SiteMap>() select u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
    }
}
