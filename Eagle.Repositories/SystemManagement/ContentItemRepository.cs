using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class ContentItemRepository : RepositoryBase<ContentItem>, IContentItemRepository
    {
        public ContentItemRepository(IDataContext dataContext) : base(dataContext) { }

        #region Tree ====================================================================================================
        public IEnumerable<ContentTreeModel> GetTreeList(int contentTypeId)
        {
            var lst = DataContext.Get<ContentItem>().Where(m => m.ContentTypeId == contentTypeId).OrderBy(m => m.ContentTypeId).ToList();
            var list = lst.Select(m => new ContentTreeModel
            {
                Id = m.ContentItemId,
                Key = m.ContentItemId,
                Name = m.ItemText,
                Tooltip = m.ItemText,
            }).ToList();
            var recursiveObjects = RecursiveFillTree(list, null);
            return recursiveObjects;
        }

        public List<ContentTreeModel> RecursiveFillTree(List<ContentTreeModel> list, int? id)
        {
            List<ContentTreeModel> items = new List<ContentTreeModel>();
            var nodes = list.Where(m => m.ParentId == id).Select(
               m => new ContentTreeModel
               {
                   Id = m.Id,
                   Key = m.Key,
                   ParentId = m.ParentId,
                   Name = m.Name,
                   Text = m.Text,
                   Title = m.Title,
                   Tooltip = m.Tooltip,
                   IsParent = m.IsParent,
                   Open = m.Open
               }).ToList();

            if (nodes.Count > 0)
            {
                items.AddRange(nodes.Select(child => new ContentTreeModel()
                {
                    Id = child.Id, Key = child.Key, ParentId = child.ParentId, Name = child.Name, Text = child.Text, Title = child.Title, Tooltip = child.Tooltip, IsParent = child.IsParent, Open = child.Open, Children = RecursiveFillTree(list, child.Id)
                }));
            }
            return items;
        }
        #endregion ==========================================================================================================================

        #region Content Items----------------------------------------------------------------------------------------------
        public bool HasDataExisted(int contentTypeId, string itemName)
        {
            var query = DataContext.Get<ContentItem>().FirstOrDefault(c => c.ContentTypeId.Equals(contentTypeId)
                   && c.ItemText.ToUpper() == itemName.ToUpper());
            return (query != null);
        }

        public SelectList PopulateContentItemsByPageToDropDownList(string selectedValue, bool isShowSelectText = false)
        {
            var lst = (from p in DataContext.Get<ContentItem>().AsEnumerable()
                   where p.ContentTypeId == (int)ContentTypeSetting.Page
                   select new SelectListItem
                   {
                       Text = p.ItemText,
                       Value = p.ContentItemId.ToString()
                   }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText)
                    lst.Insert(0, new SelectListItem() {Value = "-1", Text = $"-- {LanguageResource.SelectContentItem} --"});
            }
            else
            {
                lst.Insert(0, new SelectListItem() {Value = "-1", Text = $"-- {LanguageResource.None} --"});
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateContentItemsByModuleToDropDownList(string selectedValue, bool isShowSelectText = false)
        {
            var lst = (from p in DataContext.Get<ContentItem>().AsEnumerable()
                    where p.ContentTypeId == (int)ContentTypeSetting.Module
                    select new SelectListItem
                    {
                        Text = p.ItemText,
                        Value = p.ContentItemId.ToString()

                    }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText)
                    lst.Insert(0, new SelectListItem() { Value = "-1", Text = $"-- {LanguageResource.SelectContentItem} --" });
            }
            else
            {
                lst.Insert(0, new SelectListItem() { Value = "-1", Text = $"-- {LanguageResource.None} --" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);

        }
        #endregion ------------------------------------------------------------------------------------------------------

        #region Bind DropdownList ===================================================================================
        // dùng cho bind dropdownlist
        public List<AutoComplete> GetContentDropDownListByPage(string searchTerm, int pageSize, int pageNum)
        {
            var lst = DataContext.Get<ContentItem>()
                           .Where(c => c.ItemText.Contains(searchTerm) && c.ContentTypeId == (int)ContentTypeSetting.Page)
                           .Select(c => new AutoComplete
                           {
                               Id = c.ContentItemId,
                               Name = c.ItemKey,
                               Text = c.ItemText,
                               Level = 1
                           }).OrderBy(c => c.Name)
                    .Skip(pageSize * (pageNum - 1))
                    .Take(pageSize)
                    .ToList();
            return lst;
        }

        public List<AutoComplete> GetContentDropDownListByModule(string searchTerm, int pageSize, int pageNum)
        {
            var lst = DataContext.Get<ContentItem>()
                           .Where(c => c.ItemText.Contains(searchTerm) && c.ContentTypeId == (int)ContentTypeSetting.Module)
                           .Select(c => new AutoComplete
                           {
                               Id = c.ContentItemId,
                               Name = c.ItemKey,
                               Text = c.ItemText,
                               Level = 1
                           }).OrderBy(c => c.Name)
                    .Skip(pageSize * (pageNum - 1))
                    .Take(pageSize)
                    .ToList();
            return lst;
        }
        #endregion ==================================================================================================
        public bool IsIdExisted(int id)
        {
            var query = DataContext.Get<ContentItem>().Where(c => c.ContentItemId==id);
            return (query.Any());
        }

        public IEnumerable<ContentItem> GetList(int contentTypeId)
        {
            return (from c in DataContext.Get<ContentItem>()
                    join t in DataContext.Get<ContentType>() on c.ContentTypeId equals t.ContentTypeId into type
                    from t in type.DefaultIfEmpty()
                    where c.ContentTypeId == contentTypeId
                    select c).OrderByDescending(c => c.ContentItemId).AsEnumerable();
        }
        public ContentItem GetDetails(int contentItemId)
        {
            return (from p in DataContext.Get<ContentItem>() where p.ContentItemId == contentItemId select p).FirstOrDefault();
        }
    }
}
