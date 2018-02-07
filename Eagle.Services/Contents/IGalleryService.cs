using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Extension;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Galleries;

namespace Eagle.Services.Contents
{
    public interface IGalleryService : IBaseService
    {
        #region Gallery Topic

        IEnumerable<TreeNode> GetGalleryTopicTreeNode(GalleryTopicStatus? status, int? selectedId,
            bool? isRootShowed = false);
        IEnumerable<TreeGrid> GetGalleryTopicTreeGrid(GalleryTopicStatus? status, int? selectedId,
            bool? isRootShowed);

        IEnumerable<TreeDetail> GetGalleryTopicSelectTree(GalleryTopicStatus? status, int? selectedId,
            bool? isRootShowed = false);

        GalleryTopicDetail GetGalleryTopicDetail(int id);
        void InsertGalleryTopic(Guid userId, GalleryTopicEntry entry);
        void UpdateGalleryTopic(Guid userId, GalleryTopicEditEntry entry);
        void UpdateGalleryTopicStatus(Guid userId, int id, GalleryTopicStatus status);
        #endregion

        #region Gallery Collection

        IEnumerable<GalleryCollectionInfoDetail> Search(GalleryCollectionSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        List<GalleryCollectionInfoDetail> SearchGalleryCollections(GallerySearchEntry filter);
        List<GalleryCollectionInfoDetail> GetGalleryCollections(GalleryFileSearchEntry filter);
        IEnumerable<GalleryCollectionInfoDetail> GetGalleryCollections(int? topicId, GalleryCollectionStatus? status = null);
        GalleryCollectionInfoDetail GetLatestGalleryCollection();
        List<SelectListItem> GetGalleryCollectionSelectList(int topicId, GalleryCollectionStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false);
        SelectList PoplulateGalleryCollectionSelectList(int topicId, GalleryCollectionStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = false);
        GalleryCollectionDetail GetGalleryCollectionDetail(int id);
        GalleryCollectionDetail InsertGalleryCollection(Guid userId, GalleryCollectionEntry entry);
        void UpdateGalleryCollection(Guid userId, GalleryCollectionEditEntry entry);
        void UpdateGalleryCollectionStatus(Guid userId, int id, GalleryCollectionStatus status);
        void UpdateGalleryCollectionListOrder(Guid userId, int id, int listOrder);
        void DeleteGalleryCollection(Guid userId, int id);

        #endregion

        #region Gallery File

        IEnumerable<GalleryFileInfoDetail> GetGalleryFiles(int? collectionId, GalleryFileStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<GalleryFileInfoDetail> GetGalleryFiles(int? collectionId, GalleryFileStatus? status = null);
        List<GalleryFileInfoDetail> GetGalleryFilesFromLatestCollection(GalleryFileStatus? status = null);
        GalleryFileDetail GetGalleryFileDetail(int collectionId, int fileId);
        void InsertGalleryFile(Guid applicationId, Guid userId, GalleryFileEntry entry);
        void UpdateGalleryFile(Guid applicationId, Guid userId, GalleryFileEditEntry entry);
        void UpdateGalleryFileStatus(Guid applicationId, Guid userId, int collectionId, int fileId, bool status);
        void UpdateGalleryFileListOrder(Guid userId, int collectionId, int fileId, int listOrder);
        void DeleteGalleryFile(int collectionId, int fileId);


        #endregion
    }
}
