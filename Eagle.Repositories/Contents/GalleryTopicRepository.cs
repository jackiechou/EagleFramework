using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Common.Extensions.EnumHelper;
using Eagle.Core.Common;
using Eagle.Core.Extension;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Contents.Galleries;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Contents
{
    public class GalleryTopicRepository : RepositoryBase<GalleryTopic>, IGalleryTopicRepository
    {
        public GalleryTopicRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        #region TREE NODE
        public IEnumerable<TreeNode> GetGalleryTopicTreeNode(GalleryTopicStatus? status, int? selectedId, bool? isRootShowed = false)
        {
            var query = (from x in DataContext.Get<GalleryTopic>()
                         where status == null || x.Status == status
                         orderby x.ListOrder ascending
                         select x).AsEnumerable();

            var list = query.Select(x=> new TreeNode
                        {
                            id = x.TopicId.ToString(),
                            key = x.TopicId.ToString(),
                            parentid = x.ParentId.ToString(),
                            level = x.Depth,
                            name = x.TopicName,
                            title = x.TopicName,
                            text = x.TopicName,
                            tooltip = x.Description,
                            state = x.Status.DisplayName(),
                            haschild = x.HasChild,
                            opened = (x.HasChild != null && x.HasChild == true),
                            selected = (selectedId != null && x.TopicId == selectedId),
                            icon = (x.HasChild != null && x.HasChild == true) ? "glyphicon glyphicon-folder-close" : "glyphicon glyphicon-file"
                        }).ToList();

            var recursiveObjects = new List<TreeNode>();
            if (list.Any())
            {
                recursiveObjects = RecursiveFillTreeNode(list, "0");
                if (isRootShowed != null && isRootShowed == true)
                {
                    recursiveObjects.Insert(0, new TreeNode
                    {
                        id = "0",
                        key = "0",
                        parentid = "0",
                        level = 1,
                        name = LanguageResource.Root,
                        title = LanguageResource.Root,
                        text = LanguageResource.Root,
                        tooltip = LanguageResource.Root,
                        haschild = true,
                        opened = true,
                        selected = (selectedId != null && selectedId == 0),
                        state = "0",
                        icon = "glyphicon glyphicon-folder-open"
                    });
                }
            }
            else
            {
                recursiveObjects.Insert(0, new TreeNode
                {
                    id = "0",
                    key = "0",
                    parentid = "0",
                    level = 1,
                    name = LanguageResource.NonSpecified,
                    title = LanguageResource.Root,
                    text = LanguageResource.NonSpecified,
                    tooltip = LanguageResource.NonSpecified,
                    state = LanguageResource.Active,
                    haschild = false,
                    opened = false,
                    selected = (selectedId != null && selectedId == 0),
                    icon = "glyphicon glyphicon-file",
                });
            }

            return recursiveObjects;
        }
        private List<TreeNode> RecursiveFillTreeNode(List<TreeNode> elements, string parentid)
        {
            if (elements == null) return null;
            List<TreeNode> items = new List<TreeNode>();
            List<TreeNode> children = elements.Where(p => p.parentid == parentid).Select(
               p => new TreeNode
               {
                   id = p.id,
                   key = p.key,
                   parentid = p.parentid,
                   level = p.level,
                   name = p.name,
                   title = p.title,
                   text = p.text,
                   tooltip = p.tooltip,
                   haschild = p.haschild,
                   opened = (p.haschild != null && p.haschild == true),
                   selected = p.selected,
                   state = p.state,
                   icon = p.icon
               }).ToList();

            if (children.Count > 0)
            {
                items.AddRange(children.Select(child => new TreeNode
                {
                    id = child.id,
                    key = child.key,
                    parentid = child.parentid,
                    name = child.name,
                    title = child.title,
                    text = child.text,
                    tooltip = child.tooltip,
                    haschild = child.haschild,
                    opened = (child.haschild != null && child.haschild == true),
                    selected = child.selected,
                    state = child.state,
                    icon = child.icon,
                    children = RecursiveFillTreeNode(elements, child.id)
                }));
            }
            return items;
        }
        #endregion

        #region TREE GRID

        public IEnumerable<TreeGrid> GetGalleryTopicTreeGrid(GalleryTopicStatus? status, int? selectedId, bool? isRootShowed = false)
        {
            var list = (from x in DataContext.Get<GalleryTopic>()
                        where status == null || x.Status == status
                        orderby x.ListOrder ascending
                        select new TreeGrid
                        {
                            id = x.TopicId.ToString(),
                            parentId = x.ParentId.ToString(),
                            level = x.Depth,
                            name = x.TopicName,
                            title = x.TopicName,
                            text = x.TopicName,
                            state = new TreeGridState
                            {
                                opened = (x.HasChild != null && x.HasChild == true),
                                selected = (selectedId != null && x.TopicId == selectedId),
                            },
                            data = new TreeGridData
                            {
                                status = (x.Status == GalleryTopicStatus.InActive) ? LanguageResource.InActive : LanguageResource.Active,
                                action = @"<a data-id='" + x.TopicId + "' title=" + LanguageResource.Reset + " class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='" + x.TopicId + "' title = " + LanguageResource.Edit + " class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                            }
                        }).ToList();

            var recursiveObjects = new List<TreeGrid>();
            if (list.Any())
            {
                recursiveObjects = RecursiveFillTreeGrid(list, "0");
                if (isRootShowed != null && isRootShowed == true)
                {
                    recursiveObjects.Insert(0, new TreeGrid
                    {
                        id = "0",
                        parentId = "0",
                        level = 1,
                        name = LanguageResource.Root,
                        title = LanguageResource.Root,
                        text = LanguageResource.Root,
                        type = "glyphicon glyphicon-folder-close",
                        icon = "glyphicon glyphicon-file",
                        state = new TreeGridState
                        {
                            opened = false,
                            selected = (selectedId != null && selectedId == 0),
                        },
                        data = new TreeGridData
                        {
                            status = LanguageResource.Published,
                            action = @"<a data-id='0' title='" + LanguageResource.Reset + "' class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='0' title='" + LanguageResource.Edit + "' class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                        }
                    });
                }
            }
            else
            {
                recursiveObjects.Insert(0, new TreeGrid
                {
                    id = "0",
                    parentId = "0",
                    level = 1,
                    name = LanguageResource.NonSpecified,
                    title = LanguageResource.NonSpecified,
                    text = LanguageResource.NonSpecified,
                    type = "glyphicon glyphicon-folder-close",
                    icon = "glyphicon glyphicon-file",
                    state = new TreeGridState
                    {
                        opened = false,
                        selected = (selectedId != null && selectedId == 0),
                    },
                    data = new TreeGridData
                    {
                        status = LanguageResource.Published,
                        action = @"<a data-id='0' title='" + LanguageResource.Reset + "' class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='0' title = '" + LanguageResource.Edit + "' class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                    }
                });
            }

            return recursiveObjects;
        }
        private List<TreeGrid> RecursiveFillTreeGrid(List<TreeGrid> elements, string parentid)
        {
            if (elements == null) return null;
            List<TreeGrid> items = new List<TreeGrid>();
            List<TreeGrid> children = elements.Where(p => p.parentId == parentid).Select(
               p => new TreeGrid
               {
                   id = p.id,
                   parentId = p.parentId,
                   level = p.level,
                   name = p.name,
                   title = p.title,
                   text = p.text,
                   state = new TreeGridState
                   {
                       opened = (p.hasChild != null && p.hasChild == true),
                       selected = p.state.selected,
                   },
                   data = new TreeGridData
                   {
                       status = p.data.status,
                       action = @"<a data-id='" + p.id + "' title='" + LanguageResource.Reset + "' class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='" + p.id + "' title = '" + LanguageResource.Edit + "' class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                   },
               }).ToList();

            if (children.Count > 0)
            {
                items.AddRange(children.Select(child => new TreeGrid
                {
                    id = child.id,
                    parentId = child.parentId,
                    level = child.level,
                    name = child.name,
                    title = child.title,
                    text = child.text,
                    state = new TreeGridState
                    {
                        opened = (child.hasChild != null && child.hasChild == true),
                        selected = child.state.selected,
                    },
                    data = new TreeGridData
                    {
                        status = child.data.status,
                        action = @"<a data-id='" + child.id + "' title='" + LanguageResource.Reset + "' class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='" + child.id + "' title = '" + LanguageResource.Edit + "' class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                    },
                    children = RecursiveFillTreeGrid(elements, child.id)
                }));
            }
            return items;
        }

        #endregion

        #region SELECT TREE
        public IEnumerable<TreeEntity> GetGalleryTopicSelectTree(GalleryTopicStatus? status, int? selectedId, bool? isRootShowed = false)
        {
            var list = (from p in DataContext.Get<GalleryTopic>()
                        where status == null || p.Status == status
                        orderby p.ListOrder ascending
                        select new TreeEntity
                        {
                            id = p.TopicId,
                            key = p.TopicId,
                            parentid = p.ParentId,
                            depth = p.Depth,
                            name = p.TopicName,
                            title = p.TopicName,
                            text = p.TopicName,
                            tooltip = p.Description,
                            hasChild = p.HasChild ?? false,
                            folder = p.HasChild ?? false,
                            lazy = p.HasChild ?? false,
                            expanded = p.HasChild ?? false,
                            selected = (selectedId != null && p.TopicId == selectedId),
                            state = (p.Status == GalleryTopicStatus.Active),
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

        #region Hierachical Tree

        public IEnumerable<GalleryTopicInfo> GetGalleryTopicTree(GalleryTopicStatus? status, bool? isRootShowed = false)
        {
            var list = (from p in DataContext.Get<GalleryTopic>()
                        where status == null || p.Status == status
                        orderby p.ListOrder ascending
                        select new GalleryTopicInfo
                        {
                            TopicId = p.TopicId,
                            ParentId = p.ParentId,
                            TopicName = p.TopicName,
                            TopicAlias = p.TopicAlias,
                            TopicCode = p.TopicCode,
                            Description = p.Description,
                            Depth = p.Depth,
                            Lineage = p.Lineage,
                            HasChild = p.HasChild,
                            ListOrder = p.ListOrder,
                            Status = p.Status
                        }).ToList();

            var recursiveObjects = new List<GalleryTopicInfo>();
            if (!list.Any())
            {
                list.Insert(0, new GalleryTopicInfo
                {
                    TopicId = 0,
                    ParentId = null,
                    TopicName = LanguageResource.NonSpecified,
                    TopicCode = LanguageResource.NonSpecified,
                    TopicAlias = LanguageResource.NonSpecified,
                    Description = LanguageResource.NonSpecified,
                    Depth = 1,
                    Lineage = null,
                    HasChild = false,
                    ListOrder = 1,
                    Status = GalleryTopicStatus.Active
                });
            }
            else
            {
                recursiveObjects = RecursiveFillTree(list, 0);
                if (isRootShowed != null && isRootShowed == true)
                {
                    recursiveObjects.Insert(0, new GalleryTopicInfo
                    {
                        TopicId = 0,
                        ParentId = null,
                        TopicName = LanguageResource.Root,
                        TopicAlias = LanguageResource.Root,
                        TopicCode = LanguageResource.Root,
                        Description = LanguageResource.Root,
                        Depth = 1,
                        Lineage = null,
                        HasChild = recursiveObjects.Any(),
                        ListOrder = 1,
                        Status = GalleryTopicStatus.Active
                    });
                }
            }

            return recursiveObjects;
        }

        private List<GalleryTopicInfo> RecursiveFillTree(List<GalleryTopicInfo> list, int? parentid)
        {
            if (list == null) return null;
            List<GalleryTopicInfo> items = new List<GalleryTopicInfo>();
            List<GalleryTopicInfo> children = list.Where(p => p.ParentId == parentid).Select(
               p => new GalleryTopicInfo
               {
                   TopicId = p.TopicId,
                   ParentId = p.ParentId,
                   TopicName = p.TopicName,
                   TopicAlias = p.TopicAlias,
                   TopicCode = p.TopicCode,
                   Description = p.Description,
                   Depth = p.Depth,
                   Lineage = p.Lineage,
                   HasChild = p.HasChild,
                   ListOrder = p.ListOrder,
                   Status = p.Status
               }).ToList();

            if (children.Count > 0)
            {
                items.AddRange(children.Select(child => new GalleryTopicInfo
                {
                    TopicId = child.TopicId,
                    ParentId = child.ParentId,
                    TopicName = child.TopicName,
                    TopicAlias = child.TopicAlias,
                    TopicCode = child.TopicCode,
                    Description = child.Description,
                    Depth = child.Depth,
                    Lineage = child.Lineage,
                    HasChild = child.HasChild,
                    ListOrder = child.ListOrder,
                    Status = child.Status,
                    Children = RecursiveFillTree(list, child.TopicId)
                }));
            }
            return items;
        }

        #endregion

        public IEnumerable<GalleryTopic> Search(GalleryTopicStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var result = from p in DataContext.Get<GalleryTopic>()
                         where status == null || p.Status == status
                         select p;
            return result.OrderByDescending(t => t.ListOrder)
               .WithRecordCount(out recordCount)
               .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<GalleryTopic> GetAllParentNodesOfSelectedNode(int? id, GalleryTopicStatus? status = null)
        {
            var fullList = GetGalleryTopicTree(status).ToList();
            return (from p in DataContext.Get<GalleryTopic>()
                    join f in fullList on p.TopicId equals f.TopicId
                    where p.TopicId == id
                    select p).AsEnumerable();
        }
        public IEnumerable<GalleryTopicInfo> GetAllChildrenNodesOfSelectedNode(int? id, GalleryTopicStatus? status = null)
        {
            var lst = GetGalleryTopicTree(status);
            return lst.Where(y => y.ParentId == id)
                                      .SelectMany(x => x.Children).ToList();
        }
        public IEnumerable<GalleryTopicTree> GetHierachicalList(int? id, GalleryTopicStatus? status = null)
        {
            var query = from p in DataContext.Get<GalleryTopic>()
                        where p.ParentId == id && (status == null || p.Status == status)
                        orderby p.ListOrder
                        select new GalleryTopicTree
                        {
                            TopicId = p.TopicId,
                            ParentId = p.ParentId,
                            TopicName = p.TopicName,
                            TopicAlias = p.TopicAlias,
                            TopicCode =p.TopicCode,
                            Description = p.Description,
                            Depth = p.Depth,
                            Lineage = p.Lineage,
                            HasChild = p.HasChild,
                            ListOrder = p.ListOrder,
                            Status = p.Status,
                            Parents = (from c in DataContext.Get<GalleryTopic>()
                                       where c.TopicId == p.ParentId && (status == null || c.Status == status)
                                       select new GalleryTopicTree
                                       {
                                           TopicId = c.TopicId,
                                           ParentId = c.ParentId,
                                           TopicName = c.TopicName,
                                           TopicAlias = c.TopicAlias,
                                           TopicCode = p.TopicCode,
                                           Description = c.Description,
                                           Depth = c.Depth,
                                           Lineage = c.Lineage,
                                           HasChild = c.HasChild,
                                           ListOrder = c.ListOrder,
                                           Status = c.Status
                                       }).ToList(),
                            Children = (from c in DataContext.Get<GalleryTopic>()
                                        where c.ParentId == p.TopicId && (status == null || c.Status == status)
                                        select new GalleryTopicTree
                                        {
                                            TopicId = c.TopicId,
                                            ParentId = c.ParentId,
                                            TopicName = c.TopicName,
                                            TopicAlias = c.TopicAlias,
                                            TopicCode = p.TopicCode,
                                            Description = c.Description,
                                            Depth = c.Depth,
                                            Lineage = c.Lineage,
                                            HasChild = c.HasChild,
                                            ListOrder = c.ListOrder,
                                            Status = c.Status
                                        }).ToList()
                        };
            return query.AsEnumerable();
        }
        public IEnumerable<GalleryTopic> GetAllChildrenNodesOfSelectedNode(int topicId, GalleryTopicStatus? status)
        {
            const string strCommand = @"EXEC dbo.GalleryTopic_GetTreeNodes @topicId = {1}, @status = {2}";
            return DataContext.Get<GalleryTopic>(strCommand, topicId, status);
        }
        public GalleryTopic GetDetailByCode(string topicCode)
        {
            if (string.IsNullOrEmpty(topicCode)) return null;
            return DataContext.Get<GalleryTopic>().FirstOrDefault(c => c.TopicCode.Equals(topicCode, StringComparison.OrdinalIgnoreCase));
        }
        public GalleryTopic GetNextTopic(int currentTopicId)
        {
            return (from x in DataContext.Get<GalleryTopic>()
                    where x.TopicId > currentTopicId
                    orderby x.ListOrder descending
                    select x).FirstOrDefault();
        }
        public GalleryTopic GetPreviousTopic(int currentTopicId)
        {
            return (from x in DataContext.Get<GalleryTopic>()
                    where x.TopicId < currentTopicId
                    orderby x.ListOrder descending
                    select x).FirstOrDefault();
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<GalleryTopic>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
        public bool HasTopicNameExisted(string name, int? parentId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var query = DataContext.Get<GalleryTopic>().FirstOrDefault(c => c.ParentId == parentId &&
                c.TopicName.Equals(name, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }

        public bool HasTopicCodeExisted(string topicCode)
        {
            if (string.IsNullOrEmpty(topicCode)) return false;
            var query = DataContext.Get<GalleryTopic>().FirstOrDefault(c => c.TopicCode.Equals(topicCode, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }

        public bool HasChild(int categoryId)
        {
            var query = DataContext.Get<GalleryTopic>().FirstOrDefault(c => c.ParentId == categoryId);
            return (query != null);
        }
    }
}
