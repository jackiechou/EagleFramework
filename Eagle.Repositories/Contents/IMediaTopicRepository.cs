using System.Collections.Generic;
using Eagle.Core.Extension;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IMediaTopicRepository : IRepositoryBase<MediaTopic>
    {
        IEnumerable<TreeNode> GetMediaTopicTreeNode(MediaTopicStatus? status, int? selectedId,
            bool? isRootShowed = false);

        IEnumerable<TreeGrid> GetMediaTopicTreeGrid(MediaTopicStatus? status, int? selectedId,
            bool? isRootShowed = false);

        IEnumerable<TreeEntity> GetMediaTopicSelectTree(int typeId, MediaTopicStatus? status, int? selectedId,
            bool? isRootShowed = false);

        IEnumerable<MediaTopicInfo> GetMediaTopicTree(MediaTopicStatus? status, bool? isRootShowed = false);

        IEnumerable<MediaTopic> Search(MediaTopicStatus? status, out int recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);

        IEnumerable<MediaTopicInfo> GetAllChildrenOfSelectedNode(int? id, MediaTopicStatus? status = null);
        IEnumerable<MediaTopic> GetAllParentNodesOfSelectedNode(int id, MediaTopicStatus? status = null);
        IEnumerable<MediaTopic> GetAllChildrenNodesOfSelectedNode(int id, MediaTopicStatus? status = null);
        MediaTopic GetNextTopic(int currentTopicId);
        MediaTopic GetPreviousTopic(int currentTopicId);

        bool HasDataExisted(string name, int? parentId);
        bool HasChild(int categoryId);
    }
}