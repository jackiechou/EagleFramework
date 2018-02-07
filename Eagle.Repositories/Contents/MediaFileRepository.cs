using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.Entities.SystemManagement.FileStorage;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Contents
{
    public class MediaFileRepository : RepositoryBase<MediaFile>, IMediaFileRepository
    {
        public MediaFileRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<MediaFileInfo> GetMediaFiles(int? typeId, int? topicId, DocumentFileStatus? status)
        {
            var queryable = (from f in DataContext.Get<MediaFile>()
                             join t in DataContext.Get<MediaType>() on f.TypeId equals t.TypeId into atJoin
                             from type in atJoin.DefaultIfEmpty()
                             join tp in DataContext.Get<MediaTopic>() on f.TopicId equals tp.TopicId into tpJoin
                             from topic in tpJoin.DefaultIfEmpty()
                             join c in DataContext.Get<MediaComposer>() on f.TopicId equals c.ComposerId into composertpJoin
                             from composer in composertpJoin.DefaultIfEmpty()
                             join d in DataContext.Get<DocumentFile>() on f.FileId equals d.FileId into documentJoin
                             from fd in documentJoin.DefaultIfEmpty()
                             join df in DataContext.Get<DocumentFolder>() on fd.FolderId equals df.FolderId into documentFolderJoin
                             from dff in documentFolderJoin.DefaultIfEmpty()
                             where (status == null || fd.IsActive == status)
                             orderby f.ListOrder descending
                             select new MediaFileInfo
                             {
                                 MediaId = f.MediaId,
                                 FileId = f.FileId,
                                 TypeId = f.TypeId,
                                 TopicId = f.TopicId,
                                 Artist = f.Artist,
                                 ComposerId = f.ComposerId,
                                 AutoStart = f.AutoStart,
                                 MediaLoop = f.MediaLoop,
                                 Lyric = f.Lyric,
                                 SmallPhoto = f.SmallPhoto,
                                 LargePhoto = f.LargePhoto,
                                 ListOrder = f.ListOrder,
                                 Type = type,
                                 Topic = topic,
                                 Composer = composer,
                                 DocumentFile = fd,
                                 DocumentFolder = dff
                             });

            if (typeId != null)
            {
                queryable = queryable.Where(x => x.TypeId == typeId);
            }
            if (topicId != null && topicId > 0)
            {
                queryable = queryable.Where(x => x.TopicId == topicId);
            }
            return queryable.AsEnumerable();
        }

        public IEnumerable<MediaFileInfo> GetMediaFiles(string searchText, int? typeId, int? topicId, DocumentFileStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = (from f in DataContext.Get<MediaFile>()
                             join t in DataContext.Get<MediaType>() on f.TypeId equals t.TypeId into atJoin
                             from type in atJoin.DefaultIfEmpty()
                             join tp in DataContext.Get<MediaTopic>() on f.TopicId equals tp.TopicId into tpJoin
                             from topic in tpJoin.DefaultIfEmpty()
                             join c in DataContext.Get<MediaComposer>() on f.TopicId equals c.ComposerId into composertpJoin
                             from composer in composertpJoin.DefaultIfEmpty()
                             join d in DataContext.Get<DocumentFile>() on f.FileId equals d.FileId into documentJoin
                             from fd in documentJoin.DefaultIfEmpty()
                             join df in DataContext.Get<DocumentFolder>() on fd.FolderId equals df.FolderId into documentFolderJoin
                             from dff in documentFolderJoin.DefaultIfEmpty()
                             where (status == null || fd.IsActive == status)
                             select new MediaFileInfo
                             {
                                 MediaId = f.MediaId,
                                 FileId = f.FileId,
                                 TypeId = f.TypeId,
                                 TopicId = f.TopicId,
                                 Artist = f.Artist,
                                 ComposerId = f.ComposerId,
                                 AutoStart = f.AutoStart,
                                 MediaLoop = f.MediaLoop,
                                 Lyric = f.Lyric,
                                 SmallPhoto = f.SmallPhoto,
                                 LargePhoto = f.LargePhoto,
                                 ListOrder = f.ListOrder,
                                 Type = type,
                                 Topic = topic, 
                                 Composer = composer,
                                 DocumentFile = fd,
                                 DocumentFolder = dff
                             });

            if (!string.IsNullOrEmpty(searchText))
            {
                queryable = queryable.Where(x => x.DocumentFile.FileName.ToLower().Contains(searchText.ToLower())
                        || x.DocumentFile.FileTitle.ToLower().Contains(searchText.ToLower()));
            }

            if (typeId != null)
            {
                queryable = queryable.Where(x => x.TypeId == typeId);
            }
            if (topicId != null && topicId > 0)
            {
                queryable = queryable.Where(x => x.TopicId == topicId);
            }
            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public MediaFileInfo GetDetails(int mediaId)
        {
            return (from f in DataContext.Get<MediaFile>()
                    join t in DataContext.Get<MediaType>() on f.TypeId equals t.TypeId into atJoin
                    from type in atJoin.DefaultIfEmpty()
                    join tp in DataContext.Get<MediaTopic>() on f.TopicId equals tp.TopicId into tpJoin
                    from topic in tpJoin.DefaultIfEmpty()
                    join c in DataContext.Get<MediaComposer>() on f.TopicId equals c.ComposerId into composertpJoin
                    from composer in composertpJoin.DefaultIfEmpty()
                    join d in DataContext.Get<DocumentFile>() on f.FileId equals d.FileId into documentJoin
                    from fd in documentJoin.DefaultIfEmpty()
                    join df in DataContext.Get<DocumentFolder>() on fd.FolderId equals df.FolderId into documentFolderJoin
                    from dff in documentFolderJoin.DefaultIfEmpty()
                    where f.MediaId == mediaId
                    select new MediaFileInfo
                    {
                        MediaId = f.MediaId,
                        FileId = f.FileId,
                        TypeId = f.TypeId,
                        TopicId = f.TopicId,
                        Artist = f.Artist,
                        ComposerId = f.ComposerId,
                        AutoStart = f.AutoStart,
                        MediaLoop = f.MediaLoop,
                        Lyric = f.Lyric,
                        SmallPhoto = f.SmallPhoto,
                        LargePhoto = f.LargePhoto,
                        ListOrder = f.ListOrder,
                        Type = type,
                        Topic = topic,
                        Composer = composer,
                        DocumentFile = fd,
                        DocumentFolder = dff
                    }).FirstOrDefault();
        }

        public MediaFileInfo GetDetailsByFileId(int fileId)
        {
            return (from f in DataContext.Get<MediaFile>()
                    join t in DataContext.Get<MediaType>() on f.TypeId equals t.TypeId into atJoin
                    from type in atJoin.DefaultIfEmpty()
                    join tp in DataContext.Get<MediaTopic>() on f.TopicId equals tp.TopicId into tpJoin
                    from topic in tpJoin.DefaultIfEmpty()
                    join c in DataContext.Get<MediaComposer>() on f.TopicId equals c.ComposerId into composertpJoin
                    from composer in composertpJoin.DefaultIfEmpty()
                    join d in DataContext.Get<DocumentFile>() on f.FileId equals d.FileId into documentJoin
                    from fd in documentJoin.DefaultIfEmpty()
                    join df in DataContext.Get<DocumentFolder>() on fd.FolderId equals df.FolderId into documentFolderJoin
                    from dff in documentFolderJoin.DefaultIfEmpty()
                    where f.FileId == fileId
                    select new MediaFileInfo
                    {
                        MediaId = f.MediaId,
                        FileId = f.FileId,
                        TypeId = f.TypeId,
                        TopicId = f.TopicId,
                        Artist = f.Artist,
                        ComposerId = f.ComposerId,
                        AutoStart = f.AutoStart,
                        MediaLoop = f.MediaLoop,
                        Lyric = f.Lyric,
                        SmallPhoto = f.SmallPhoto,
                        LargePhoto = f.LargePhoto,
                        ListOrder = f.ListOrder,
                        Type = type,
                        Topic = topic,
                        Composer = composer,
                        DocumentFile = fd,
                        DocumentFolder = dff
                    }).FirstOrDefault();
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<MediaFile>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }

        public SelectList PopulateMediaFileTypes(string selectedValue = null, bool? isShowSelectText = false)
        {
            var dict = new Dictionary<string, string>
            {
                {"HTML5 VIDEO", MediaFileType.HTML5VIDEO },
                {"YOUTUBE", MediaFileType.YOUTUBE },
                {"VIMEO", MediaFileType.VIMEO },
                {"WISTIA", MediaFileType.WISTIA }
            };

            var lst = new List<SelectListItem>();
            lst.AddRange(dict.Select(keyValuePair => new SelectListItem()
            {
                Value = keyValuePair.Key,
                Text = keyValuePair.Value
            }));

            if (isShowSelectText != null && isShowSelectText.Value == true)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
    }
}
