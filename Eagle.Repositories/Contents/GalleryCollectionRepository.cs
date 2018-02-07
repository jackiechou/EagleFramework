using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Galleries;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Contents
{
    public class GalleryCollectionRepository : RepositoryBase<GalleryCollection>, IGalleryCollectionRepository
    {
        public GalleryCollectionRepository(IDataContext dataContext) : base(dataContext)
        {
        }
     
        public IEnumerable<GalleryCollectionInfo> GetGalleryCollections(string collectionName, int? topicId, GalleryCollectionStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from c in DataContext.Get<GalleryCollection>()
                            join t in DataContext.Get<GalleryTopic>() on c.TopicId equals t.TopicId into topicJoin
                            from ct in topicJoin.DefaultIfEmpty()
                            where status == null || c.Status == status
                            select new GalleryCollectionInfo
                            {
                                TopicId = c.TopicId,
                                CollectionId = c.CollectionId,
                                CollectionName = c.CollectionName,
                                Description = c.Description,
                                IconFile = c.IconFile,
                                ListOrder = c.ListOrder,
                                Status = c.Status,
                                GalleryTopic = ct
                            };

            if (!string.IsNullOrEmpty(collectionName))
            {
                query = query.Where(x => x.CollectionName.ToLower().Contains(collectionName.ToLower()) 
                        || x.Description.ToLower().Contains(collectionName.ToLower()));
            }

            if (topicId != null && topicId > 0)
            {
                query = query.Where(x => x.TopicId == topicId);
            }

            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        
        public IEnumerable<GalleryCollectionInfo> GetGalleryCollections(string collectionName, int? topicId, GalleryCollectionStatus? status = null)
        {
            var query = (from c in DataContext.Get<GalleryCollection>()
                    join t in DataContext.Get<GalleryTopic>() on c.TopicId equals t.TopicId into topicJoin
                    from ct in topicJoin.DefaultIfEmpty()
                    where (status == null || c.Status == status)
                    orderby c.ListOrder descending
                    select new GalleryCollectionInfo
                    {
                        TopicId = c.TopicId,
                        CollectionId = c.CollectionId,
                        CollectionName = c.CollectionName,
                        Description = c.Description,
                        IconFile = c.IconFile,
                        ListOrder = c.ListOrder,
                        Status = c.Status,
                        GalleryTopic = ct
                    });

            if (topicId != null && topicId > 0)
            {
                query = query.Where(x => x.TopicId == topicId);
            }

            if (!string.IsNullOrEmpty(collectionName))
            {
                query = query.Where(x => x.CollectionName.ToLower().Contains(collectionName.ToLower())
                                         || x.Description.ToLower().Contains(collectionName.ToLower()));
            }

            return query.AsEnumerable();
        }

        public GalleryCollectionInfo GetDetail(int collectionId)
        {
            return (from c in DataContext.Get<GalleryCollection>()
                    join t in DataContext.Get<GalleryTopic>() on c.TopicId equals t.TopicId into topicJoin
                    from ct in topicJoin.DefaultIfEmpty()
                    where c.CollectionId == collectionId
                    select new GalleryCollectionInfo
                    {
                        TopicId = c.TopicId,
                        CollectionId = c.CollectionId,
                        CollectionName = c.CollectionName,
                        Description = c.Description,
                        IconFile = c.IconFile,
                        ListOrder = c.ListOrder,
                        Status = c.Status,
                        GalleryTopic = ct
                    }).FirstOrDefault();
        }
        public GalleryCollectionInfo GetLatestGalleryCollection()
        {
            return (from c in DataContext.Get<GalleryCollection>()
                    join t in DataContext.Get<GalleryTopic>() on c.TopicId equals t.TopicId into topicJoin
                    from ct in topicJoin.DefaultIfEmpty()
                    orderby c.ListOrder descending
                    select new GalleryCollectionInfo
                    {
                        TopicId = c.TopicId,
                        CollectionId = c.CollectionId,
                        CollectionName = c.CollectionName,
                        Description = c.Description,
                        IconFile = c.IconFile,
                        ListOrder = c.ListOrder,
                        Status = c.Status,
                        GalleryTopic = ct
                    }).FirstOrDefault();
        }

        
        public List<SelectListItem> GetGalleryCollectionSelectList(int topicId, GalleryCollectionStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<GalleryCollection>()
                       where c.TopicId == topicId && (status == null || c.Status == status)
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.CollectionName, Value = p.CollectionId.ToString(), Selected = (selectedValue != null && p.CollectionId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectGalleryCollection} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return listItems;
        } 

        public SelectList PopulateGalleryCollectionSelectList(int topicId, GalleryCollectionStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<GalleryCollection>()
                       where c.TopicId == topicId && (status == null || c.Status == status)
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.CollectionName, Value = p.CollectionId.ToString(), Selected = (selectedValue!=null && p.CollectionId == selectedValue)  }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectGalleryCollection} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<GalleryCollection>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }

        public bool HasDataExisted(string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName)) return false;
            var query = DataContext.Get<GalleryCollection>().FirstOrDefault(p => p.CollectionName.Equals(collectionName));
            return (query != null);
        }
    }
}
