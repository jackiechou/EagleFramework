using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Galleries;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IGalleryFileRepository : IRepositoryBase<GalleryFile>
    {
        IEnumerable<GalleryFileInfo> GetGalleryFileList(int? collectionId, GalleryFileStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<GalleryFileInfo> GetGalleryFiles(int? collectionId, GalleryFileStatus? status=null);
        GalleryFile GetDetails(int collectionId, int fileId);
        int GetNewListOrder();
        bool HasDataExisted(int collectionId, int fileId);
    }
}
