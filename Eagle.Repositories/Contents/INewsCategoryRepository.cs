using System;
using System.Collections.Generic;
using Eagle.Core.Extension;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Articles;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface INewsCategoryRepository : IRepositoryBase<NewsCategory>
    {
        IEnumerable<TreeGrid> GetNewsCategoryTreeGrid(string languageCode, NewsCategoryStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null, int? selectedId = null, bool? isRootShowed = false);

        IEnumerable<NewsCategory> GetNewsCategories(string languageCode, string searchText, NewsCategoryStatus? status,
            out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<NewsCategory> GetListByLanguageCode(string languageCode);
        IEnumerable<NewsCategory> GetNewsCategories(string languageCode, NewsCategoryStatus? status);
        IEnumerable<NewsCategory> GetTreeNodes(string languageCode, string searchText, int? categoryId,
            NewsCategoryStatus? status,
            out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<NewsCategory> GetAllParentNodesOfSelectedNode(string languageCode, Guid categoryCode);
        IEnumerable<NewsCategory> GetAllChildrenNodesOfSelectedNode(string languageCode, Guid categoryCode);

        IEnumerable<NewsCategory> GetAllChildrenNodesOfSelectedNode(string languageCode, string searchText,
            int? categoryId, NewsCategoryStatus? status = null);
        IEnumerable<NewsCategory> GetAllParentNodesOfSelectedNodeStatus(string languageCode, Guid categoryCode, NewsCategoryStatus? status = null);
        IEnumerable<NewsCategory> GetAllChildrenNodesOfSelectedNodeStatus(string languageCode, Guid categoryCode, NewsCategoryStatus? status = null);
        IEnumerable<NewsCategory> GetParentNodes();
        NewsCategory GetDetailsByCode(string categoryCode);
        int GenerateNewCategoryId();
        string GenerateCategoryCode(int num);
        string GenerateCategoryCode(int num, string sid);
        bool HasDataExisted(string categoryName, int? parentId);
        bool IsCodeExisted(string categoryCode);
        bool HasChild(int categoryId);
        NewsCategory GetDetailsByCategoryCode(string categoryCode);
        NewsCategory FindByCode(string categoryCode);
        NewsCategory GetNextCategory(int currentCategoryId);
        NewsCategory GetPreviousCategory(int currentCategoryId);
    }
}