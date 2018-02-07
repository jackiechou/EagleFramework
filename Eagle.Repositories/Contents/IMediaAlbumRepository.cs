using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IMediaAlbumRepository: IRepositoryBase<MediaAlbum>
    {
        IEnumerable<MediaAlbumInfo> GetAlbums(string albumName, int? typeId, int? topicId, MediaAlbumStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<MediaAlbumInfo> GetAlbums(int? typeId, int? topicId, MediaAlbumStatus? status);
        MediaAlbumInfo GeDetails(int id);
        int GetNewListOrder();
        int GetNewTotalView();
        MultiSelectList PoplulateMediaAlbumMultiSelectList(int typeId, int topicId, MediaAlbumStatus? status = null, bool? isShowSelectText = null,
          int[] selectedValues = null);
    }
}
