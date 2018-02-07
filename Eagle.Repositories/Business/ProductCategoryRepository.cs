using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.Entities.Common;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Business
{
    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(IDataContext dataContext) : base(dataContext)
        {
        }
        public IEnumerable<ProductCategory> GetList(string languageCode, int vendorId, ProductCategoryStatus? status, ref int? recordCount,
      string orderBy = null, int? page = null, int? pageSize = null)
        {
            const string strCommand = @"EXEC [Production].[Product_Category_GetListByStatus] @languageCode={0}, @vendorId={1}, @status = {2}";
            var lst = DataContext.Get<ProductCategory>(strCommand, languageCode, vendorId, status);
            return lst.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<ProductCategory> GetList(string languageCode, int vendorId, Guid categoryCode, ProductCategoryStatus? status, ref int? recordCount,
      string orderBy = null, int? page = null, int? pageSize = null)
        {
            const string strCommand = @"EXEC [Production].[Product_Category_GetListByCategoryCodeStatus] @languageCode={0},  @vendorId={1}, @categoryCode = {2}, @status = {3}";
            var lst = DataContext.Get<ProductCategory>(strCommand, languageCode, vendorId, categoryCode, status);
            return lst.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        #region SELECT TREE
        public IEnumerable<TreeEntity> GetProductCategorySelectTree(ProductCategoryStatus? status, int? selectedId, bool? isRootShowed = false)
        {
            var list = (from p in DataContext.Get<ProductCategory>()
                        where status == null || p.Status == status
                        orderby p.ViewOrder ascending
                        select new TreeEntity
                        {
                            id = p.CategoryId,
                            key = p.CategoryId,
                            parentid = p.ParentId,
                            depth = p.Depth,
                            name = p.CategoryName,
                            title = p.CategoryName,
                            text = p.CategoryName,
                            tooltip = p.Description,
                            hasChild = p.HasChild ?? false,
                            folder = p.HasChild ?? false,
                            lazy = p.HasChild ?? false,
                            expanded = p.HasChild ?? false,
                            selected = (selectedId != null && selectedId > 0 && p.CategoryId == selectedId),
                            state = (p.Status == ProductCategoryStatus.Active),
                        }).ToList();

            var recursiveObjects = RecursiveFillProductCategorySelectTree(list, 0);

            if (isRootShowed != null && isRootShowed == true)
            {
                recursiveObjects.Insert(0, new TreeEntity
                {
                    id = 0,
                    key = 0,
                    parentid = 0,
                    depth = 1,
                    name = LanguageResource.Root,
                    title = LanguageResource.Root,
                    text = LanguageResource.Root,
                    tooltip = LanguageResource.Root,
                    hasChild = recursiveObjects.Any(),
                    folder = true,
                    lazy = true,
                    expanded = true,
                    selected = (selectedId != null && selectedId == 0),
                    state = true
                });
            }
            else
            {
                if (selectedId == 0)
                {
                    recursiveObjects.Insert(0, new TreeEntity
                    {
                        id = 0,
                        key = 0,
                        parentid = 0,
                        depth = 1,
                        name = LanguageResource.Root,
                        title = LanguageResource.Root,
                        text = LanguageResource.Root,
                        tooltip = LanguageResource.Root,
                        hasChild = recursiveObjects.Any(),
                        folder = true,
                        lazy = true,
                        expanded = true,
                        selected = true,
                        state = true
                    });
                }
            }

            return recursiveObjects;
        }
        private List<TreeEntity> RecursiveFillProductCategorySelectTree(List<TreeEntity> elements, int? parentid)
        {
            if (elements == null) return null;
            List<TreeEntity> items = new List<TreeEntity>();
            List<TreeEntity> children = elements.Where(p => p.parentid == parentid).Select(
               p => new TreeEntity
               {
                   id = p.id,
                   key = p.key,
                   parentid = p.parentid,
                   depth = p.depth,
                   name = p.name,
                   title = p.title,
                   text = p.text,
                   tooltip = p.tooltip,
                   hasChild = p.hasChild,
                   folder = p.folder,
                   lazy = p.lazy,
                   expanded = p.expanded,
                   selected = p.selected,
                   state = p.state
               }).ToList();

            if (children.Count > 0)
            {
                items.AddRange(children.Select(child => new TreeEntity
                {
                    id = child.id,
                    key = child.key,
                    parentid = child.parentid,
                    depth = child.depth,
                    name = child.name,
                    title = child.title,
                    text = child.text,
                    tooltip = child.tooltip,
                    hasChild = child.hasChild,
                    folder = child.folder,
                    lazy = child.lazy,
                    expanded = child.expanded,
                    selected = child.selected,
                    state = child.state,
                    children = RecursiveFillProductCategorySelectTree(elements, child.id)
                }));
            }
            return items;
        }

        #endregion
        public ProductCategory FindByCategoryCode(string categoryCode)
        {
            return DataContext.Get<ProductCategory>()
                .FirstOrDefault(c => c.CategoryCode.ToUpper().Equals(categoryCode.ToUpper()));
        }

        public ProductCategory FindByCategoryName(string categoryName)
        {
            return DataContext.Get<ProductCategory>()
                .FirstOrDefault(c => c.CategoryName.ToUpper().Equals(categoryName.ToUpper()));
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<ProductCategory>() select (int)u.ViewOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }

        public bool HasDataExisted(string name, int? parentId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var query = DataContext.Get<ProductCategory>().FirstOrDefault(c => c.ParentId == parentId &&
                c.CategoryName.Equals(name, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }
    }
}
