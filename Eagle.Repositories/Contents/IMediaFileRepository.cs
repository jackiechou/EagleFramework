using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IMediaFileRepository : IRepositoryBase<MediaFile>
    {
        IEnumerable<MediaFileInfo> GetMediaFiles(int? typeId, int? topicId, DocumentFileStatus? status);
        IEnumerable<MediaFileInfo> GetMediaFiles(string searchText, int? typeId, int? topicId, DocumentFileStatus? status,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        MediaFileInfo GetDetails(int mediaId);
        MediaFileInfo GetDetailsByFileId(int fileId);
        int GetNewListOrder();
        SelectList PopulateMediaFileTypes(string selectedValue = null, bool? isShowSelectText = false);
    }
}
