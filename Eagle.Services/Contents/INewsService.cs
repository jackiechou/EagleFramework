using System;
using System.Collections.Generic;
using Eagle.Core.Extension;
using Eagle.Core.Search;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Articles;
using Eagle.Services.Dtos.Contents.Feedbacks;

namespace Eagle.Services.Contents
{
    public interface INewsService : IBaseService
    {
        #region News Category

        IEnumerable<NewsInfoDetail> Search(NewsSearchEntry searchEntry, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<TreeGrid> GetNewsCategoryTreeGrid(string languageCode, NewsCategoryStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null, int? selectedId = null, bool? isRootShowed = false);
        SearchDataResult<NewsCategoryDetail> GetNewsCategories(string languageCode, string searchText, NewsCategoryStatus? status,
            out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        SearchDataResult<TreeGrid> GetNewsCategoryList(string languageCode, int? page, int? pageSize);
        string GenerateCategoryCode(int num);
        NewsCategoryDetail GetNewsCategoryByCode(string categoryCode);
        NewsCategoryDetail GetNewsCategoryDetail(int id);
        IEnumerable<TreeNodeDetail> PopulateHierachicalNewsCategoryDropDownList(string languageCode);

        IEnumerable<TreeDetail> GetNewsCategoryTree(string languageCode, NewsCategoryStatus? status = null, int? selectedId = null,
            bool? isRootShowed = true);
        IEnumerable<NewsCategoryDetail> GetNewsCategoryParentNodes();
        IEnumerable<TreeNodeDetail> GetChildren(int? parentId, string languageCode);
        void InsertNewsCategory(Guid userId, string languageCode, NewsCategoryEntry entry);
        void UpdateNewsCategory(Guid userId, string languageCode, NewsCategoryEditEntry entry);
        void UpdateNewsCategoryListOrder(Guid userId, int currentCategoryId, bool isUp);
        void UpdateNewsCategoryListOrder(Guid userId, NewsCategorySortOrderEntry entry);
        void UpdateNewsCategoryListOrders(Guid userId, NewsCategoryListOrderEntry entry);
        void UpdateNewsCategoryStatus(Guid userId, int id, NewsCategoryStatus status);
        #endregion

        #region NEWS

        IEnumerable<NewsInfoDetail> GetNewsList(string searchText, NewsStatus? status, out int recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);

        IEnumerable<NewsInfoDetail> GetNewsByCategoryId(int categoryId, NewsStatus? status, out int recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<NewsInfoDetail> GetNews(int number);
        IEnumerable<NewsInfoDetail> GetListByTotalViews(int recordCount);
        NewsInfoDetail GeNewsDetail(int id);
        NewsDetail InsertNews(Guid applicationId, Guid userId, int? vendorId, NewsEntry entry);
        void UpdateNews(Guid applicationId, Guid userId, int id, NewsEntry entry);
        void UpdateNewsListOrder(Guid userId, NewsSortOrderEntry entry);
        void UpdateNewsListOrders(Guid userId, NewsListOrderEntry entry);
        void UpdateNewsTotalViews(int id);
        void UpdateNewsTotalView(Guid userId, int id);
        void UpdateNewsTotalView(Guid userId, int id, int totalview);
        void UpdateNewsStatus(Guid userId, int id, NewsStatus status);
        void DeleteNews(int id);

        #endregion

        #region News Comment

        IEnumerable<NewsCommentInfoDetail> GetNewsComments(NewsCommentStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<NewsCommentDetail> GetNewsComments(int newsId, NewsCommentStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<NewsCommentDetail> GetNewsComments(int newsId, NewsCommentStatus? status);
        NewsCommentDetail GetNewsCommentDetail(int id);
        void InsertNewsComment(NewsCommentEntry entry);
        void UpdateNewsComment(int id, NewsCommentEntry entry);
        void UpdateNewsCommentStatus(int id, NewsCommentStatus status);
        void DeleteNewsComment(int id);

        #endregion

        #region News Rating

        int GetDefaultNewsRating(Guid applicationId);
        IEnumerable<NewsRatingDetail> GetNewsRatings(int newsId);

        decimal InsertNewsRating(NewsRatingEntry entry);
       
        #endregion

        #region Feedback

        IEnumerable<FeedbackDetail> GetFeedbacks(bool? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        void InsertFeedback(Guid applicationId, FeedbackEntry entry);
        void UpdateFeedbackStatus(int id, bool status);

        #endregion
    }
}
