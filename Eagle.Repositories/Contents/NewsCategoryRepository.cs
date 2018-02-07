using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Common.Utilities;
using Eagle.Core.Common;
using Eagle.Core.Extension;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Articles;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Contents
{
    public class NewsCategoryRepository : RepositoryBase<NewsCategory>, INewsCategoryRepository
    {
        public NewsCategoryRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<TreeGrid> GetNewsCategoryTreeGrid(string languageCode, NewsCategoryStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null, int? selectedId = null, bool? isRootShowed = false)
        {
            var list = (from x in DataContext.Get<NewsCategory>()
                        where (status == null || x.Status == status)
                    && x.LanguageCode == languageCode
                        orderby x.ListOrder ascending
                        select new TreeGrid
                        {
                            id = x.CategoryId.ToString(),
                            parentId = x.ParentId.ToString(),
                            level = x.Depth,
                            name = x.CategoryName,
                            title = x.CategoryName,
                            text = x.CategoryName,
                            state = new TreeGridState
                            {
                                opened = (x.HasChild != null && x.HasChild == true),
                                selected = (selectedId != null && x.CategoryId == selectedId),
                            },
                            data = new TreeGridData
                            {
                               status = (
                                  x.Status ==  NewsCategoryStatus.InActive ? LanguageResource.InActive :
                                  x.Status ==  NewsCategoryStatus.Active ? LanguageResource.Active : LanguageResource.Published
                                ),
                                action = @"<a data-id='" + x.CategoryId + "' title=" + LanguageResource.Reset + " class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='" + x.CategoryId + "' title = " + LanguageResource.Edit + " class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                            }
                        }).ToList();

            var recursiveObjects = new List<TreeGrid>();
            if (list.Any())
            {
                recursiveObjects = RecursiveFillTreeGrid(list, "0");
                if (isRootShowed != null && isRootShowed == true)
                {
                    recursiveObjects.Insert(0, new TreeGrid
                    {
                        id = "0",
                        parentId = "0",
                        level = 1,
                        name = LanguageResource.Root,
                        title = LanguageResource.Root,
                        text = LanguageResource.Root,
                        type = "glyphicon glyphicon-folder-close",
                        icon = "glyphicon glyphicon-file",
                        state = new TreeGridState
                        {
                            opened = false,
                            selected = false,
                        },
                        data = new TreeGridData
                        {
                            status = LanguageResource.Published,
                            action = @"<a data-id='0' title='" + LanguageResource.Reset + "' class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='0' title='" + LanguageResource.Edit + "' class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                        }
                    });
                }
            }
            else
            {
                recursiveObjects.Insert(0, new TreeGrid
                {
                    id = "0",
                    parentId = "0",
                    level = 1,
                    name = LanguageResource.NonSpecified,
                    title = LanguageResource.NonSpecified,
                    text = LanguageResource.NonSpecified,
                    type = "glyphicon glyphicon-folder-close",
                    icon = "glyphicon glyphicon-file",
                    state = new TreeGridState
                    {
                        opened = false,
                        selected = false,
                    },
                    data = new TreeGridData
                    {
                        status = LanguageResource.Published,
                        action = @"<a data-id='0' title='" + LanguageResource.Reset + "' class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='0' title = '" + LanguageResource.Edit + "' class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                    }
                });
            }

            return recursiveObjects.WithRecordCount(out recordCount);
            //return recursiveObjects.WithRecordCount(out recordCount)
            // .WithSortingAndPaging(orderBy, page, pageSize);
        }
        private List<TreeGrid> RecursiveFillTreeGrid(List<TreeGrid> elements, string parentid)
        {
            if (elements == null) return null;
            List<TreeGrid> items = new List<TreeGrid>();
            List<TreeGrid> children = elements.Where(p => p.parentId == parentid).Select(
               p => new TreeGrid
               {
                   id = p.id,
                   parentId = p.parentId,
                   level = p.level,
                   name = p.name,
                   title = p.title,
                   text = p.text,
                   state = new TreeGridState
                   {
                       opened = (p.hasChild != null && p.hasChild == true),
                       selected = p.state.selected,
                   },
                   data = new TreeGridData
                   {
                       status = p.data.status,
                       action = @"<a data-id='" + p.id + "' title='" + LanguageResource.Reset + "' class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='" + p.id + "' title = '" + LanguageResource.Edit + "' class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                   },
               }).ToList();

            if (children.Count > 0)
            {
                items.AddRange(children.Select(child => new TreeGrid
                {
                    id = child.id,
                    parentId = child.parentId,
                    level = child.level,
                    name = child.name,
                    title = child.title,
                    text = child.text,
                    state = new TreeGridState
                    {
                        opened = (child.hasChild != null && child.hasChild == true),
                        selected = child.state.selected,
                    },
                    data = new TreeGridData
                    {
                        status = child.data.status,
                        action = @"<a data-id='" + child.id + "' title='" + LanguageResource.Reset + "' class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='" + child.id + "' title = '" + LanguageResource.Edit + "' class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                    },
                    children = RecursiveFillTreeGrid(elements, child.id)
                }));
            }
            return items;
        }

        public IEnumerable<NewsCategory> GetNewsCategories(string languageCode, string searchText, NewsCategoryStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var result = (from x in DataContext.Get<NewsCategory>()
                          where (status == null || x.Status == status)
                          && x.LanguageCode == languageCode
                          && (string.IsNullOrEmpty(searchText) || x.CategoryName.ToLower().Contains(searchText.ToLower()))
                          select x);
            return result.OrderByDescending(t => t.CategoryId)
               .WithRecordCount(out recordCount)
               .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<NewsCategory> GetListByLanguageCode(string languageCode)
        {
            return DataContext.Get<NewsCategory>().OrderBy(p => p.ListOrder).Where(p => p.LanguageCode == languageCode).AsEnumerable();
        }
        public IEnumerable<NewsCategory> GetNewsCategories(string languageCode, NewsCategoryStatus? status)
        {
            var lst = (from x in DataContext.Get<NewsCategory>()
                       where (status == null || x.Status == status)
                             && (string.IsNullOrEmpty(languageCode) || x.LanguageCode == languageCode)
                       select x).AsEnumerable();
            return lst;
        }
        public IEnumerable<NewsCategory> GetTreeNodes(string languageCode, string searchText, int? categoryId, NewsCategoryStatus? status,
            out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            const string strCommand = @"EXEC dbo.NewsCategory_GetTreeNodes @languageCode = {0}, @categoryId = {1}, @status = {2}";
            var lst = DataContext.Get<NewsCategory>(strCommand, languageCode, categoryId, status).Where(x =>
             (string.IsNullOrEmpty(searchText) || x.CategoryName.ToLower().Contains(searchText.ToLower())));
            return lst.WithRecordCount(out recordCount)
               .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<NewsCategory> GetAllParentNodesOfSelectedNode(string languageCode, Guid categoryCode)
        {
            const string strCommand = @"EXEC dbo.NewsCategory_GetAllParentNodesOfSelectedNode @LanguageCode = {0}, @CategoryCode = {1}";
            return DataContext.Get<NewsCategory>(strCommand, languageCode, categoryCode).AsEnumerable();
        }
        public IEnumerable<NewsCategory> GetAllChildrenNodesOfSelectedNode(string languageCode, Guid categoryCode)
        {
            const string strCommand = @"EXEC dbo.NewsCategory_GetAllChildrenNodesOfSelectedNode @LanguageCode = {0}, @CategoryCode = {1}";
            return DataContext.Get<NewsCategory>(strCommand, languageCode, categoryCode).AsEnumerable();
        }
        public IEnumerable<NewsCategory> GetAllChildrenNodesOfSelectedNode(string languageCode, string searchText, int? categoryId, NewsCategoryStatus? status=null)
        {
            const string strCommand = @"EXEC dbo.NewsCategory_GetAllChildrenNodes @languageCode = {0}, @categoryId = {1}, @status = {2}";
            return DataContext.Get<NewsCategory>(strCommand, languageCode, categoryId, status).Where(x =>
             (string.IsNullOrEmpty(searchText) || x.CategoryName.ToLower().Contains(searchText.ToLower())));
        }
        public IEnumerable<NewsCategory> GetAllParentNodesOfSelectedNodeStatus(string languageCode, Guid categoryCode, NewsCategoryStatus? status = null)
        {
            const string strCommand = @"EXEC dbo.NewsCategories_GetAllParentNodesOfSelectedNodeStatus @LanguageCode = {0}, @CategoryCode = {1}, @Status = {2}";
            return DataContext.Get<NewsCategory>(strCommand, languageCode, categoryCode, status).AsEnumerable();
        }
        public IEnumerable<NewsCategory> GetAllChildrenNodesOfSelectedNodeStatus(string languageCode, Guid categoryCode, NewsCategoryStatus? status = null)
        {
            const string strCommand = @"EXEC dbo.NewsCategory_GetAllChildrenNodesOfSelectedNode @LanguageCode = {0}, @CategoryCode = {1}, @Status = {2}";
            return DataContext.Get<NewsCategory>(strCommand, languageCode, categoryCode, status).AsEnumerable();
        }
        public NewsCategory GetDetailsByCode(string categoryCode)
        {
            return DataContext.Get<NewsCategory>()
                    .FirstOrDefault(c => c.CategoryCode.ToUpper().Equals(categoryCode.ToUpper()));
        }
        public int GenerateNewCategoryId()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<NewsCategory>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
        public string GenerateCategoryCode(int num)
        {
            int? maxId = (from u in DataContext.Get<NewsCategory>() select u.CategoryId).Max() + 1;
            return StringUtils.GenerateCode(maxId.ToString(), num);
        }
        public string GenerateCategoryCode(int num, string sid)
        {
            //    int? maxId = (from u in DataContext.Get<NewsCategory>() select u.CategoryId).Max() + 1;
            //string _MaxId = StringUtils.GenerateCode(maxId.ToString(), num);
            return $"{num}-{sid.Substring(0, 5).ToUpper()}";
        }
        public bool HasDataExisted(string categoryName, int? parentId)
        {
            if (string.IsNullOrEmpty(categoryName)) return false;
            var query = DataContext.Get<NewsCategory>().FirstOrDefault(c => (parentId == null || c.ParentId == parentId) && (c.CategoryName.ToUpper().Equals(categoryName.ToUpper())));
            return (query != null);
        }
        public bool IsCodeExisted(string categoryCode)
        {
            var query = DataContext.Get<NewsCategory>().FirstOrDefault(c => c.CategoryCode.ToUpper().Equals(categoryCode.ToUpper()));
            return (query != null);
        }
        public bool IsIdExisted(int id)
        {
            var query = DataContext.Get<NewsCategory>().FirstOrDefault(c => c.CategoryId.Equals(id));
            return (query != null);
        }

        public bool HasChild(int categoryId)
        {
            var query = DataContext.Get<NewsCategory>().FirstOrDefault(c => c.ParentId == categoryId);
            return (query != null);
        }

        #region HierachicalDropDownList=============================================================================================================================
        //public List<TreeNode> PopulateHierachicalDropDownList(string languageCode)
        //{
        //    List<TreeNode> list = DataContext.Get<NewsCategory>().OrderBy(p => p.ListOrder).Where(p => p.LanguageCode == languageCode).Select(p => new TreeNode()
        //    {
        //        Id = p.Id,
        //        Text = p.CategoryName,
        //        ParentId = p.ParentId
        //    }).ToList();
        //    return UnitOfWork.BaseRepository.RecursiveFillTreeNodes(list, null);
        //}

        //public List<TreeNode> GetTreeListByParentNode(int parentId)
        //{
        //    //var Lineage = DataContext.Get<NewsCategory>().Where(p => p.CategoryId == CategoryId).Select(p => p.Lineage).FirstOrDefault();
        //    //var allChildenCompany = DataContext.Get<NewsCategory>().Where(p => p.Lineage.Contains(Lineage)).Select(p => p.CategoryId).ToList();
        //    List<TreeNode> list = DataContext.Get<NewsCategory>().OrderBy(p => p.ListOrder).Select(p => new TreeNode()
        //    {
        //        Id = p.Id,
        //        Text = p.CategoryName,
        //        ParentId = p.ParentId
        //    }).ToList();
        //    List<TreeNode> recursiveObjects = BaseRepository.RecursiveFillTreeNodes(list, parentId);
        //    return recursiveObjects;
        //}


        //public List<TreeEntity> GetTreeList(string languageCode)
        //{
        //    List<TreeEntity> list = DataContext.Get<NewsCategory>().OrderBy(p => p.ListOrder).Where(p => p.LanguageCode == languageCode).Select(p => new TreeEntity()
        //    {
        //        Key = p.Id,
        //        Title = p.CategoryName,
        //        ParentId = p.ParentId,
        //        Depth = p.Depth,
        //        Folder = (p.Depth > 1),
        //        Lazy = (p.Depth > 1),
        //        Expanded = (p.Depth > 1),
        //        Tooltip = p.Description
        //    }).ToList();
        //    List<TreeEntity> recursiveObjects = UnitOfWork.BaseRepository.RecursiveFillTreeEntities(list, null);
        //    return recursiveObjects;
        //}

        public IEnumerable<NewsCategory> GetParentNodes()
        {
            return DataContext.Get<NewsCategory>().Where(p => p.ParentId == null || p.ParentId == 0).OrderBy(p => p.ListOrder).ToList();
        }

        public List<int> GetTreeIdListByNodeId(int? id)
        {
            List<int> children = new List<int>();
            if (id != null && id > 0)
            {
                var lineage = DataContext.Get<NewsCategory>().Where(p => p.CategoryId == id).Select(p => p.Lineage).FirstOrDefault();
                children = DataContext.Get<NewsCategory>().Where(p => p.Lineage.Contains(lineage)).Select(p => p.CategoryId).ToList();
            }
            return children;
        }


        #endregion =============================================================================================================================

        public NewsCategory GetDetailsByCategoryCode(string categoryCode)
        {
            var query = (from x in DataContext.Get<NewsCategory>()
                         where x.CategoryCode.Trim() == categoryCode
                         select x).SingleOrDefault();
            return query;
        }

        public NewsCategory FindByCode(string categoryCode)
        {
            var query = (from x in DataContext.Get<NewsCategory>()
                         where x.CategoryCode.Trim() == categoryCode
                         select x).SingleOrDefault();
            return query;
        }

        public NewsCategory GetNextCategory(int currentCategoryId)
        {
            return (from x in DataContext.Get<NewsCategory>()
                    where x.CategoryId > currentCategoryId
                    orderby x.ListOrder descending
                    select x).FirstOrDefault();
        }


        public NewsCategory GetPreviousCategory(int currentCategoryId)
        {

            return (from x in DataContext.Get<NewsCategory>()
                    where x.CategoryId < currentCategoryId
                    orderby x.ListOrder descending
                    select x).FirstOrDefault();
        }
    }
}
