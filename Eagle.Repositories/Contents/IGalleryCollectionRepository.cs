using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Galleries;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IGalleryCollectionRepository : IRepositoryBase<GalleryCollection>
    {
        IEnumerable<GalleryCollectionInfo> GetGalleryCollections(string collectionName, int? topicId, GalleryCollectionStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<GalleryCollectionInfo> GetGalleryCollections(string collectionName, int? topicId, GalleryCollectionStatus? status = null);
        GalleryCollectionInfo GetLatestGalleryCollection();
        GalleryCollectionInfo GetDetail(int collectionId);
        List<SelectListItem> GetGalleryCollectionSelectList(int topicId, GalleryCollectionStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false);
        SelectList PopulateGalleryCollectionSelectList(int topicId, GalleryCollectionStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false);

        int GetNewListOrder();
        bool HasDataExisted(string collectionName);
    }
}
