using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class PageRepository : RepositoryBase<Page>, IPageRepository
    {
        public PageRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<Page> GetList(PageStatus? status)
        {
            return (from p in DataContext.Get<Page>() where p.IsActive == status select p).AsEnumerable();
        }
        public IEnumerable<Page> GetListByPageTypeIdAndKeywords(string keywords, PageType? pageTypeId, PageStatus? status)
        {
            var query = (from x in DataContext.Get<Page>() select x);

            if (pageTypeId != null)
            {
                query = query.Where(x => x.PageTypeId == pageTypeId);
            }

            if (status != null)
            {
                query = query.Where(x => x.IsActive == status);
            }

            if (!string.IsNullOrEmpty(keywords))
                query = query.Where(x => x.PageName.ToLower().Contains(keywords.ToLower()) || x.PageTitle.ToLower().Contains(keywords.ToLower()));

            return query.AsEnumerable();
        }
        public IEnumerable<Page> GetListByPageTypeId(PageType pageTypeId, PageStatus? status)
        {
            return (from p in DataContext.Get<Page>()
                    where p.PageTypeId == pageTypeId
                    && (status == null || p.IsActive == status)
                    select p).AsEnumerable();
        }
        public IEnumerable<AutoComplete> GetAutoCompleteList(out int recordCount, string searchTerm, PageType? pageTypeId, string orderBy = null, int? page = null, int? pageSize = null)
        {
            return (from x in DataContext.Get<Page>()
                       where pageTypeId==null || x.PageTypeId == pageTypeId
                       && (string.IsNullOrEmpty(searchTerm)
                       || x.PageName.ToLower().Contains(searchTerm.ToLower())
                       || x.PageTitle.ToLower().Contains(searchTerm.ToLower()))
                       select new AutoComplete
                       {
                           Id = x.PageId,
                           Name = x.PageName,
                           Text = x.PageTitle,
                           Level = 1
                       }).WithRecordCount(out recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<Page> Search(string keywords, PageType? pageTypeId, PageStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = (from x in DataContext.Get<Page>() select x);

            if (status != null)
                query = query.Where(x => x.IsActive == status);

            if (pageTypeId != null)
                query = query.Where(x => x.PageTypeId == pageTypeId);

            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(x => x.PageName.ToLower().Contains(keywords.ToLower())
                                         || x.PageTitle.ToLower().Contains((keywords).ToLower()));
            }
            return query.WithRecordCount(out recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public Page GetDetails(int id)
        {
            var entity = (from p in DataContext.Get<Page>()
                          where p.PageId == id
                          select p).FirstOrDefault();
            return entity;
        }

        #region Load Data========================================================================================================================================
       
        public SelectList PopulatePageTypeSelectList(int? selectedValue, bool isShowSelectText = false)
        {
            var lst = (from PageType x in Enum.GetValues(typeof(PageType)).Cast<PageType>()
                        select new SelectListItem
                        {
                            Text = x.ToString(),
                            Value = ((int)x).ToString(),
                            Selected = x.Equals(selectedValue)
                        }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectPageType} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        public MultiSelectList PopulatePageMultiSelectList(PageType pageTypeId, PageStatus? status = null, List<string> selectedValues = null)
        {
            var pages = (from m in DataContext.Get<Page>()
                where m.PageTypeId == pageTypeId && (status == null || m.IsActive == status)
                orderby m.ListOrder
                select m).ToList();

            var list = pages.Select(x => new SelectListItem
            {
                Text = x.PageTitle,
                Value = x.PageId.ToString(),
                Selected = selectedValues != null && selectedValues.Contains(x.PageId.ToString())
            });
            return new MultiSelectList(list, "Value", "Text", selectedValues);
        }

        public MultiSelectList PopulatePageMultiSelectList(PageType pageTypeId, PageStatus? status=null, int? moduleId=null, List<string> selectedValues =null)
        {
            var pages = (from m in DataContext.Get<Page>()
                        where m.PageTypeId == pageTypeId
                              && (status == null || m.IsActive == status)
                        select m).ToList();

            if (moduleId != null && moduleId > 0)
            {
                var selectedPages = (from pm in DataContext.Get<PageModule>()
                    join p in DataContext.Get<Page>() on pm.PageId equals p.PageId into pageLst
                    from pl in pageLst.DefaultIfEmpty()
                    where pm.ModuleId == moduleId && pl.PageTypeId == pageTypeId
                    select pl).ToList();

                var selectedPageIds = selectedPages.Select(x => x.PageId).ToList();
                var pageIds = pages.Select(x => x.PageId).ToList();
                var differentIds = pageIds.Except(selectedPageIds).ToList();
                pages = pages.Where(x => differentIds.Contains(x.PageId)).ToList();
            }

            var list = pages.Select(x=> new SelectListItem
                        {
                            Text = x.PageTitle,
                            Value = x.PageId.ToString()
                        }).OrderByDescending(m => m.Text).ToList();

            return new MultiSelectList(list, "Value", "Text", selectedValues);
        }
        public SelectList PopulatePageSelectList(PageType? pageTypeId, int? selectedValue=null, bool isShowSelectText = false)
        {
            var lst = (from p in DataContext.Get<Page>().Where(p => (pageTypeId == null || p.PageTypeId == pageTypeId) && p.IsActive == PageStatus.Active)
                        select new SelectListItem
                        {
                            Text = p.PageTitle,
                            Value = p.PageId.ToString(),
                            Selected = selectedValue != null && p.PageId.Equals(selectedValue)
                        }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectPage} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        public SelectList PopulateActivePageSelectList(PageType? pageTypeId, string languageCode, int? selectedValue = null, bool isShowSelectText = false)
        {
            var lst = (DataContext.Get<Page>().Where(p => (pageTypeId == null || p.PageTypeId == pageTypeId) 
            && p.IsActive == PageStatus.Active 
            && p.LanguageCode == languageCode).Select(p => new SelectListItem()
            {
                Text = p.PageTitle,
                Value = p.PageId.ToString()
            })).ToList();

            if (lst.Any())
            {
                if (isShowSelectText)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectPage} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        public SelectList PopulatePageSelectListByPageTypeId(PageType? pageTypeId, PageStatus? status, int? selectedValue, bool? isShowSelectText = false)
        {
            var lst = (from p in DataContext.Get<Page>()
                       where ((pageTypeId == null || p.PageTypeId == pageTypeId) && (status == null || p.IsActive == status))
                       select new SelectListItem
                       {
                           Text = p.PageName,
                           Value = p.PageId.ToString(),
                           Selected = selectedValue!=null && p.PageId == selectedValue
                       }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText!=null && isShowSelectText==true)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectPage} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        #endregion===============================================================================================================================================

        public bool HasPageNameExisted(string pageName)
        {
            if (string.IsNullOrEmpty(pageName)) return false;
            var query = DataContext.Get<Page>().FirstOrDefault(c => c.PageName.Equals(pageName, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }
        public bool HasPageTitleExisted(string pageTitle)
        {
            if (string.IsNullOrEmpty(pageTitle)) return false;
            var query = DataContext.Get<Page>().FirstOrDefault(c => c.PageTitle.Equals(pageTitle, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }
        public bool HasPageCodeExisted(string pageCode)
        {
            if (string.IsNullOrEmpty(pageCode)) return false;
            var query = DataContext.Get<Page>().FirstOrDefault(c => c.PageCode.Equals(pageCode, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<Page>() select u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
    }
}
