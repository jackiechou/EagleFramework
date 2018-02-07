using System.Collections.Generic;
using Eagle.Core.Extension;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Messaging
{
    public interface INotificationTypeRepository : IRepositoryBase<NotificationType>
    {
        #region TREE NODE

        IEnumerable<TreeNode> GetNotificationTypeTreeNode(NotificationTypeStatus? status, int? selectedId,
            bool? isRootShowed = false);
        #endregion

        #region TREE GRID

        IEnumerable<TreeGrid> GetNotificationTypeTreeGrid(NotificationTypeStatus? status, int? selectedId,
            bool? isRootShowed = false);

        #endregion

        #region SELECT TREE

        IEnumerable<TreeEntity> GetNotificationTypeSelectTree(NotificationTypeStatus? status, int? selectedId, bool? isRootShowed = true);
       

        #endregion

        #region Hierachical Tree

        IEnumerable<NotificationTypeInfo> GetNotificationTypeTree(NotificationTypeStatus? status,
            bool? isRootShowed = false);
        

        #endregion

        IEnumerable<NotificationType> Search(NotificationTypeStatus? status, out int recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<NotificationType> GetAllParentNodesOfSelectedNode(int? id,
            NotificationTypeStatus? status = null);

        IEnumerable<NotificationTypeInfo> GetAllChildrenNodesOfSelectedNode(int? id,
            NotificationTypeStatus? status = null);
        IEnumerable<NotificationTypeTree> GetHierachicalList(int? id, NotificationTypeStatus? status = null);

        IEnumerable<NotificationType> GetAllChildrenNodesOfSelectedNode(int typeId,
            NotificationTypeStatus? status);

        NotificationType GetNextTopic(int currentCategoryId);

        NotificationType GetPreviousTopic(int currentCategoryId);
        int GetNewListOrder();

        bool HasDataExisted(string name, int? parentId);
        bool HasChild(int categoryId);
    }
}
