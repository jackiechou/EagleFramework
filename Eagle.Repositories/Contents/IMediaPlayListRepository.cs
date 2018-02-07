using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IMediaPlayListRepository : IRepositoryBase<MediaPlayList>
    {
        IEnumerable<MediaPlayListInfo> GetMediaPlayLists(int? typeId, int? topicId, MediaPlayListStatus? status);

        IEnumerable<MediaPlayListInfo> GetMediaPlayLists(string searchText, int? typeId, int? topicId, MediaPlayListStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        MediaPlayListInfo GeDetails(int id);
        int GetNewListOrder();
        bool HasDataExisted(string playListName);

        SelectList PopulateMediaPlayListSelectList(int? typeId, int? topicId, MediaPlayListStatus? status = null, bool ? isShowSelectText = null, int? selectedValue = null);

        MultiSelectList PoplulateMediaPlayListMultiSelectList(int typeId, int topicId, MediaPlayListStatus? status = null, bool? isShowSelectText = null,
            int[] selectedValues = null);
    }
}
