using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Services.Events;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Event
{
    public interface IEventTypeRepository : IRepositoryBase<EventType>
    {
        IEnumerable<TreeEntity> GetEventTypeSelectTree(EventTypeStatus? status, int? selectedId,
            bool? isRootShowed = false);

        IEnumerable<EventType> GetAllChildrenNodesOfSelectedNode(int id, EventTypeStatus? status = null);
        bool HasDataExisted(string name, int? parentId);
        bool HasChild(int typeId);

    }
}
