using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Articles;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Contents
{
    public class NewsRepository : RepositoryBase<News>, INewsRepository
    {
        public NewsRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<NewsInfo> Search(out int recordCount, string searchText = null, string author = null, int? categoryId = null, DateTime? fromDate = null, DateTime? toDate = null, NewsStatus? status = null, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = from x in DataContext.Get<News>()
                            join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                            from cate in catelist.DefaultIfEmpty()
                            join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                            from profile in profileInfo.DefaultIfEmpty()
                            join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                            from contact in contactInfo.DefaultIfEmpty()
                            where (status == null || x.Status == status)
                            select new NewsInfo
                            {
                                NewsId = x.NewsId,
                                Title = x.Title,
                                Headline = x.Headline,
                                Alias = x.Alias,
                                Summary = x.Summary,
                                FrontImage = x.FrontImage,
                                MainImage = x.MainImage,
                                MainText = x.MainText,
                                NavigateUrl = x.NavigateUrl,
                                Authors = x.Authors,
                                ListOrder = x.ListOrder,
                                Tags = x.Tags,
                                Source = x.Source,
                                TotalRates = x.TotalRates,
                                TotalViews = x.TotalViews,
                                PostedDate = x.PostedDate,
                                Status = x.Status,
                                CreatedDate = x.CreatedDate,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedByUserId = x.CreatedByUserId,
                                LastModifiedByUserId = x.LastModifiedByUserId,
                                Ip = x.Ip,
                                LastUpdatedIp = x.LastUpdatedIp,
                                Category = cate,
                                CategoryId = x.CategoryId,
                                CategoryName = cate.CategoryName,
                                FullName = (cate.LanguageCode == LanguageType.Vietnamese)? contact.LastName + " " + contact.FirstName : contact.FirstName + " " + contact.LastName
                            };
            if (categoryId != null && categoryId > 0)
            {
                queryable = queryable.Where(x => x.CategoryId == categoryId);
            }
            if (fromDate != null)
            {
                queryable = queryable.Where(x => x.PostedDate >= fromDate);
            }
            if (toDate != null)
            {
                queryable = queryable.Where(x => x.PostedDate <= toDate);
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                queryable = queryable.Where(x => x.Headline.ToLower().Contains(searchText.ToLower())
                || x.MainText.ToLower().Contains(searchText.ToLower()));
            }
            if (!string.IsNullOrEmpty(author))
            {
                queryable = queryable.Where(x => x.Authors.ToLower().Contains(author.ToLower()));
            }
            return queryable.WithRecordCount(out recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<NewsInfo> GetList(string searchText, NewsStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = from x in DataContext.Get<News>()
                            join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                            from cate in catelist.DefaultIfEmpty()
                            join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                            from profile in profileInfo.DefaultIfEmpty()
                            join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                            from contact in contactInfo.DefaultIfEmpty()
                            where status == null || x.Status == status
                            orderby x.NewsId descending
                            select new NewsInfo
                            {
                                CategoryId = cate.CategoryId,
                                NewsId = x.NewsId,
                                Title = x.Title,
                                Headline = x.Headline,
                                Alias = x.Alias,
                                Summary = x.Summary,
                                FrontImage = x.FrontImage,
                                MainImage = x.MainImage,
                                MainText = x.MainText,
                                NavigateUrl = x.NavigateUrl,
                                Authors = x.Authors,
                                ListOrder = x.ListOrder,
                                Tags = x.Tags,
                                Source = x.Source,
                                TotalRates = x.TotalRates,
                                TotalViews = x.TotalViews,
                                PostedDate = x.PostedDate,
                                Status = x.Status,
                                CreatedDate = x.CreatedDate,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedByUserId = x.CreatedByUserId,
                                LastModifiedByUserId = x.LastModifiedByUserId,
                                Ip = x.Ip,
                                LastUpdatedIp = x.LastUpdatedIp,
                                CategoryName = cate.CategoryName,
                                FullName = contact.FirstName + " " + contact.LastName,
                                Category = cate
                            };

            if (!string.IsNullOrEmpty(searchText))
            {
                queryable = queryable.Where(x => x.Headline.ToLower().Contains(searchText.ToLower())
                || x.MainText.ToLower().Contains(searchText.ToLower()));
            }
            return queryable.WithRecordCount(out recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<NewsInfo> GetNews(int number)
        {
            var queryable = (from x in DataContext.Get<News>()
                             join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                             from cate in catelist.DefaultIfEmpty()
                             join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                             from profile in profileInfo.DefaultIfEmpty()
                             join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                             from contact in contactInfo.DefaultIfEmpty()
                             where x.Status == NewsStatus.Published
                             orderby x.ListOrder descending
                             select new NewsInfo
                             {
                                 CategoryId = cate.CategoryId,
                                 NewsId = x.NewsId,
                                 Title = x.Title,
                                 Headline = x.Headline,
                                 Alias = x.Alias,
                                 Summary = x.Summary,
                                 FrontImage = x.FrontImage,
                                 MainImage = x.MainImage,
                                 MainText = x.MainText,
                                 NavigateUrl = x.NavigateUrl,
                                 Authors = x.Authors,
                                 ListOrder = x.ListOrder,
                                 Tags = x.Tags,
                                 Source = x.Source,
                                 TotalRates = x.TotalRates,
                                 TotalViews = x.TotalViews,
                                 PostedDate = x.PostedDate,
                                 Status = x.Status,
                                 CreatedDate = x.CreatedDate,
                                 LastModifiedDate = x.LastModifiedDate,
                                 CreatedByUserId = x.CreatedByUserId,
                                 LastModifiedByUserId = x.LastModifiedByUserId,
                                 Ip = x.Ip,
                                 LastUpdatedIp = x.LastUpdatedIp,
                                 CategoryName = cate.CategoryName,
                                 FullName = contact.FirstName + " " + contact.LastName,
                                 Category = cate
                             });

            var recordCount = queryable.Count();
            if (number > recordCount)
            {
                return queryable.Take(recordCount).AsEnumerable();
            }
            return queryable.Take(number).AsEnumerable();
        }
        public IEnumerable<NewsInfo> GetListByCategoryId(int categoryId, NewsStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            return (from x in DataContext.Get<News>()
                    join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                    from cate in catelist.DefaultIfEmpty()
                    join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                    from profile in profileInfo.DefaultIfEmpty()
                    join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                    from contact in contactInfo.DefaultIfEmpty()
                    where x.CategoryId == categoryId && (status == null || x.Status == status)
                    orderby x.NewsId descending
                    select new NewsInfo
                    {
                        CategoryId = cate.CategoryId,
                        NewsId = x.NewsId,
                        Title = x.Title,
                        Headline = x.Headline,
                        Alias = x.Alias,
                        Summary = x.Summary,
                        FrontImage = x.FrontImage,
                        MainImage = x.MainImage,
                        MainText = x.MainText,
                        NavigateUrl = x.NavigateUrl,
                        Authors = x.Authors,
                        ListOrder = x.ListOrder,
                        Tags = x.Tags,
                        Source = x.Source,
                        TotalRates = x.TotalRates,
                        TotalViews = x.TotalViews,
                        PostedDate = x.PostedDate,
                        Status = x.Status,
                        CreatedDate = x.CreatedDate,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedByUserId = x.CreatedByUserId,
                        LastModifiedByUserId = x.LastModifiedByUserId,
                        Ip = x.Ip,
                        LastUpdatedIp = x.LastUpdatedIp,
                        CategoryName = cate.CategoryName,
                        FullName = contact.FirstName + " " + contact.LastName,
                        Category = cate
                    }).WithRecordCount(out recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<NewsInfo> GetListByCategoryId(int categoryId, string beginDate, string endDate, NewsStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from x in DataContext.Get<News>()
                        join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                        from cate in catelist.DefaultIfEmpty()
                        join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                        from profile in profileInfo.DefaultIfEmpty()
                        join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                        from contact in contactInfo.DefaultIfEmpty()
                        where x.CategoryId == categoryId && (status == null || x.Status == status)
                        orderby x.NewsId descending
                        select new NewsInfo
                        {
                            CategoryId = cate.CategoryId,
                            NewsId = x.NewsId,
                            Title = x.Title,
                            Headline = x.Headline,
                            Alias = x.Alias,
                            Summary = x.Summary,
                            FrontImage = x.FrontImage,
                            MainImage = x.MainImage,
                            MainText = x.MainText,
                            NavigateUrl = x.NavigateUrl,
                            Authors = x.Authors,
                            ListOrder = x.ListOrder,
                            Tags = x.Tags,
                            Source = x.Source,
                            TotalRates = x.TotalRates,
                            TotalViews = x.TotalViews,
                            PostedDate = x.PostedDate,
                            Status = x.Status,
                            CreatedDate = x.CreatedDate,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedByUserId = x.CreatedByUserId,
                            LastModifiedByUserId = x.LastModifiedByUserId,
                            Ip = x.Ip,
                            LastUpdatedIp = x.LastUpdatedIp,
                            CategoryName = cate.CategoryName,
                            FullName = contact.FirstName + " " + contact.LastName,
                            Category = cate
                        };

            if (!string.IsNullOrEmpty(beginDate) && string.IsNullOrEmpty(endDate))
            {
                query = query.Where(x => x.PostedDate >= Convert.ToDateTime(beginDate));
            }

            if (string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
            {
                query = query.Where(x => x.PostedDate <= Convert.ToDateTime(endDate));
            }

            if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
            {
                query = query.Where(x => x.PostedDate >= Convert.ToDateTime(beginDate) && x.PostedDate <= Convert.ToDateTime(endDate));
            }
            return query.WithRecordCount(ref recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<NewsInfo> GetTopListByCategoryId(int categoryId, int recordCount)
        {
            var query = (from x in DataContext.Get<News>()
                         join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                         from cate in catelist.DefaultIfEmpty()
                         join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                         from profile in profileInfo.DefaultIfEmpty()
                         join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                         from contact in contactInfo.DefaultIfEmpty()
                         where x.CategoryId == categoryId
                         orderby x.NewsId descending
                         select new NewsInfo
                         {
                             CategoryId = cate.CategoryId,
                             NewsId = x.NewsId,
                             Title = x.Title,
                             Headline = x.Headline,
                             Alias = x.Alias,
                             Summary = x.Summary,
                             FrontImage = x.FrontImage,
                             MainImage = x.MainImage,
                             MainText = x.MainText,
                             NavigateUrl = x.NavigateUrl,
                             Authors = x.Authors,
                             ListOrder = x.ListOrder,
                             Tags = x.Tags,
                             Source = x.Source,
                             TotalRates = x.TotalRates,
                             TotalViews = x.TotalViews,
                             PostedDate = x.PostedDate,
                             Status = x.Status,
                             CreatedDate = x.CreatedDate,
                             LastModifiedDate = x.LastModifiedDate,
                             CreatedByUserId = x.CreatedByUserId,
                             LastModifiedByUserId = x.LastModifiedByUserId,
                             Ip = x.Ip,
                             LastUpdatedIp = x.LastUpdatedIp,
                             CategoryName = cate.CategoryName,
                             FullName = contact.FirstName + " " + contact.LastName,
                             Category = cate
                         });
            var itemCount = (recordCount > query.Count()) ? query.Count() : recordCount;
            return query.Take(itemCount).AsEnumerable();
        }
        public IEnumerable<NewsInfo> GetRandomList(int recordCount)
        {
            var query = (from x in DataContext.Get<News>()
                         join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                         from cate in catelist.DefaultIfEmpty()
                         join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                         from profile in profileInfo.DefaultIfEmpty()
                         join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                         from contact in contactInfo.DefaultIfEmpty()
                         orderby x.NewsId descending
                         select new NewsInfo
                         {
                             CategoryId = cate.CategoryId,
                             NewsId = x.NewsId,
                             Title = x.Title,
                             Headline = x.Headline,
                             Alias = x.Alias,
                             Summary = x.Summary,
                             FrontImage = x.FrontImage,
                             MainImage = x.MainImage,
                             MainText = x.MainText,
                             NavigateUrl = x.NavigateUrl,
                             Authors = x.Authors,
                             ListOrder = x.ListOrder,
                             Tags = x.Tags,
                             Source = x.Source,
                             TotalRates = x.TotalRates,
                             TotalViews = x.TotalViews,
                             PostedDate = x.PostedDate,
                             Status = x.Status,
                             CreatedDate = x.CreatedDate,
                             LastModifiedDate = x.LastModifiedDate,
                             CreatedByUserId = x.CreatedByUserId,
                             LastModifiedByUserId = x.LastModifiedByUserId,
                             Ip = x.Ip,
                             LastUpdatedIp = x.LastUpdatedIp,
                             CategoryName = cate.CategoryName,
                             FullName = contact.FirstName + " " + contact.LastName,
                             Category = cate
                         });
            var itemCount = (recordCount > query.Count()) ? query.Count() : recordCount;
            return query.Take(itemCount).AsEnumerable();
        }
        public IEnumerable<NewsInfo> GetNewerListByCategoryId(int categoryId, int selectedNewId, int recordCount)
        {
            var query = (from x in DataContext.Get<News>()
                         join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                         from cate in catelist.DefaultIfEmpty()
                         join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                         from profile in profileInfo.DefaultIfEmpty()
                         join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                         from contact in contactInfo.DefaultIfEmpty()
                         where cate.CategoryId == categoryId && x.Status == NewsStatus.Published && x.NewsId > selectedNewId
                         orderby x.NewsId descending
                         select new NewsInfo
                         {
                             CategoryId = cate.CategoryId,
                             NewsId = x.NewsId,
                             Title = x.Title,
                             Headline = x.Headline,
                             Alias = x.Alias,
                             Summary = x.Summary,
                             FrontImage = x.FrontImage,
                             MainImage = x.MainImage,
                             MainText = x.MainText,
                             NavigateUrl = x.NavigateUrl,
                             Authors = x.Authors,
                             ListOrder = x.ListOrder,
                             Tags = x.Tags,
                             Source = x.Source,
                             TotalRates = x.TotalRates,
                             TotalViews = x.TotalViews,
                             PostedDate = x.PostedDate,
                             Status = x.Status,
                             CreatedDate = x.CreatedDate,
                             LastModifiedDate = x.LastModifiedDate,
                             CreatedByUserId = x.CreatedByUserId,
                             LastModifiedByUserId = x.LastModifiedByUserId,
                             Ip = x.Ip,
                             LastUpdatedIp = x.LastUpdatedIp,
                             CategoryName = cate.CategoryName,
                             FullName = contact.FirstName + " " + contact.LastName,
                             Category = cate
                         });
            var itemCount = (recordCount > query.Count()) ? query.Count() : recordCount;
            return query.Take(itemCount).AsEnumerable();
        }
        public IEnumerable<NewsInfo> GetOlderListByCategoryId(int categoryId, int recordCount)
        {
            var query = (from x in DataContext.Get<News>()
                         join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                         from cate in catelist.DefaultIfEmpty()
                         join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                         from profile in profileInfo.DefaultIfEmpty()
                         join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                         from contact in contactInfo.DefaultIfEmpty()
                         where cate.CategoryId == categoryId && x.Status == NewsStatus.Published
                         orderby x.NewsId descending
                         select new NewsInfo
                         {
                             CategoryId = cate.CategoryId,
                             NewsId = x.NewsId,
                             Title = x.Title,
                             Headline = x.Headline,
                             Alias = x.Alias,
                             Summary = x.Summary,
                             FrontImage = x.FrontImage,
                             MainImage = x.MainImage,
                             MainText = x.MainText,
                             NavigateUrl = x.NavigateUrl,
                             Authors = x.Authors,
                             ListOrder = x.ListOrder,
                             Tags = x.Tags,
                             Source = x.Source,
                             TotalRates = x.TotalRates,
                             TotalViews = x.TotalViews,
                             PostedDate = x.PostedDate,
                             Status = x.Status,
                             CreatedDate = x.CreatedDate,
                             LastModifiedDate = x.LastModifiedDate,
                             CreatedByUserId = x.CreatedByUserId,
                             LastModifiedByUserId = x.LastModifiedByUserId,
                             Ip = x.Ip,
                             LastUpdatedIp = x.LastUpdatedIp,
                             CategoryName = cate.CategoryName,
                             FullName = contact.FirstName + " " + contact.LastName,
                             Category = cate
                         });
            var itemCount = (recordCount > query.Count()) ? query.Count() : recordCount;
            return query.Take(itemCount).AsEnumerable();
        }
        public IEnumerable<NewsInfo> GetOlderListByCategoryId(int categoryId, int selectedId, int recordCount)
        {
            var query = (from x in DataContext.Get<News>()
                         join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                         from cate in catelist.DefaultIfEmpty()
                         join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                         from profile in profileInfo.DefaultIfEmpty()
                         join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                         from contact in contactInfo.DefaultIfEmpty()
                         where cate.CategoryId == categoryId && x.Status == NewsStatus.Published && x.NewsId < selectedId
                         orderby x.NewsId descending
                         select new NewsInfo
                         {
                             CategoryId = cate.CategoryId,
                             NewsId = x.NewsId,
                             Title = x.Title,
                             Headline = x.Headline,
                             Alias = x.Alias,
                             Summary = x.Summary,
                             FrontImage = x.FrontImage,
                             MainImage = x.MainImage,
                             MainText = x.MainText,
                             NavigateUrl = x.NavigateUrl,
                             Authors = x.Authors,
                             ListOrder = x.ListOrder,
                             Tags = x.Tags,
                             Source = x.Source,
                             TotalRates = x.TotalRates,
                             TotalViews = x.TotalViews,
                             PostedDate = x.PostedDate,
                             Status = x.Status,
                             CreatedDate = x.CreatedDate,
                             LastModifiedDate = x.LastModifiedDate,
                             CreatedByUserId = x.CreatedByUserId,
                             LastModifiedByUserId = x.LastModifiedByUserId,
                             Ip = x.Ip,
                             LastUpdatedIp = x.LastUpdatedIp,
                             CategoryName = cate.CategoryName,
                             FullName = contact.FirstName + " " + contact.LastName,
                             Category = cate
                         });
            var itemCount = (recordCount > query.Count()) ? query.Count() : recordCount;
            return query.Take(itemCount).AsEnumerable();
        }
        public IEnumerable<NewsInfo> GetListByTotalViews(int recordCount)
        {
            var query = (from x in DataContext.Get<News>()
                         join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                         from cate in catelist.DefaultIfEmpty()
                         join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                         from profile in profileInfo.DefaultIfEmpty()
                         join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                         from contact in contactInfo.DefaultIfEmpty()
                         where x.Status == NewsStatus.Published && (x.TotalViews != null && x.TotalViews > 0)
                         orderby x.TotalViews descending
                         select new NewsInfo
                         {
                             CategoryId = cate.CategoryId,
                             NewsId = x.NewsId,
                             Title = x.Title,
                             Headline = x.Headline,
                             Alias = x.Alias,
                             Summary = x.Summary,
                             FrontImage = x.FrontImage,
                             MainImage = x.MainImage,
                             MainText = x.MainText,
                             NavigateUrl = x.NavigateUrl,
                             Authors = x.Authors,
                             ListOrder = x.ListOrder,
                             Tags = x.Tags,
                             Source = x.Source,
                             TotalRates = x.TotalRates,
                             TotalViews = x.TotalViews,
                             PostedDate = x.PostedDate,
                             Status = x.Status,
                             CreatedDate = x.CreatedDate,
                             LastModifiedDate = x.LastModifiedDate,
                             CreatedByUserId = x.CreatedByUserId,
                             LastModifiedByUserId = x.LastModifiedByUserId,
                             Ip = x.Ip,
                             LastUpdatedIp = x.LastUpdatedIp,
                             CategoryName = cate.CategoryName,
                             FullName = contact.FirstName + " " + contact.LastName,
                             Category = cate
                         });
            var itemCount = (recordCount > query.Count()) ? query.Count() : recordCount;
            return query.Take(itemCount).AsEnumerable();
        }
        public IEnumerable<NewsInfo> GetChildrenListByParentId(int? parentId, NewsStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            return (from x in DataContext.Get<News>()
                    join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                    from cate in catelist.DefaultIfEmpty()
                    join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                    from profile in profileInfo.DefaultIfEmpty()
                    join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                    from contact in contactInfo.DefaultIfEmpty()
                    where (parentId == null || cate.ParentId == parentId) && (status == null || x.Status == status)
                    orderby x.NewsId descending
                    orderby x.NewsId descending
                    select new NewsInfo
                    {
                        CategoryId = cate.CategoryId,
                        NewsId = x.NewsId,
                        Title = x.Title,
                        Headline = x.Headline,
                        Alias = x.Alias,
                        Summary = x.Summary,
                        FrontImage = x.FrontImage,
                        MainImage = x.MainImage,
                        MainText = x.MainText,
                        NavigateUrl = x.NavigateUrl,
                        Authors = x.Authors,
                        ListOrder = x.ListOrder,
                        Tags = x.Tags,
                        Source = x.Source,
                        TotalRates = x.TotalRates,
                        TotalViews = x.TotalViews,
                        PostedDate = x.PostedDate,
                        Status = x.Status,
                        CreatedDate = x.CreatedDate,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedByUserId = x.CreatedByUserId,
                        LastModifiedByUserId = x.LastModifiedByUserId,
                        Ip = x.Ip,
                        LastUpdatedIp = x.LastUpdatedIp,
                        CategoryName = cate.CategoryName,
                        FullName = contact.FirstName + " " + contact.LastName,
                        Category = cate
                    }).WithRecordCount(ref recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<NewsInfo> GetList(NewsStatus status, int recordCount)
        {
            var query = (from x in DataContext.Get<News>()
                         join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                         from cate in catelist.DefaultIfEmpty()
                         join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                         from profile in profileInfo.DefaultIfEmpty()
                         join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                         from contact in contactInfo.DefaultIfEmpty()
                         where x.Status == status
                         orderby x.NewsId descending
                         select new NewsInfo
                         {
                             CategoryId = cate.CategoryId,
                             NewsId = x.NewsId,
                             Title = x.Title,
                             Headline = x.Headline,
                             Alias = x.Alias,
                             Summary = x.Summary,
                             FrontImage = x.FrontImage,
                             MainImage = x.MainImage,
                             MainText = x.MainText,
                             NavigateUrl = x.NavigateUrl,
                             Authors = x.Authors,
                             ListOrder = x.ListOrder,
                             Tags = x.Tags,
                             Source = x.Source,
                             TotalRates = x.TotalRates,
                             TotalViews = x.TotalViews,
                             PostedDate = x.PostedDate,
                             Status = x.Status,
                             CreatedDate = x.CreatedDate,
                             LastModifiedDate = x.LastModifiedDate,
                             CreatedByUserId = x.CreatedByUserId,
                             LastModifiedByUserId = x.LastModifiedByUserId,
                             Ip = x.Ip,
                             LastUpdatedIp = x.LastUpdatedIp,
                             CategoryName = cate.CategoryName,
                             FullName = contact.FirstName + " " + contact.LastName,
                             Category = cate
                         });
            var itemCount = (recordCount > query.Count()) ? query.Count() : recordCount;
            return query.Take(itemCount).AsEnumerable();
        }
        public IEnumerable<NewsInfo> SearchByKeywords(string keywords, int recordCount)
        {
            string lowerKeywords = keywords.ToLower();

            var query = from x in DataContext.Get<News>()
                        join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                        from cate in catelist.DefaultIfEmpty()
                        join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                        from profile in profileInfo.DefaultIfEmpty()
                        join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                        from contact in contactInfo.DefaultIfEmpty()
                        where x.Headline.ToLower().Contains(lowerKeywords)
                        || x.Alias.Replace("-", " ").Contains(lowerKeywords)
                        || x.Authors.Contains(lowerKeywords)
                        || x.Summary.Contains(lowerKeywords)
                        || x.MainText.Contains(lowerKeywords)
                        || x.Source.Contains(lowerKeywords)
                        || x.Tags.Contains(lowerKeywords)
                        && x.Status == NewsStatus.Published
                        orderby x.NewsId descending
                        select new NewsInfo
                        {
                            CategoryId = cate.CategoryId,
                            NewsId = x.NewsId,
                            Title = x.Title,
                            Headline = x.Headline,
                            Alias = x.Alias,
                            Summary = x.Summary,
                            FrontImage = x.FrontImage,
                            MainImage = x.MainImage,
                            MainText = x.MainText,
                            NavigateUrl = x.NavigateUrl,
                            Authors = x.Authors,
                            ListOrder = x.ListOrder,
                            Tags = x.Tags,
                            Source = x.Source,
                            TotalRates = x.TotalRates,
                            TotalViews = x.TotalViews,
                            PostedDate = x.PostedDate,
                            Status = x.Status,
                            CreatedDate = x.CreatedDate,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedByUserId = x.CreatedByUserId,
                            LastModifiedByUserId = x.LastModifiedByUserId,
                            Ip = x.Ip,
                            LastUpdatedIp = x.LastUpdatedIp,
                            CategoryName = cate.CategoryName,
                            FullName = contact.FirstName + " " + contact.LastName,
                            Category = cate
                        };
            var itemCount = (recordCount > query.Count()) ? query.Count() : recordCount;
            return query.Take(itemCount).AsEnumerable();
        }
        public IEnumerable<NewsInfo> GetListByCategoryId(int categoryId, NewsStatus status, int recordCount)
        {
            var query = from x in DataContext.Get<News>()
                        join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                        from cate in catelist.DefaultIfEmpty()
                        join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                        from profile in profileInfo.DefaultIfEmpty()
                        join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                        from contact in contactInfo.DefaultIfEmpty()
                        where x.CategoryId == categoryId
                            && x.Status == status
                            && x.TotalViews > 0
                        orderby x.TotalViews descending
                        select new NewsInfo
                        {
                            CategoryId = cate.CategoryId,
                            NewsId = x.NewsId,
                            Title = x.Title,
                            Headline = x.Headline,
                            Alias = x.Alias,
                            Summary = x.Summary,
                            FrontImage = x.FrontImage,
                            MainImage = x.MainImage,
                            MainText = x.MainText,
                            NavigateUrl = x.NavigateUrl,
                            Authors = x.Authors,
                            ListOrder = x.ListOrder,
                            Tags = x.Tags,
                            Source = x.Source,
                            TotalRates = x.TotalRates,
                            TotalViews = x.TotalViews,
                            PostedDate = x.PostedDate,
                            Status = x.Status,
                            CreatedDate = x.CreatedDate,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedByUserId = x.CreatedByUserId,
                            LastModifiedByUserId = x.LastModifiedByUserId,
                            Ip = x.Ip,
                            LastUpdatedIp = x.LastUpdatedIp,
                            CategoryName = cate.CategoryName,
                            FullName = contact.FirstName + " " + contact.LastName,
                            Category = cate
                        };
            var itemCount = (recordCount > query.Count()) ? query.Count() : recordCount;
            return query.Take(itemCount).AsEnumerable();
        }
        public NewsInfo GetDetails(int newsId)
        {
            return (from x in DataContext.Get<News>()
                    join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                    from cate in catelist.DefaultIfEmpty()
                    join p in DataContext.Get<UserProfile>() on x.CreatedByUserId equals p.UserId into profileInfo
                    from profile in profileInfo.DefaultIfEmpty()
                    join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                    from contact in contactInfo.DefaultIfEmpty()
                    where x.NewsId == newsId
                    orderby x.TotalViews descending
                    select new NewsInfo
                    {
                        CategoryId = cate.CategoryId,
                        NewsId = x.NewsId,
                        Title = x.Title,
                        Headline = x.Headline,
                        Alias = x.Alias,
                        Summary = x.Summary,
                        FrontImage = x.FrontImage,
                        MainImage = x.MainImage,
                        MainText = x.MainText,
                        NavigateUrl = x.NavigateUrl,
                        Authors = x.Authors,
                        ListOrder = x.ListOrder,
                        Tags = x.Tags,
                        Source = x.Source,
                        TotalRates = x.TotalRates,
                        TotalViews = x.TotalViews,
                        PostedDate = x.PostedDate,
                        Status = x.Status,
                        CreatedDate = x.CreatedDate,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedByUserId = x.CreatedByUserId,
                        LastModifiedByUserId = x.LastModifiedByUserId,
                        Ip = x.Ip,
                        LastUpdatedIp = x.LastUpdatedIp,
                        CategoryName = cate.CategoryName,
                        FullName = contact.FirstName + " " + contact.LastName,
                        Category = cate
                    }).FirstOrDefault();
        }
        public News GetDetailByAlias(string alias)
        {
            return (from x in DataContext.Get<News>()
                    join y in DataContext.Get<NewsCategory>() on x.CategoryId equals y.CategoryId into catelist
                    from cate in catelist.DefaultIfEmpty()
                    where x.Alias.ToLower().Contains(alias.ToLower())
                    select x).FirstOrDefault();
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<News>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }

        public bool HasDataExisted(int categoryId, string title)
        {
            var query = DataContext.Get<News>().FirstOrDefault(c => c.CategoryId == categoryId && c.Title.ToUpper().Equals(title.ToUpper()));
            return (query != null);
        }
    }
}
