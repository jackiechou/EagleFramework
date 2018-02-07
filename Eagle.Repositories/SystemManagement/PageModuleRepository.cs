using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class PageModuleRepository : RepositoryBase<PageModule>, IPageModuleRepository
    {
        public PageModuleRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<PageModuleInfo> GetListByPageId(string keywords, int? pageId, ModuleType? moduleTypeId=ModuleType.Admin)
        {
            return (from m in DataContext.Get<Module>()
                    join pm in DataContext.Get<PageModule>() on m.ModuleId equals pm.ModuleId into moduleLst
                    from ml in moduleLst.DefaultIfEmpty()
                    where (pageId==null || ml.PageId == pageId) 
                    && (moduleTypeId == null || m.ModuleTypeId == moduleTypeId)
                    && (m.ModuleName == keywords || keywords == null) 
                    && (m.ModuleTitle == keywords || keywords == null)
                    orderby ml.ModuleOrder
                    select new PageModuleInfo
                    {
                        ApplicationId = m.ApplicationId,
                        PageModuleId = ml.PageModuleId,
                        ModuleTypeId = m.ModuleTypeId,
                        PageId = ml.PageId,
                        ModuleId = m.ModuleId,
                        ModuleCode = m.ModuleCode,
                        ModuleTitle = m.ModuleTitle,
                        ModuleName = m.ModuleName,
                        Header = m.Header,
                        Footer = m.Footer,
                        StartDate = m.StartDate,
                        EndDate = m.EndDate,
                        ModuleOrder = ml.ModuleOrder,
                        Alignment = ml.Alignment,
                        Color = ml.Color,
                        Border = ml.Border,
                        AllPages = m.AllPages,
                        IsVisible = ml.IsVisible
                    }).OrderByDescending(pm => pm.ModuleOrder).ToList();
        }
        public IEnumerable<Module> GetModules(int pageId, bool? isVisible)
        {
            return (from pm in DataContext.Get<PageModule>()
                    join m in DataContext.Get<Module>() on pm.ModuleId equals m.ModuleId into pmJoin
                    from module in pmJoin.DefaultIfEmpty()
                    where pm.PageId == pageId
                    where isVisible == null || pm.IsVisible == isVisible
                    orderby pm.ModuleOrder
                    select module).ToList();
        }
        public IEnumerable<PageModule> GetPageModulesByPageId(int pageId, bool? isVisible)
        {
            return (from pm in DataContext.Get<PageModule>()
                    join m in DataContext.Get<Module>() on pm.ModuleId equals m.ModuleId into moduleLst
                    from ml in moduleLst.DefaultIfEmpty()
                    where pm.PageId == pageId
                    where isVisible == null || pm.IsVisible == isVisible
                    select pm).AsEnumerable();
        }
        public IEnumerable<PageModule> GetPageModulesByModuleId(int moduleId, bool? isVisible)
        {
            return (from pm in DataContext.Get<PageModule>()
                    join m in DataContext.Get<Module>() on pm.ModuleId equals m.ModuleId into moduleLst
                    from ml in moduleLst.DefaultIfEmpty()
                    where pm.ModuleId == moduleId && (isVisible == null || pm.IsVisible == isVisible)
                    select pm).AsEnumerable();
        }
        public PageModule GetDetailsByPageIdModuleId(int pageId, int moduleId)
        {
            return (from pm in DataContext.Get<PageModule>()
                    where pm.PageId == pageId && pm.ModuleId == moduleId
                    select pm).FirstOrDefault();
        }
        public MultiSelectList PopulatePageByModuleIdMultiSelectList(int moduleId, bool? isVisible, string[] selectedValues)
        {
            var list = (from pm in DataContext.Get<PageModule>()
                        join p in DataContext.Get<Page>() on pm.PageId equals p.PageId into pageLst
                        from pl in pageLst.DefaultIfEmpty()
                        where pm.ModuleId == moduleId && (isVisible == null || pm.IsVisible == isVisible)
                        select new SelectListItem
                        {
                            Text = pl.PageTitle,
                            Value = pl.PageId.ToString()
                        }).OrderByDescending(m => m.Text).ToList();
            return new MultiSelectList(list, "Value", "Text", selectedValues);
        }

        public MultiSelectList PopulatePagesByModuleIdMultiSelectList(int moduleId, PageType? pageTypeId, bool? isVisible, string[] selectedValues)
        {
            var list = (from pm in DataContext.Get<PageModule>()
                        join p in DataContext.Get<Page>() on pm.PageId equals p.PageId into pageLst
                        from pl in pageLst.DefaultIfEmpty()
                        where pm.ModuleId == moduleId  && (pageTypeId == null || pl.PageTypeId == pageTypeId) && (isVisible == null || pm.IsVisible == isVisible)
                        select new SelectListItem
                        {
                            Text = pl.PageTitle,
                            Value = pl.PageId.ToString()
                        }).OrderByDescending(m => m.Text).ToList();
            return new MultiSelectList(list, "Value", "Text", selectedValues);
        }

        public MultiSelectList PopulateModulesByPageMultiSelectList(int? pageId, ModuleType? moduleTypeId, string[] selectedValues)
        {
            var lst = (from m in DataContext.Get<Module>()
                       join pm in DataContext.Get<PageModule>() on m.ModuleId equals pm.ModuleId into moduleLst
                       from ml in moduleLst.DefaultIfEmpty()
                       where (pageId == null || ml.PageId == pageId)
                       && (moduleTypeId == null || m.ModuleTypeId == moduleTypeId)
                       select new SelectListItem
                       {
                           Text = m.ModuleTitle,
                           Value = m.ModuleId.ToString()
                       }).OrderBy(m => m.Text).ToList(); 
           
            return new MultiSelectList(lst, "Value", "Text", selectedValues);
        }

        public SelectList PopulateModulesByPage(int? pageId, ModuleType? moduleTypeId, string selectedValue, bool? isShowSelectText = false)
        {
            var list = (from m in DataContext.Get<Module>()
                       join pm in DataContext.Get<PageModule>() on m.ModuleId equals pm.ModuleId into moduleLst
                       from ml in moduleLst.DefaultIfEmpty()
                       where (pageId == null || ml.PageId == pageId)
                       && (moduleTypeId == null || m.ModuleTypeId == moduleTypeId)
                       select new SelectListItem
                       {
                           Text = m.ModuleTitle,
                           Value = m.ModuleId.ToString()
                       }).OrderBy(m => m.Text).ToList();


            if (isShowSelectText!=null && isShowSelectText==true)
                list.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectModule} ---", Value = "" });
            return new SelectList(list, "Value", "Text", selectedValue);

        }
        public PageModuleInfo Details(int id)
        {
            return (from pm in DataContext.Get<PageModule>()
                    join m in DataContext.Get<Module>() on pm.ModuleId equals m.ModuleId into moduleLst
                    from ml in moduleLst.DefaultIfEmpty()
                    where ml.ModuleId == id
                    select new PageModuleInfo
                    {
                        PageModuleId = pm.PageModuleId,
                        PageId = pm.PageId,
                        ModuleId = ml.ModuleId,
                        Pane = pm.Pane,
                        Alignment = pm.Alignment,
                        Color = pm.Color,
                        Border = pm.Border,
                        InsertedPosition = pm.InsertedPosition,
                        IconFile = pm.IconFile,
                        IconClass = pm.IconClass,
                        ModuleOrder = pm.ModuleOrder,
                        IsVisible = pm.IsVisible
                    }).FirstOrDefault();
        }
        public int GetNewModuleOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<PageModule>() select (int)u.ModuleOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
        public bool HasDataExisted(int pageId, int moduleId)
        {
            var query = DataContext.Get<PageModule>().FirstOrDefault(p => p.PageId == pageId && p.ModuleId == moduleId);
            return (query != null);
        }
    }
}
