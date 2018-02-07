using System.Collections.Generic;
using Eagle.Core.Extension;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Contents.Galleries;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IGalleryTopicRepository : IRepositoryBase<GalleryTopic>
    {
        IEnumerable<TreeNode> GetGalleryTopicTreeNode(GalleryTopicStatus? status, int? selectedId,
            bool? isRootShowed = false);
        IEnumerable<TreeGrid> GetGalleryTopicTreeGrid(GalleryTopicStatus? status, int? selectedId,
            bool? isRootShowed = false);
        IEnumerable<TreeEntity> GetGalleryTopicSelectTree(GalleryTopicStatus? status, int? selectedId,
            bool? isRootShowed = false);
        IEnumerable<GalleryTopicInfo> GetGalleryTopicTree(GalleryTopicStatus? status, bool? isRootShowed = false);
        IEnumerable<GalleryTopic> Search(GalleryTopicStatus? status, out int recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<GalleryTopic> GetAllParentNodesOfSelectedNode(int? id, GalleryTopicStatus? status = null);
        IEnumerable<GalleryTopicInfo> GetAllChildrenNodesOfSelectedNode(int? id, GalleryTopicStatus? status = null);
        IEnumerable<GalleryTopicTree> GetHierachicalList(int? id, GalleryTopicStatus? status = null);
        IEnumerable<GalleryTopic> GetAllChildrenNodesOfSelectedNode(int topicId, GalleryTopicStatus? status);
        GalleryTopic GetDetailByCode(string topicCode);
        GalleryTopic GetNextTopic(int currentTopicId);
        GalleryTopic GetPreviousTopic(int currentTopicId);
        int GetNewListOrder();
        bool HasTopicNameExisted(string name, int? parentId);
        bool HasTopicCodeExisted(string topicCode);
        bool HasChild(int categoryId);
    }
}
