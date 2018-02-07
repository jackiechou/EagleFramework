using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework;
using Eagle.Resources;
using System.Web.Mvc;

namespace Eagle.Repositories.Services.Booking
{
    public class ServiceCategoryRepository : RepositoryBase<ServiceCategory>, IServiceCategoryRepository
    {
        public ServiceCategoryRepository(IDataContext dataContext) : base(dataContext) { }

        #region SELECT TREE
        public IEnumerable<TreeEntity> GetServiceCategorySelectTree(ServiceType typeId = ServiceType.Single, ServiceCategoryStatus? status = null, int? selectedId = null, bool? isRootShowed = false)
        {
            var list = (from p in DataContext.Get<ServiceCategory>()
                        where p.TypeId == typeId && (status == null || p.Status == status)
                        orderby p.ListOrder
                        select new TreeEntity
                        {
                            id = p.CategoryId,
                            key = p.CategoryId,
                            parentid = p.ParentId,
                            depth = p.Depth,
                            name = p.CategoryName,
                            title = p.CategoryName,
                            text = p.CategoryName,
                            tooltip = p.CategoryName,
                            hasChild = p.HasChild ?? false,
                            folder = p.HasChild ?? false,
                            lazy = p.HasChild ?? false,
                            expanded = p.HasChild ?? false,
                            selected = (selectedId != null && p.CategoryId == selectedId),
                            state = (p.Status == ServiceCategoryStatus.Active),
                        }).ToList();

            var recursiveObjects = RecursiveFillTopicSelectTree(list, 0);

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
            return recursiveObjects;
        }
        private List<TreeEntity> RecursiveFillTopicSelectTree(List<TreeEntity> elements, int? parentid)
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
                    children = RecursiveFillTopicSelectTree(elements, child.id)
                }));
            }
            return items;
        }

        #endregion

        public IEnumerable<ServiceCategory> GetListByStatus(ServiceCategoryStatus? status)
        {
            var lst = (from x in DataContext.Get<ServiceCategory>()
                where (status == null || x.Status == status)
                select x).AsEnumerable();
            return lst;
        }

        public IEnumerable<ServiceCategory> GetAllChildrenNodesOfSelectedNode(int id, ServiceCategoryStatus? status = null)
        {
            const string strCommand = @"EXEC Booking.ServiceCategory_GetAllChildrenNodesOfSelectedNode @id = {0}, @status = {1}";
            return DataContext.Get<ServiceCategory>(strCommand, id, status);
        }

        public SelectList GetServiceCategoryChildList(ServiceCategoryStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ServiceCategory>()
                       where ((status == null || c.Status == status) && c.ParentId.Value == (int)ServiceCategoryRoot.Default)
                       orderby c.ListOrder descending
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.CategoryName, Value = p.CategoryId.ToString(), Selected = (selectedValue != null && p.CategoryId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public SelectList GetServiceCategoryChildListByCode(string discountCode, ServiceCategoryStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ServiceCategory>()
                       join s in DataContext.Get<ServicePack>() on c.CategoryId equals s.CategoryId
                       join d in DataContext.Get<ServiceDiscount>() on s.DiscountId equals d.DiscountId
                       where ((status == null || c.Status == status) && c.ParentId.Value == (int)ServiceCategoryRoot.Default) && d.DiscountCode == discountCode
                       orderby c.ListOrder descending
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.CategoryName, Value = p.CategoryId.ToString(), Selected = (selectedValue != null && p.CategoryId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public bool HasDataExisted(string name, int? parentId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var query = DataContext.Get<ServiceCategory>().FirstOrDefault(c => c.ParentId == parentId &&
                c.CategoryName.Equals(name, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }
        public bool HasChild(int typeId)
        {
            var query = DataContext.Get<ServiceCategory>().FirstOrDefault(c => c.ParentId == typeId);
            return (query != null);
        }
    }
}
