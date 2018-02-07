using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Extension;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Media;

namespace Eagle.Services.Contents
{
    public interface IMediaService : IBaseService
    {
        #region Album

        IEnumerable<MediaAlbumInfoDetail> GetAlbums(MediaAlbumSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<MediaAlbumInfoDetail> GetAlbums(int? typeId, int? topicId, MediaAlbumStatus? status);
        MultiSelectList PoplulateMediaAlbumMultiSelectList(int typeId, int topicId,
            MediaAlbumStatus? status = null, bool? isShowSelectText = null, int[] selectedValues = null);
        MediaAlbumInfoDetail GetAlbumDetail(int id);
        MediaAlbumDetail InsertAlbum(Guid applicationId, Guid userId, MediaAlbumEntry entry);
        void UpdateAlbum(Guid applicationId, Guid userId, MediaAlbumEditEntry entry);
        void UpdateAlbumStatus(Guid userId, int id, MediaAlbumStatus status);
        void UpdateAlbumTotalViews(Guid userId, int id);
        void UpdateAlbumListOrder(Guid userId, int id, int listOrder);

        #endregion

        #region Composer - Author

        IEnumerable<MediaComposerDetail> GetComposers(MediaComposerSearchEntry filter,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        SelectList PoplulateMediaComposerSelectList(int? selectedValue, bool? isShowSelectText = null,
            MediaComposerStatus? status = null);
        MediaComposerDetail GetComposerDetail(int id);
        MediaComposerDetail InsertComposer(Guid userId, MediaComposerEntry entry);
        void UpdateComposer(Guid userId, MediaComposerEditEntry entry);
        void UpdateComposerStatus(Guid userId, int id, MediaComposerStatus status);
        void UpdateComposerListOrder(Guid userId, int id, int listOrder);


        #endregion

        #region Artist

        IEnumerable<MediaArtistDetail> GetArtists(MediaArtistSearchEntry filter,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        MediaArtistDetail GetArtistDetail(int id);
        MediaArtistDetail InsertArtist(Guid userId, MediaArtistEntry entry);
        void UpdateArtist(Guid userId, MediaArtistEditEntry entry);
        void UpdateArtistStatus(Guid userId, int id, MediaArtistStatus status);
        void UpdateArtistListOrder(Guid userId, int id, int listOrder);


        #endregion

        #region Media File

        SelectList PopulateMediaFileTypes(string selectedValue = null, bool? isShowSelectText = false);
        List<MediaFileInfoDetail> GetMediaFiles(MediaFileSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        List<MediaFileInfoDetail> GetMediaFiles(int? typeId, int? topicId, DocumentFileStatus? status);
        List<MediaAlbumFileInfoDetail> GetMediaFilesByAlbumId(int albumId, MediaAlbumFileStatus? status = null);
        List<MediaPlayListFileInfoDetail> GetMediaFilesByPlayListId(int playListId, MediaPlayListFileStatus? status = null);
        MediaFileDetail GetMediaFileDetail(int mediaId);
        MediaFileInfoDetail GetMediaFileInfoDetail(int mediaId);
        MediaFileInfoDetail GetMediaFileInfoDetailByFileId(int fileId);
        int[] UploadVideoThumbnail(Guid applicationId, Guid? userId, int videoFileId);
        void InsertMediaFile(Guid applicationId, Guid userId, MediaFileEntry entry);
        void UpdateMediaFile(Guid applicationId, Guid userId, MediaFileEditEntry entry);
        void UpdateMediaFileStatus(int fileId, DocumentFileStatus status);
        void UpdateMediaFileListOrder(int mediaId, int listOrder);
        void DeleteMediaFile(int fileId);
        #endregion

        #region Media Topic

        IEnumerable<TreeNode> GetMediaTopicTreeNode(MediaTopicStatus? status, int? selectedId,
            bool? isRootShowed = false);

        IEnumerable<TreeGrid> GetMediaTopicTreeGrid(MediaTopicStatus? status, int? selectedId, bool? isRootShowed);

        IEnumerable<TreeDetail> GetMediaTopicSelectTree(int typeId, MediaTopicStatus? status, int? selectedId,
            bool? isRootShowed = false);

        MediaTopicDetail GetMediaTopicDetail(int id);
        void InsertMediaTopic(Guid userId, MediaTopicEntry entry);
        void UpdateMediaTopic(Guid userId, MediaTopicEditEntry entry);
        void UpdateMediaTopicStatus(Guid userId, int id, MediaTopicStatus status);
        void UpdateMediaTopicListOrder(Guid userId, int id, int listOrder);

        #endregion

        #region Media PlayList

        IEnumerable<MediaPlayListInfoDetail> GetPlayLists(MediaPlayListSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<MediaPlayListInfoDetail> GetPlayLists(int? typeId, int? topicId, MediaPlayListStatus? status);

        MultiSelectList PoplulateMediaPlayListMultiSelectList(int typeId, int topicId, MediaPlayListStatus? status = null, bool? isShowSelectText = null,
            int[] selectedValues = null);
        MediaPlayListInfoDetail GetPlayListDetail(int id);
        SelectList PoplulatePlayListSelectList(int typeId, int topicId, int? selectedValue, bool? isShowSelectText = null,
            MediaPlayListStatus? status = null);
        MediaPlayListDetail InsertPlayList(Guid applicationId, Guid userId, MediaPlayListEntry entry);

        void UpdatePlayList(Guid applicationId, Guid userId, MediaPlayListEditEntry entry);
        void UpdatePlayListStatus(Guid userId, int id, MediaPlayListStatus status);
        void UpdatePlayListListOrder(Guid userId, int id, int listOrder);
        void DeletePlayList(int id);

        #endregion

        #region Media Type

        IEnumerable<MediaTypeDetail> GetMediaTypes(MediaTypeStatus? status, ref int? recordCount, int? page,
            int? pageSize);

        IEnumerable<MediaTypeDetail> GetMediaTypes(MediaTypeStatus? status);
        MediaTypeDetail GetMediaTypeDetail(int id);

        SelectList PoplulateMediaTypeSelectList(int? selectedValue, bool? isShowSelectText = false,
            MediaTypeStatus? status = null);

        MediaTypeDetail InsertMediaType(Guid userId, MediaTypeEntry entry);
        void UpdateMediaType(Guid userId, MediaTypeEditEntry entry);
        void UpdateMediaTypeStatus(Guid userId, int id, MediaTypeStatus status);
        void UpdateMediaTypeListOrder(Guid userId, int id, int listOrder);
        void DeleteMediaType(int id);


        #endregion
    }
}
