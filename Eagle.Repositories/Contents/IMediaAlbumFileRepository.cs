using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IMediaAlbumFileRepository : IRepositoryBase<MediaAlbumFile>
    {
        IEnumerable<MediaAlbumFile> GetListByAlbumId(int albumId, MediaAlbumFileStatus? status = null);
        IEnumerable<MediaAlbumFile> GetListByFileId(int fileId, MediaAlbumFileStatus? status = null);
        MediaAlbumFile GetDetails(int albumId, int fileId);
        int GetNewListOrder();
    }
}
