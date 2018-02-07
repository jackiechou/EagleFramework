using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Articles;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Contents
{
    public class NewsCommentRepository : RepositoryBase<NewsComment>, INewsCommentRepository
    {
        public NewsCommentRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<NewsCommentInfo> GetNewsComments(NewsCommentStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = (from x in DataContext.Get<NewsComment>()
                         join y in DataContext.Get<News>() on x.NewsId equals y.NewsId into xyJoin
                         from cate in xyJoin.DefaultIfEmpty()
                         where (status == null || x.IsPublished == status.Value)
                         select new NewsCommentInfo
                         {
                             NewsId = x.NewsId,
                             CommentId = x.CommentId,
                             CommentName = x.CommentName,
                             CommentText = x.CommentText,
                             CreatedByEmail = x.CreatedByEmail,
                             IsReplied = x.IsReplied,
                             IsPublished = x.IsPublished,
                             Ip = x.Ip,
                             CreatedDate = x.CreatedDate
                         });
            return query.WithRecordCount(ref recordCount)
                                .WithSortingAndPaging(orderBy, page, pageSize);

        }
        public IEnumerable<NewsComment> GetNewsComments(int newsId, NewsCommentStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            return DataContext.Get<NewsComment>().Where(x => x.NewsId == newsId && (status == null || x.IsPublished == status.Value))
                               .WithRecordCount(ref recordCount)
                               .WithSortingAndPaging(orderBy, page, pageSize);

        }
        public IEnumerable<NewsComment> GetNewsComments(int newsId, NewsCommentStatus? status = null)
        {
            return DataContext.Get<NewsComment>().Where(x => x.NewsId == newsId && (status == null || x.IsPublished == status.Value));
        }
        public bool HasDataExisted(string commentName, string email)
        {
            var query = DataContext.Get<NewsComment>().FirstOrDefault(c => c.CommentName.ToUpper().Equals(commentName.ToUpper())
                   && (email == null || c.CreatedByEmail.Equals(email)));
            return (query != null);
        }
    }
}
