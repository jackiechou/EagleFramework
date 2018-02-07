using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Extensions.EnumHelper;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
    {
        public ModuleRepository(IDataContext dataContext) : base(dataContext) { }
        //public JsonResult TreeData(FormCollection form)
        //{
        //    return GetTreeData(Request.QueryString["id"], Request.QueryString["uitype"]);
        //}

        //public List<Role> GetModuleRolePermissions()
        //{
        //    lst = (from p in context.Roles
        //           select new ModuleRolePermissionViewModel
        //           {
        //               ApplicationId = p.ApplicationId,
        //               ModuleId = p.ModuleId,
        //               ModuleCode = p.ModuleCode,
        //               ModuleKey = p.ModuleKey,
        //               ModuleTitle = p.ModuleTitle,
        //               ModuleName = p.ModuleName,
        //               AllPages = p.AllPages,
        //               ModuleTypeId = p.ModuleTypeId,
        //               IsDeleted = p.IsDeleted,
        //               InheritViewPermissions = p.InheritViewPermissions,
        //               Header = p.Header,
        //               Footer = p.Footer,
        //               StartDate = p.StartDate,
        //               EndDate = p.EndDate,
        //               Pane = p.Pane,
        //               InsertedPosition = p.InsertedPosition,
        //               Visibility = p.Visibility,
        //               Alignment = p.Alignment,
        //               Border = p.Border,
        //               Color = p.Color,
        //               Icon = p.Icon
        //           }).ToList();
        //}

        #region Module Type List ======================================================================================

        public SelectList PopulateModuleTypeSelectList(int? selectedValue, bool isShowSelectText = false)
        {
            var lst = (from ModuleType x in Enum.GetValues(typeof(ModuleType)).Cast<ModuleType>()
                        select new SelectListItem
                        {
                            Text = x.ToString(),
                            Value = ((int)x).ToString(),
                            Selected = x.Equals(selectedValue)
                        }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectModuleType} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        #endregion ====================================================================================================

        #region Select List ======================================================================================

        public SelectList PopulateAlignmentList(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Left, Value = "Left"},
                new SelectListItem {Text = LanguageResource.Center, Value = "Center"},
                new SelectListItem {Text = LanguageResource.Right, Value = "Right"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectAlignment} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        public SelectList PopulateModuleList(ModuleType? moduleTypeId, ModuleStatus? status, string selectedValue, bool isShowSelectText = false)
        {
            var list = (from m in DataContext.Get<Module>()
                         where (moduleTypeId == null || m.ModuleTypeId == moduleTypeId)
                               && (status == null || m.IsActive == status)
                         select new SelectListItem
                         {
                             Text = m.ModuleTitle,
                             Value = m.ModuleId.ToString()
                         }).OrderByDescending(m => m.Text).ToList();

            if (isShowSelectText)
                list.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectModule} ---", Value = "" });
            return new SelectList(list, "Value", "Text", selectedValue);
        }
        public MultiSelectList PopulateModuleMultiSelectList(ModuleType? moduleTypeId, ModuleStatus? status, string[] selectedValues)
        {
            var list = (from m in DataContext.Get<Module>()
                        where (moduleTypeId == null || m.ModuleTypeId == moduleTypeId)
                              && (status == null || m.IsActive == status)
                        select new SelectListItem
                        {
                            Text = m.ModuleTitle,
                            Value = m.ModuleId.ToString()
                        }).OrderByDescending(m => m.Text).ToList();
            return new MultiSelectList(list, "Value", "Text", selectedValues);
        }
        public SelectList PopulateInsertedPositionList(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Top, Value = "Top"},
                new SelectListItem {Text = LanguageResource.Bottom, Value = "Bottom"},
                new SelectListItem {Text = LanguageResource.Left, Value = "Left"},
                new SelectListItem {Text = LanguageResource.Right, Value = "Right"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectPosition} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateVisibilityList(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.SameAsPage, Value = "True"},
                new SelectListItem {Text = LanguageResource.PageEditorsOnly, Value = "False"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectVisibility} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        public SelectList PopulatePaneList(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.ContentPane, Value = "ContentPane"},
                new SelectListItem {Text = LanguageResource.TopPane, Value = "TopPane"},
                new SelectListItem {Text = LanguageResource.LeftPane, Value = "LeftPane"},
                new SelectListItem {Text = LanguageResource.RightPane, Value = "RightPane"},
                new SelectListItem {Text = LanguageResource.BottomPane, Value = "BottomPane"},
                new SelectListItem {Text = LanguageResource.BannerPane, Value = "BannerPane"},
                new SelectListItem {Text = LanguageResource.LogoPane, Value = "LogoPane"}
            };

            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectPane} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        #endregion ====================================================================================================

        public IEnumerable<Module> Search(string keywords, ModuleType moduleTypeId, ModuleStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = (from x in DataContext.Get<Module>()
                         where x.ModuleTypeId == moduleTypeId && (status==null || x.IsActive == status)
                         select x);
          
            if (!string.IsNullOrEmpty(keywords))
                query = query.Where(x => x.ModuleName.ToLower().Contains(keywords.ToLower()) || x.ModuleTitle.ToLower().Contains(keywords.ToLower()));
            return query.WithRecordCount(out recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<Module> GetList(ModuleType? moduleTypeId, ModuleStatus? status)
        {
            return (from m in DataContext.Get<Module>()
                    join p in DataContext.Get<ModulePermission>() on m.ModuleId equals p.ModuleId into permissionlist
                    from mp in permissionlist.DefaultIfEmpty()
                    where (moduleTypeId == null || m.ModuleTypeId == moduleTypeId) 
                    && (status==null || m.IsActive==status)
                    select new Module
                    {
                        ApplicationId = m.ApplicationId,
                        ModuleId = m.ModuleId,
                        ModuleTypeId = m.ModuleTypeId,
                        ModuleCode = m.ModuleCode,
                        ModuleTitle = m.ModuleTitle,
                        ModuleName = m.ModuleName,
                        AllPages = m.AllPages,
                        IsActive = m.IsActive,
                        Header = m.Header,
                        Footer = m.Footer,
                        StartDate = m.StartDate,
                        EndDate = m.EndDate
                    }).OrderByDescending(m => m.ModuleId).AsEnumerable();
        }
        public bool HasIdExisted(int moduleId)
        {
            var query = DataContext.Get<Module>().FirstOrDefault(p => p.ModuleId.Equals(moduleId));
            return (query != null);
        }
        public bool HasModuleNameExisted(string moduleName)
        {
            var query =
                DataContext.Get<Module>()
                    .FirstOrDefault(p => p.ModuleName.ToLower().Equals(moduleName, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }
        public bool HasModuleTitleExisted(string moduleTitle)
        {
            var query =
                DataContext.Get<Module>()
                    .FirstOrDefault(p => p.ModuleTitle.ToLower().Equals(moduleTitle, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }
        public bool HasModuleCodeExisted(string moduleCode)
        {
            var query =
                DataContext.Get<Module>()
                    .FirstOrDefault(p => p.ModuleCode.ToLower().Equals(moduleCode, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }
        public int GetListOrder()
        {
            return DataContext.Get<Module>().DefaultIfEmpty().Max(x => x == null ? 0 : x.ViewOrder + 1) ?? 0;
        }
    }
}
