using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Contents
{
    public class MediaAlbumRepository : RepositoryBase<MediaAlbum>, IMediaAlbumRepository
    {
        public MediaAlbumRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<MediaAlbumInfo> GetAlbums(string albumName, int? typeId, int? topicId, MediaAlbumStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = (from a in DataContext.Get<MediaAlbum>()
                             join t in DataContext.Get<MediaType>() on a.TypeId equals t.TypeId into atJoin
                             from at in atJoin.DefaultIfEmpty()
                             join tp in DataContext.Get<MediaTopic>() on a.TopicId equals tp.TopicId into tpJoin
                             from topic in tpJoin.DefaultIfEmpty()
                             where (status == null || a.Status == status)
                             select new MediaAlbumInfo
                             {
                                 TypeId = a.TypeId,
                                 TopicId = a.TopicId,
                                 AlbumId = a.AlbumId,
                                 AlbumName = a.AlbumName,
                                 AlbumAlias = a.AlbumAlias,
                                 FrontImage = a.FrontImage,
                                 MainImage = a.MainImage,
                                 Description = a.Description,
                                 TotalViews = a.TotalViews,
                                 ListOrder = a.ListOrder,
                                 Status = a.Status,
                                 CreatedDate = a.CreatedDate,
                                 Type = at,
                                 Topic = topic
                             });
            if (!string.IsNullOrEmpty(albumName))
            {
                queryable = queryable.Where(x => x.AlbumName.ToLower().Contains(albumName.ToLower())
                        || x.Description.ToLower().Contains(albumName.ToLower()));
            }

            if (typeId !=null && typeId > 0)
            {
                queryable = queryable.Where(x => x.TypeId == typeId);
            }
            if (topicId != null && topicId > 0)
            {
                queryable = queryable.Where(x => x.TopicId == topicId);
            }
            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<MediaAlbumInfo> GetAlbums(int? typeId, int? topicId, MediaAlbumStatus? status)
        {
            var queryable = (from a in DataContext.Get<MediaAlbum>()
                join t in DataContext.Get<MediaType>() on a.TypeId equals t.TypeId into atJoin
                from at in atJoin.DefaultIfEmpty()
                join tp in DataContext.Get<MediaTopic>() on a.TopicId equals tp.TopicId into tpJoin
                from topic in tpJoin.DefaultIfEmpty()
                where (status == null || a.Status == status)
                orderby a.ListOrder descending 
                select new MediaAlbumInfo
                {
                    TypeId = a.TypeId,
                    TopicId = a.TopicId,
                    AlbumId = a.AlbumId,
                    AlbumName = a.AlbumName,
                    AlbumAlias = a.AlbumAlias,
                    FrontImage = a.FrontImage,
                    MainImage = a.MainImage,
                    Description = a.Description,
                    TotalViews = a.TotalViews,
                    ListOrder = a.ListOrder,
                    Status = a.Status,
                    CreatedDate = a.CreatedDate,
                    Type = at,
                    Topic = topic
                });
            if (typeId != null && typeId > 0)
            {
                queryable = queryable.Where(x => x.TypeId == typeId);
            }
            if (topicId != null && topicId > 0)
            {
                queryable = queryable.Where(x => x.TopicId == topicId);
            }
            return queryable.AsEnumerable();
        }
        public MediaAlbumInfo GeDetails(int id)
        {
           return (from a in DataContext.Get<MediaAlbum>()
                join t in DataContext.Get<MediaType>() on a.TypeId equals t.TypeId into atJoin
                from at in atJoin.DefaultIfEmpty()
                join tp in DataContext.Get<MediaTopic>() on a.TopicId equals tp.TopicId into tpJoin
                from topic in tpJoin.DefaultIfEmpty()
                where a.AlbumId == id
                select new MediaAlbumInfo
                {
                    TypeId = a.TypeId,
                    TopicId = a.TopicId,
                    AlbumId = a.AlbumId,
                    AlbumName = a.AlbumName,
                    AlbumAlias = a.AlbumAlias,
                    FrontImage = a.FrontImage,
                    MainImage = a.MainImage,
                    Description = a.Description,
                    TotalViews = a.TotalViews,
                    ListOrder = a.ListOrder,
                    Status = a.Status,
                    CreatedDate = a.CreatedDate,
                    Type = at,
                    Topic = topic
                }).FirstOrDefault();
        }

        public int GetNewTotalView()
        {
            int totalViews = 1;
            var query = from u in DataContext.Get<MediaAlbum>() select (int)u.TotalViews;
            if (query.Any())
            {
                totalViews = query.Max() + 1;
            }
            return totalViews;
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<MediaAlbum>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }

        public MultiSelectList PoplulateMediaAlbumMultiSelectList(int typeId, int topicId, MediaAlbumStatus? status = null,bool ? isShowSelectText = null, int[] selectedValues = null)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from p in DataContext.Get<MediaAlbum>()
                       where p.TypeId == typeId && p.TopicId == topicId && (status == null || p.Status == status)
                       select p).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem
                {
                    Text = p.AlbumName,
                    Value = p.AlbumId.ToString(),
                    Selected = (selectedValues != null && selectedValues.Contains(p.AlbumId))
                }).OrderByDescending(m => m.Text).ToList();

                if (isShowSelectText != null && isShowSelectText == true)
                {
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectMediaAlbum} ---", Value = "" });
                }
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }

            return new MultiSelectList(listItems, "Value", "Text", selectedValues);
        }
    }
}
