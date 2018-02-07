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
    public class MediaPlayListRepository : RepositoryBase<MediaPlayList>, IMediaPlayListRepository
    {
        public MediaPlayListRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<MediaPlayListInfo> GetMediaPlayLists(int? typeId, int? topicId, MediaPlayListStatus? status)
        {
            var queryable = (from a in DataContext.Get<MediaPlayList>()
                             join t in DataContext.Get<MediaType>() on a.TypeId equals t.TypeId into atJoin
                             from at in atJoin.DefaultIfEmpty()
                             join tp in DataContext.Get<MediaTopic>() on a.TopicId equals tp.TopicId into tpJoin
                             from topic in tpJoin.DefaultIfEmpty()
                             where (status == null || a.Status == status)
                             orderby a.ListOrder descending
                             select new MediaPlayListInfo
                             {
                                 TypeId = a.TypeId,
                                 TopicId = a.TopicId,
                                 PlayListId = a.PlayListId,
                                 PlayListName = a.PlayListName,
                                 PlayListAlias = a.PlayListAlias,
                                 FrontImage = a.FrontImage,
                                 MainImage = a.MainImage,
                                 Description = a.Description,
                                 TotalViews = a.TotalViews,
                                 ListOrder = a.ListOrder,
                                 CreatedDate = a.CreatedDate,
                                 Status = a.Status,
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

        public IEnumerable<MediaPlayListInfo> GetMediaPlayLists(string searchText, int? typeId, int? topicId, MediaPlayListStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = (from a in DataContext.Get<MediaPlayList>()
                join t in DataContext.Get<MediaType>() on a.TypeId equals t.TypeId into atJoin
                from at in atJoin.DefaultIfEmpty()
                join tp in DataContext.Get<MediaTopic>() on a.TopicId equals tp.TopicId into tpJoin
                from topic in tpJoin.DefaultIfEmpty()
                where (status == null || a.Status == status)
                orderby a.ListOrder descending
                select new MediaPlayListInfo
                {
                    TypeId = a.TypeId,
                    TopicId = a.TopicId,
                    PlayListId = a.PlayListId,
                    PlayListName = a.PlayListName,
                    PlayListAlias = a.PlayListAlias,
                    FrontImage = a.FrontImage,
                    MainImage = a.MainImage,
                    Description = a.Description,
                    TotalViews = a.TotalViews,
                    ListOrder = a.ListOrder,
                    CreatedDate = a.CreatedDate,
                    Status = a.Status,
                    Type = at,
                    Topic = topic
                });

            if (!string.IsNullOrEmpty(searchText))
            {
                queryable = queryable.Where(x => x.PlayListName.ToLower().Contains(searchText.ToLower())
                        || x.Description.ToLower().Contains(searchText.ToLower()));
            }
            if (typeId != null && typeId > 0)
            {
                queryable = queryable.Where(x => x.TypeId == typeId);
            }
            if (topicId != null && topicId > 0)
            {
                queryable = queryable.Where(x => x.TopicId == topicId);
            }

            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public MediaPlayListInfo GeDetails(int id)
        {
            return (from a in DataContext.Get<MediaPlayList>()
                join t in DataContext.Get<MediaType>() on a.TypeId equals t.TypeId into atJoin
                from at in atJoin.DefaultIfEmpty()
                join tp in DataContext.Get<MediaTopic>() on a.TopicId equals tp.TopicId into tpJoin
                from topic in tpJoin.DefaultIfEmpty()
                where a.PlayListId == id
                    select new MediaPlayListInfo
                {
                    TypeId = a.TypeId,
                    TopicId = a.TopicId,
                    PlayListId = a.PlayListId,
                    PlayListName = a.PlayListName,
                    PlayListAlias = a.PlayListAlias,
                    FrontImage = a.FrontImage,
                    MainImage = a.MainImage,
                    Description = a.Description,
                    TotalViews = a.TotalViews,
                    ListOrder = a.ListOrder,
                    CreatedDate = a.CreatedDate,
                    Status = a.Status,
                    Type = at,
                    Topic = topic
                }).FirstOrDefault();
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<MediaPlayList>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
        public bool HasDataExisted(string playListName)
        {
            var query = DataContext.Get<MediaPlayList>().FirstOrDefault(p => p.PlayListName.ToLower().Contains(playListName.ToLower()));
            return (query != null);
        }

        public SelectList PopulateMediaPlayListSelectList(int? typeId, int? topicId, MediaPlayListStatus? status = null, bool? isShowSelectText = null, int? selectedValue = null)
        {
            var listItems = new List<SelectListItem>();
            var lst = GetMediaPlayLists(typeId, topicId, status).ToList();
            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.PlayListName, Value = p.PlayListId.ToString() }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                {
                    listItems.Insert(0, new SelectListItem { Text = $@"--- {LanguageResource.SelectMediaPlayList} ---", Value = "" });
                }
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $@"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public MultiSelectList PoplulateMediaPlayListMultiSelectList(int typeId, int topicId, MediaPlayListStatus? status = null, bool? isShowSelectText = null, int[] selectedValues = null)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from p in DataContext.Get<MediaPlayList>()
                       where p.TypeId == typeId && p.TopicId == topicId && (status == null || p.Status == status)
                       select p).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem
                {
                    Text = p.PlayListName,
                    Value = p.PlayListId.ToString(),
                    Selected = (selectedValues != null && selectedValues.Contains(p.PlayListId))
                }).OrderByDescending(m => m.Text).ToList();

                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $@"--- {LanguageResource.Select} ---", Value = "" });
            }

            return new MultiSelectList(listItems, "Value", "Text", selectedValues);
        }
    }
}
