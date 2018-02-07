using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement
{
    public class PageService : BaseService, IPageService
    {
        private IModuleService ModuleService { get; set; }
        private IPermissionService PermissionService { get; set; }
        public PageService(IUnitOfWork unitOfWork, IModuleService moduleService, IPermissionService permissionService) : base(unitOfWork)
        {
            PermissionService = permissionService;
            ModuleService = moduleService;
        }

        #region PAGE  ======================================================================================
        public MultiSelectList PopulatePageMultiSelectList(PageType pageTypeId, PageStatus? status = null, List<string> selectedValues = null)
        {
            return UnitOfWork.PageRepository.PopulatePageMultiSelectList(pageTypeId, status, selectedValues);
        }
        public MultiSelectList PopulatePageMultiSelectList(PageType pageTypeId, PageStatus? status=null, int? moduleId = null, List<string> selectedValues =null)
        {
            return UnitOfWork.PageRepository.PopulatePageMultiSelectList(pageTypeId, status, moduleId, selectedValues);
        }
        public SelectList PopulatePageSelectList(PageType? pageTypeId, PageStatus? status = null, int? selectedValue=null,
        bool? isShowSelectText = false)
        {
            return UnitOfWork.PageRepository.PopulatePageSelectListByPageTypeId(pageTypeId, status, selectedValue, isShowSelectText);
        }

        public IEnumerable<PageDetail> GetListByPageTypeId(PageType pageTypeId, PageStatus? status)
        {
            var lst = UnitOfWork.PageRepository.GetListByPageTypeId(pageTypeId, status);
            return lst.ToDtos<Page, PageDetail>();
        }

        public IEnumerable<PageDetail> GetListByPageTypeIdAndKeywords(string keywords, PageType? pageTypeId,
            PageStatus? status)
        {
            var lst = UnitOfWork.PageRepository.GetListByPageTypeIdAndKeywords(keywords, pageTypeId, status);
            return lst.ToDtos<Page, PageDetail>();
        }
        public IEnumerable<PageDetail> Search(string keywords, PageType? pageTypeId, PageStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.PageRepository.Search(keywords, pageTypeId, status, out recordCount, orderBy, page,
                pageSize);
            return lst.ToDtos<Page, PageDetail>();
        }

        /// <summary>
        /// Auto Complete with select2
        /// </summary>
        /// <param name="recordCount"></param>
        /// <param name="searchTerm"></param>
        /// <param name="pageTypeId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public Select2PagedResult GetAutoCompletePages(out int recordCount, string searchTerm, PageType? pageTypeId, int? page)
        {
            var lst = UnitOfWork.PageRepository.GetAutoCompleteList(out recordCount, searchTerm, pageTypeId, null, page, GlobalSettings.DefaultPageSize);
            var results = lst.Select(item => new Select2Result { id = item.Id.ToString(), name = item.Name, text = item.Text, level = item.Level }).ToList();
            if (!results.Any()) return new Select2PagedResult { Results = null, Total = 0, MorePage = false };

            return new Select2PagedResult
            {
                Results = results,
                Total = recordCount,
                PageSize = GlobalSettings.DefaultPageSize,
                MorePage = page * GlobalSettings.DefaultPageSize < recordCount
            };
        }

        #region Page Tree List ==========================================================================================================
        public string LoadPageList(PageType pageTypeId)
        {
            string strResult = string.Empty;
            var pageList = UnitOfWork.PageRepository.GetListByPageTypeId(pageTypeId, null);
            if (pageList != null)
            {
                foreach (var item in pageList)
                {
                    strResult += "<li id='" + item.PageId + "'>";
                    strResult += "<span class=" + item.IconClass + ">" + item.PageTitle + "</span>";
                    strResult += "</li>";
                }
            }
            var strHtml = "<ul id='page_nav'>" + strResult + "</ul>";
            return strHtml;
        }
        public IEnumerable<PageTree> GetPageTree(PageType pageTypeId)
        {
            var lst = UnitOfWork.PageRepository.GetListByPageTypeId(pageTypeId, null).ToList();
            List<PageTree> list = lst.Select(m => new PageTree()
            {
                Id = m.PageId,
                Key = m.PageId,
                Name = m.PageName,
                Text = m.PageTitle,
                Title = m.PageTitle,
                Tooltip = m.Description,
                Open = true
            }).ToList();
            var recursiveObjects = RecursiveFillTree(list, null);
            return recursiveObjects;
        }
        private List<PageTree> RecursiveFillTree(List<PageTree> list, int? id)
        {
            var items = new List<PageTree>();
            var nodes = list.Where(m => m.ParentId == id).Select(
               m => new PageTree
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

            if (!nodes.Any()) return items;

            items.AddRange(nodes.Select(child => new PageTree()
            {
                Id = child.Id,
                Key = child.Key,
                ParentId = child.ParentId,
                Name = child.Name,
                Text = child.Text,
                Title = child.Title,
                Tooltip = child.Tooltip,
                IsParent = child.IsParent,
                Open = child.Open,
                Children = RecursiveFillTree(list, child.Id)
            }));
            return items;
        }
        #endregion =======================================================================================================================

        public PageDetail GetDetails(int id)
        {
            var entity = UnitOfWork.PageRepository.FindById(id);
            return entity.ToDto<Page, PageDetail>();
        }

        public AutoCompleteDetail GetAutoCompleteDetails(int id)
        {
            var entity = UnitOfWork.PageRepository.FindById(id);
            return new AutoCompleteDetail
            {
                Id = entity.PageId,
                Name = entity.PageName,
                Text = entity.PageTitle
            };
        }

        public void Insert(Guid applicationId, Guid userId, Guid roleId, string languageCode, PageEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullPageEntry, "PageEntry"));
                throw new ValidationError(violations);
            }
            if (string.IsNullOrEmpty(entry.PageName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPageName, "PageName", null, ErrorMessage.Messages[ErrorCode.NullPageName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PageName.Length > 200)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPageName, "PageName", entry.PageName, ErrorMessage.Messages[ErrorCode.InvalidPageName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool isDuplicate = UnitOfWork.PageRepository.HasPageNameExisted(entry.PageName);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicatePageName, "PageName",
                            entry.PageName, ErrorMessage.Messages[ErrorCode.DuplicatePageName]));
                        throw new ValidationError(violations);
                    }
                }
            }

            if (string.IsNullOrEmpty(entry.PageTitle))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPageTitle, "PageTitle", null, ErrorMessage.Messages[ErrorCode.NullPageTitle]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PageTitle.Length > 200)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPageTitle, "PageTitle", entry.PageTitle, ErrorMessage.Messages[ErrorCode.InvalidPageTitle]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entry.PageTitle.Length > 200)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidPageTitle, "PageTitle", entry.PageTitle,
                            ErrorMessage.Messages[ErrorCode.InvalidPageTitle]));
                        throw new ValidationError(violations);
                    }
                    else
                    {
                        bool isDuplicate = UnitOfWork.PageRepository.HasPageTitleExisted(entry.PageTitle);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicatePageTitle, "PageTitle",
                                entry.PageName, ErrorMessage.Messages[ErrorCode.DuplicatePageTitle]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(entry.PageCode))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPageCode, "PageCode", null, ErrorMessage.Messages[ErrorCode.NullPageCode]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PageCode.Length > 200)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPageCode, "PageCode", entry.PageCode, ErrorMessage.Messages[ErrorCode.InvalidPageCode]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool isDuplicate = UnitOfWork.PageRepository.HasPageCodeExisted(entry.PageCode);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicatePageCode, "PageCode",
                            entry.PageName, ErrorMessage.Messages[ErrorCode.DuplicatePageCode]));
                        throw new ValidationError(violations);
                    }
                }
            }
            
            var entity = entry.ToEntity<PageEntry, Page>();
            int listOrder = UnitOfWork.PageRepository.GetNewListOrder();

            entity.IsExtenalLink = entry.IsExtenalLink ?? false;
            entity.IsMenu = entry.IsMenu;
            entity.PageAlias = StringUtils.ConvertTitle2Alias(entry.PageName);
            entity.ApplicationId = applicationId;
            entity.LanguageCode = languageCode;
            entity.StartDate = entry.StartDate?? DateTime.UtcNow;
            entity.ListOrder = listOrder;
            entity.CreatedDate= DateTime.UtcNow;
            entity.CreatedByUserId = userId;
            entity.Ip = ip;

            UnitOfWork.PageRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            //Insert Page Permission for Administrator
            var pagePermission = new PagePermission
            {
                PageId = entity.PageId,
                RoleId = roleId,
                AllowAccess = true
            };

            UnitOfWork.PagePermissionRepository.Insert(pagePermission);
            UnitOfWork.SaveChanges();
        }
        public void Update(Guid userId, PageEditEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullPageEntry, "PageEntry"));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.PageRepository.FindById(entry.PageId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPage, "Page", entry.PageId));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.PageName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPageName, "PageName", null,
                    ErrorMessage.Messages[ErrorCode.NullPageName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PageName.Length > 200)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPageName, "PageName", entry.PageName,
                        ErrorMessage.Messages[ErrorCode.InvalidPageName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.PageName.ToLower() != entry.PageName.ToLower())
                    {
                        bool isDuplicate = UnitOfWork.PageRepository.HasPageNameExisted(entry.PageName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicatePageName, "PageName",
                                entry.PageName, ErrorMessage.Messages[ErrorCode.DuplicatePageName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(entry.PageTitle))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPageTitle, "PageTitle", null,
                    ErrorMessage.Messages[ErrorCode.NullPageTitle]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PageTitle.Length > 200)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPageTitle, "PageTitle", entry.PageName,
                        ErrorMessage.Messages[ErrorCode.InvalidPageTitle]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.PageTitle != entry.PageTitle)
                    {
                        bool isDuplicate = UnitOfWork.PageRepository.HasPageTitleExisted(entry.PageTitle);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicatePageTitle, "PageTitle",
                                entry.PageName, ErrorMessage.Messages[ErrorCode.DuplicatePageTitle]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(entry.PageCode))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPageCode, "PageCode", null,
                    ErrorMessage.Messages[ErrorCode.NullPageCode]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PageCode.Length > 200)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPageCode, "PageCode", entry.PageCode,
                        ErrorMessage.Messages[ErrorCode.InvalidPageCode]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.PageCode.ToLower() != entry.PageCode.ToLower())
                    {
                        bool isDuplicate = UnitOfWork.PageRepository.HasPageNameExisted(entry.PageCode);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicatePageCode, "PageCode",
                                entry.PageName, ErrorMessage.Messages[ErrorCode.DuplicatePageCode]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            entity.TemplateId = entry.TemplateId;
            entity.PageName = entry.PageName;
            entity.PageTitle = entry.PageTitle;
            entity.PageAlias = StringUtils.ConvertTitle2Alias(entry.PageName);
            entity.PageCode = entry.PageCode;
            entity.PagePath = entry.PagePath;
            entity.PageUrl = entry.PageUrl;
            entity.IconClass = entry.IconClass;
            entity.Description = entry.Description;
            entity.Keywords = entry.Keywords;
            entity.PageHeadText = entry.PageHeadText;
            entity.PageFooterText = entry.PageFooterText;
            entity.StartDate = entry.StartDate;
            entity.EndDate = entry.EndDate;
           
            entity.DisplayTitle = entry.DisplayTitle;
            entity.IsExtenalLink = entry.IsExtenalLink;
            entity.IsMenu = entry.IsMenu;
            entity.IsSecured = entry.IsSecured ?? false;
            entity.DisableLink = entry.DisableLink ?? false;
            entity.LastUpdatedIp = ip;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.PageRepository.Update(entity);
            UnitOfWork.SaveChanges();

            //Update Modules on Page
            if (entry.SelectedModules != null && entry.SelectedModules.Any())
            {
                var modulesOnPage = entry.SelectedModules.Select(moduleId => new PageModuleEntry
                {
                    PageId = entity.PageId,
                    ModuleId = moduleId,
                    IsVisible = true
                }).ToList();

                UpdatePageModules(modulesOnPage);
            }
            else
            {
                DeletePageModuleByPageId(entity.PageId);
            }

            //Update Page Permission
            if (entry.PagePermissions != null)
            {
                PermissionService.UpdatePermission(entry.PageId, entry.PagePermissions);
            }
        }
        public void UpdateStatus(Guid userId, int pageId, PageStatus status)
        {
            string ip = NetworkUtils.GetIP4Address();

            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.PageRepository.FindById(pageId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPage, "PageId", pageId));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(PageStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }

            entity.IsActive = status;
            entity.LastModifiedByUserId = userId;
            entity.LastUpdatedIp = ip;

            UnitOfWork.PageRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateListOrder(Guid userId, int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.PageRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPage, "Page", id));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            entity.LastModifiedByUserId = userId;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.PageRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void Delete(int pageId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.PageRepository.FindById(pageId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPage, "Page", pageId));
                throw new ValidationError(violations);
            }

            var pageModules = UnitOfWork.PageModuleRepository.GetListByPageId(null, pageId);
            if (pageModules != null)
            {
                foreach (var pageModule in pageModules)
                {
                    var item = new PageModule
                    {
                        PageId = pageId,
                        ModuleId = pageModule.ModuleId
                    };
                    UnitOfWork.PageModuleRepository.Delete(item);
                }
            }

            var pagePermissions = UnitOfWork.PagePermissionRepository.GetPagePermissionsByPageId(pageId);
            if (pagePermissions != null)
            {
                foreach (var pagePermission in pagePermissions)
                {
                    UnitOfWork.PagePermissionRepository.Delete(pagePermission);
                }
            }

            UnitOfWork.PageRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion ====================================================================================================

        #region PAGE TYPE ======================================================================================
        public SelectList PopulatePageTypeSelectList(int? selectedValue, bool isShowSelectText = false)
        {
            return UnitOfWork.PageRepository.PopulatePageTypeSelectList(selectedValue, isShowSelectText);
        }
        #endregion ====================================================================================================

        #region PAGE MODULE  ======================================================================================
        public IEnumerable<PageModuleDetail> GetPagePagesByModuleId(int moduleId, bool? isVisible)
        {
            var lst = UnitOfWork.PageModuleRepository.GetPageModulesByModuleId(moduleId, isVisible);
            return lst.ToDtos<PageModule, PageModuleDetail>();
        }
        public IEnumerable<PageModuleDetail> GetPageModulesByPage(int pageId, bool? isVisible)
        {
            var lst = UnitOfWork.PageModuleRepository.GetPageModulesByPageId(pageId, isVisible);
            return lst.ToDtos<PageModule, PageModuleDetail>();
        }
        public MultiSelectList PopulatePageByModuleIdMultiSelectList(int moduleId, bool? isVisible=null, string[] selectedValues=null)
        {
            if(moduleId < 0) return new MultiSelectList(null, "Value", "Text", selectedValues);
            return UnitOfWork.PageModuleRepository.PopulatePageByModuleIdMultiSelectList(moduleId, isVisible, selectedValues);
        }
        public MultiSelectList PopulatePagesByModuleIdMultiSelectList(int moduleId, PageType? pageTypeId = null, bool? isVisible = null, string[] selectedValues = null)
        {
            if (moduleId < 0) return new MultiSelectList(null, "Value", "Text", selectedValues);
            return UnitOfWork.PageModuleRepository.PopulatePagesByModuleIdMultiSelectList(moduleId, pageTypeId, isVisible, selectedValues);
        }

        public MultiSelectList PopulateModulesByPageMultiSelectList(int? pageId, ModuleType? moduleTypeId = null, string[] selectedValues = null)
        {
            return UnitOfWork.PageModuleRepository.PopulateModulesByPageMultiSelectList(pageId, moduleTypeId, selectedValues);
        }
        public SelectList PopulateModulesByPage(int? pageId, ModuleType? moduleTypeId = null, string selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.PageModuleRepository.PopulateModulesByPage(pageId, moduleTypeId, selectedValue, isShowSelectText);
        }
        public void InsertPageModule(PageModuleEntry entry)
        {
            bool isDuplicate = UnitOfWork.PageModuleRepository.HasDataExisted(entry.PageId, entry.ModuleId);
            if (isDuplicate) return;

            var moduleOrder = UnitOfWork.PageModuleRepository.GetNewModuleOrder();
            var entity = entry.ToEntity<PageModuleEntry, PageModule>();
            entity.ModuleOrder = moduleOrder;

            UnitOfWork.PageModuleRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdatePageModule(int id, PageModuleEntry entry)
        {
            var entity = UnitOfWork.PageModuleRepository.FindById(id);
            if (entity != null)
            {
                entity.PageId = entry.PageId;
                entity.ModuleId = entry.ModuleId;
                entity.Pane = entry.Pane;
                entity.Alignment = entry.Alignment;
                entity.Color = entry.Color;
                entity.Border = entry.Border;
                entity.InsertedPosition = entry.InsertedPosition;
                entity.IconFile = entry.IconFile;
                entity.IconClass = entry.IconClass;
                entity.IsVisible = entry.IsVisible;

                UnitOfWork.PageModuleRepository.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdatePageModules(List<PageModuleEntry> modulesOnPage)
        {
            if (modulesOnPage == null || modulesOnPage.Count == 0) return;
            int pageId = modulesOnPage.FirstOrDefault().PageId;
            var pageModules = UnitOfWork.PageModuleRepository.GetPageModulesByPageId(pageId, null).ToList();

            if (pageModules.Any())
            {
                var previousPageModules = pageModules.Select(x => x.ModuleId).ToList();
                var latestPageModules = modulesOnPage.Select(x => x.ModuleId).ToList();

                //get intersection between 2 lists
                var intersectList = latestPageModules.Intersect(previousPageModules).ToList();
                if (intersectList.Count > 0)
                {
                    foreach (var moduleId in intersectList)
                    {
                        var entity = UnitOfWork.PageModuleRepository.GetDetailsByPageIdModuleId(pageId, moduleId);
                        if (entity != null)
                        {
                            var entry = modulesOnPage.FirstOrDefault(x => x.ModuleId == moduleId);
                            if (entity.IsVisible != entry.IsVisible)
                            {
                                entity.PageId = entry.PageId;
                                entity.ModuleId = entry.ModuleId;
                                entity.IsVisible = entry.IsVisible;

                                UnitOfWork.PageModuleRepository.Update(entity);
                                UnitOfWork.SaveChanges();
                            }
                        }
                    }
                }
               

                //Get the elements in latest list a but not in previous list b - Except
                var exceptLatestList = latestPageModules.Except(previousPageModules).ToList();
                if (exceptLatestList.Count > 0)
                {
                    foreach (var moduleId in exceptLatestList)
                    {
                        var entry = modulesOnPage.FirstOrDefault(x => x.ModuleId == moduleId);
                        InsertPageModule(entry);
                    }
                }

                //Get the elements in previous list b but not in latest list a - Except
                var exceptPreviousList = previousPageModules.Except(latestPageModules).ToList();
                if (exceptPreviousList.Count > 0)
                {
                    foreach (var moduleId in exceptPreviousList)
                    {
                        var pageModule = UnitOfWork.PageModuleRepository.GetDetailsByPageIdModuleId(pageId, moduleId);
                        if (pageModule == null) return;

                        UnitOfWork.PageModuleRepository.Delete(pageModule);
                        UnitOfWork.SaveChanges();
                    }
                }
               
            }
            else
            {
                foreach (var module in modulesOnPage)
                {
                    InsertPageModule(module);
                }
            }
        }
        public void UpdatePageModuleVisible(int pageId, bool isVisible)
        {
            var lst = UnitOfWork.PageModuleRepository.GetPageModulesByPageId(pageId, isVisible);
            foreach (var item in lst)
            {
                item.IsVisible = isVisible;
                UnitOfWork.PageModuleRepository.Update(item);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdateModuleOrder(int pageId, int moduleId, int moduleOrder)
        {
            var entity = UnitOfWork.PageModuleRepository.GetDetailsByPageIdModuleId(pageId, moduleId);
            if (entity == null) return;

            entity.ModuleOrder = moduleOrder;
            UnitOfWork.PageModuleRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeletePageModule(int pageModuleId)
        {
            var pageModule = UnitOfWork.PageModuleRepository.FindById(pageModuleId);
            if (pageModule == null) return;

            UnitOfWork.PageModuleRepository.Delete(pageModule);
            UnitOfWork.SaveChanges();
        }
        public void DeletePageModuleByPageId(int pageId)
        {
            var pageModules = UnitOfWork.PageModuleRepository.GetListByPageId(null, pageId, null);
            if (pageModules == null) return;

            foreach (var item in pageModules)
            {
                var pageModule = new PageModule
                {
                    PageId = pageId,
                    ModuleId = item.PageId
                };
                UnitOfWork.PageModuleRepository.Delete(pageModule);
            }
            UnitOfWork.SaveChanges();
        }
        #endregion ====================================================================================================

        #region Dipose

        private bool _disposed = false;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    ModuleService = null;
                    PermissionService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
