using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Articles;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface INewsCommentRepository : IRepositoryBase<NewsComment>
    {
        IEnumerable<NewsCommentInfo> GetNewsComments(NewsCommentStatus? status, ref int? recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);
        IEnumerable<NewsComment> GetNewsComments(int newsId, NewsCommentStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<NewsComment> GetNewsComments(int newsId, NewsCommentStatus? status);
        bool HasDataExisted(string commentName, string email);
    }
}
