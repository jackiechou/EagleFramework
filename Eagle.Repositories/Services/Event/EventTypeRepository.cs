using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Services.Events;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Services.Event
{
    public class EventTypeRepository : RepositoryBase<EventType>, IEventTypeRepository
    {
        public EventTypeRepository(IDataContext dataContext) : base(dataContext) { }

        #region SELECT TREE
        public IEnumerable<TreeEntity> GetEventTypeSelectTree(EventTypeStatus? status, int? selectedId, bool? isRootShowed = false)
        {
            var list = (from p in DataContext.Get<EventType>()
                        where status == null || p.Status == status
                        orderby p.ListOrder ascending
                        select new TreeEntity
                        {
                            id = p.TypeId,
                            key = p.TypeId,
                            parentid = p.ParentId,
                            depth = p.Depth,
                            name = p.TypeName,
                            title = p.TypeName,
                            text = p.TypeName,
                            tooltip = p.TypeName,
                            hasChild = p.HasChild ?? false,
                            folder = p.HasChild ?? false,
                            lazy = p.HasChild ?? false,
                            expanded = p.HasChild ?? false,
                            selected = (selectedId != null && p.TypeId == selectedId),
                            state = (p.Status == EventTypeStatus.Active),
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

        public IEnumerable<EventType> GetAllChildrenNodesOfSelectedNode(int id, EventTypeStatus? status = null)
        {
            const string strCommand = @"EXEC dbo.EventType_GetAllChildrenNodesOfSelectedNode @id = {0}, @status = {1}";
            return DataContext.Get<EventType>(strCommand, id, status);
        }


        public bool HasDataExisted(string name, int? parentId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var query = DataContext.Get<EventType>().FirstOrDefault(c => c.ParentId == parentId &&
                c.TypeName.Equals(name, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }
        public bool HasChild(int typeId)
        {
            var query = DataContext.Get<EventType>().FirstOrDefault(c => c.ParentId == typeId);
            return (query != null);
        }
    }
}
