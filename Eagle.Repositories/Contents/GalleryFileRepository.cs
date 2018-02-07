using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Galleries;
using Eagle.Entities.SystemManagement.FileStorage;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Contents
{
    public class GalleryFileRepository : RepositoryBase<GalleryFile>, IGalleryFileRepository
    {
        public GalleryFileRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<GalleryFileInfo> GetGalleryFileList(int? collectionId, GalleryFileStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from f in DataContext.Get<GalleryFile>()
                        join c in DataContext.Get<GalleryCollection>() on f.CollectionId equals c.CollectionId into collectionJoin
                        from fc in collectionJoin.DefaultIfEmpty()
                        join t in DataContext.Get<GalleryTopic>() on fc.TopicId equals t.TopicId into topicJoin
                        from ct in topicJoin.DefaultIfEmpty()
                        join d in DataContext.Get<DocumentFile>() on f.FileId equals d.FileId into documentJoin
                        from fd in documentJoin.DefaultIfEmpty()
                        join df in DataContext.Get<DocumentFolder>() on fd.FolderId equals df.FolderId into documentFolderJoin
                        from dff in documentFolderJoin.DefaultIfEmpty()
                        where (status == null || f.Status == status)
                        select new GalleryFileInfo
                        {
                            CollectionId = f.CollectionId,
                            FileId = f.FileId,
                            ListOrder = f.ListOrder,
                            Status = f.Status,
                            GalleryTopic = ct,
                            GalleryCollection = fc,
                            DocumentFile = fd,
                            DocumentFolder = dff
                        };

            if (collectionId != null && collectionId > 0)
            {
                query = query.Where(x => x.CollectionId == collectionId);
            }
            return query.WithRecordCount(out recordCount)
                                         .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<GalleryFileInfo> GetGalleryFiles(int? collectionId, GalleryFileStatus? status=null)
        {
            var query = from f in DataContext.Get<GalleryFile>()
                        join c in DataContext.Get<GalleryCollection>() on f.CollectionId equals c.CollectionId into collectionJoin
                        from fc in collectionJoin.DefaultIfEmpty()
                        join t in DataContext.Get<GalleryTopic>() on fc.TopicId equals t.TopicId into topicJoin
                        from ct in topicJoin.DefaultIfEmpty()
                        join d in DataContext.Get<DocumentFile>() on f.FileId equals d.FileId into documentJoin
                        from fd in documentJoin.DefaultIfEmpty()
                        join df in DataContext.Get<DocumentFolder>() on fd.FolderId equals df.FolderId into documentFolderJoin
                        from dff in documentFolderJoin.DefaultIfEmpty()
                        where (status == null || f.Status == status)
                        select new GalleryFileInfo
                        {
                            CollectionId = f.CollectionId,
                            FileId = f.FileId,
                            ListOrder = f.ListOrder,
                            Status = f.Status,
                            GalleryTopic = ct,
                            GalleryCollection = fc,
                            DocumentFile = fd,
                            DocumentFolder = dff
                        };

            if (collectionId != null && collectionId > 0)
            {
                query = query.Where(x => x.CollectionId == collectionId);
            }

            return query.AsEnumerable();
        }

        public GalleryFile GetDetails(int collectionId, int fileId)
        {
            return (from pm in DataContext.Get<GalleryFile>()
                    where pm.CollectionId == collectionId && pm.FileId == fileId
                    select pm).FirstOrDefault();
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<GalleryFile>() select u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
        public bool HasDataExisted(int collectionId, int fileId)
        {
            var query = DataContext.Get<GalleryFile>().FirstOrDefault(p => p.CollectionId == collectionId && p.FileId == fileId);
            return (query != null);
        }
    }
}
