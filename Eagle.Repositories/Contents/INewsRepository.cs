using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Articles;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface INewsRepository : IRepositoryBase<News>
    {
        IEnumerable<NewsInfo> Search(out int recordCount, string searchText = null, string author = null, int? categoryId = null, DateTime? fromDate = null, DateTime? toDate = null, NewsStatus? status = null, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<NewsInfo> GetList(string searchText, NewsStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<NewsInfo> GetNews(int number);
        IEnumerable<NewsInfo> GetListByCategoryId(int categoryId, NewsStatus? status, out int recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<NewsInfo> GetListByCategoryId(int categoryId, string beginDate, string endDate, NewsStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<NewsInfo> GetTopListByCategoryId(int categoryId, int recordCount);
        IEnumerable<NewsInfo> GetRandomList(int recordCount);
        IEnumerable<NewsInfo> GetNewerListByCategoryId(int categoryId, int selectedNewId, int recordCount);
        IEnumerable<NewsInfo> GetOlderListByCategoryId(int categoryId, int recordCount);
        IEnumerable<NewsInfo> GetOlderListByCategoryId(int categoryId, int selectedNewId, int recordCount);
        IEnumerable<NewsInfo> GetListByTotalViews(int recordCount);

        IEnumerable<NewsInfo> GetChildrenListByParentId(int? parentId, NewsStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<NewsInfo> GetList(NewsStatus status, int recordCount);
        IEnumerable<NewsInfo> SearchByKeywords(string keywords, int recordCount);
        IEnumerable<NewsInfo> GetListByCategoryId(int categoryId, NewsStatus status, int recordCount);
        NewsInfo GetDetails(int newsId);
        News GetDetailByAlias(string alias);
        int GetNewListOrder();
        bool HasDataExisted(int categoryId, string title);
        //string GetFrontImageById(int idx);
        //string GetMainImageById(int idx);
    }
}
