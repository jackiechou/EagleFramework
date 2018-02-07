using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.SystemManagement.Validation;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement
{
    public class ModuleService : BaseService, IModuleService
    {
        public IRoleService RoleService { get; set; }

        public ModuleService(IRoleService roleService, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            RoleService = roleService;
        }

        #region MODULE
        public SelectList PopulateModuleTypeSelectList(int? selectedValue, bool isShowSelectText = false)
        {
            return UnitOfWork.ModuleRepository.PopulateModuleTypeSelectList(selectedValue, isShowSelectText);
        }

        public IEnumerable<ModuleDetail> Search(ModuleSearchEntry filter, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ModuleRepository.Search(filter.SearchText, filter.SearchModuleType, filter.Status, out recordCount, orderBy, page, pageSize);
            return lst.ToDtos<Module, ModuleDetail>();
        }
        public IEnumerable<ModuleDetail> GetModules(ModuleType? moduleTypeId, ModuleStatus? status)
        {
            var lst = UnitOfWork.ModuleRepository.GetList(moduleTypeId, status);
            return lst.ToDtos<Module, ModuleDetail>();
        }
        public MultiSelectList PopulateModuleMultiSelectList(ModuleType? moduleTypeId, ModuleStatus? status, string[] selectedValues)
        {
            return UnitOfWork.ModuleRepository.PopulateModuleMultiSelectList(moduleTypeId, status, selectedValues);
        }
        public SelectList PopulateModuleList(ModuleType? moduleTypeId, ModuleStatus? status, string selectedValue, bool isShowSelectText = false)
        {
            return UnitOfWork.ModuleRepository.PopulateModuleList(moduleTypeId, status, selectedValue, isShowSelectText);
        }
        public SelectList PopulateAlignmentList(string selectedValue, bool isShowSelectText = false)
        {
            return UnitOfWork.ModuleRepository.PopulateAlignmentList(selectedValue, isShowSelectText);
        }
        public SelectList PopulateInsertedPositionList(string selectedValue, bool isShowSelectText = false)
        {
            return UnitOfWork.ModuleRepository.PopulateInsertedPositionList(selectedValue, isShowSelectText);
        }
        public SelectList PopulateVisibilityList(string selectedValue, bool isShowSelectText = false)
        {
            return UnitOfWork.ModuleRepository.PopulateVisibilityList(selectedValue, isShowSelectText);
        }
        public SelectList PopulatePaneList(string selectedValue, bool isShowSelectText = false)
        {
            return UnitOfWork.ModuleRepository.PopulatePaneList(selectedValue, isShowSelectText);
        }

        public ModuleDetail GetModuleDetails(int id)
        {
            var entity = UnitOfWork.ModuleRepository.FindById(id);
            return entity.ToDto<Module, ModuleDetail>();
        }
        public void InsertModule(Guid applicationId, Guid roleId, ModuleEntry entry)
        {
            using (var tranScope = new TransactionScope())
            {
                ISpecification<ModuleEntry> validator = new ModuleEntryValidator(UnitOfWork, PermissionLevel.Create);
                var dataViolations = new List<RuleViolation>();
                var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
                if (!isDataValid) throw new ValidationError(dataViolations);

                var moduleEntity = new Module
                {
                    ModuleTypeId = (ModuleType)entry.ModuleTypeId,
                    ModuleCode= entry.ModuleCode,
                    ModuleName = entry.ModuleName,
                    ModuleTitle = entry.ModuleTitle,
                    Description = entry.Description,
                    Header = entry.Header,
                    Footer = entry.Footer,
                    StartDate = entry.StartDate ?? DateTime.UtcNow,
                    EndDate = entry.EndDate,
                    InheritViewPermissions = entry.InheritViewPermissions,
                    AllPages = entry.AllPages ?? false,
                    IsSecured = entry.IsSecured,
                    ApplicationId = applicationId,
                    ViewOrder = UnitOfWork.ModuleRepository.GetListOrder(),
                    IsActive = ModuleStatus.Active,
                };

                UnitOfWork.ModuleRepository.Insert(moduleEntity);
                UnitOfWork.SaveChanges();

                //Insert Page Module
                if (entry.SelectedPages != null)
                {
                    CreatePageModules(moduleEntity.ModuleId, entry.SelectedPages);
                }

                if (entry.ModuleCapabilities != null)
                {
                    CreateModuleCapabilities(roleId, moduleEntity.ModuleId, entry.ModuleCapabilities);
                }

                tranScope.Complete();
            }
        }
        public void UpdateModule(Guid roleId, ModuleEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullModuleEditEntry, "ModuleEditEntry", null, ErrorMessage.Messages[ErrorCode.NullModuleEditEntry]));
                throw new ValidationError(violations);
            }

            var module = UnitOfWork.ModuleRepository.FindById(entry.ModuleId);
            if (module == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundModule, "ModuleId", entry.ModuleId, ErrorMessage.Messages[ErrorCode.NotFoundModule]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.ModuleName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullModuleName, "ModuleName", entry.ModuleName, ErrorMessage.Messages[ErrorCode.NullModuleName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.ModuleName.Length > 256)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidModuleName, "ModuleName", entry.ModuleName,
                        ErrorMessage.Messages[ErrorCode.InvalidModuleName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entry.ModuleName.Length > 250)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidModuleName, "ModuleName", entry.ModuleName,
                            ErrorMessage.Messages[ErrorCode.InvalidModuleName]));
                        throw new ValidationError(violations);
                    }
                    else
                    {
                        if (module.ModuleName != entry.ModuleName)
                        {
                            bool flag = UnitOfWork.ModuleRepository.HasModuleNameExisted(entry.ModuleName);
                            if (flag)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateModuleName, "ModuleName",
                                    entry.ModuleName, ErrorMessage.Messages[ErrorCode.DuplicateModuleName]));
                                throw new ValidationError(violations);
                            }
                        }
                    }
                }
            }

            if (entry.ModuleTitle.Length > 256)
            {
                violations.Add(new RuleViolation(ErrorCode.NullModuleTitle, "ModuleTitle", entry.ModuleTitle,
                    ErrorMessage.Messages[ErrorCode.NullModuleTitle]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.ModuleTitle.Length > 256)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidModuleTitle, "ModuleTitle", entry.ModuleTitle,
                        ErrorMessage.Messages[ErrorCode.InvalidModuleTitle]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (module.ModuleTitle != entry.ModuleTitle)
                    {
                        bool flag = UnitOfWork.ModuleRepository.HasModuleTitleExisted(entry.ModuleTitle);
                        if (flag)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateModuleTitle, "ModuleTitle",
                                entry.ModuleTitle, ErrorMessage.Messages[ErrorCode.DuplicateModuleTitle]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            if (entry.ModuleCode.Length > 250)
            {
                violations.Add(new RuleViolation(ErrorCode.NullModuleCode, "ModuleCode", entry.ModuleCode,
                    ErrorMessage.Messages[ErrorCode.NullModuleCode]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.ModuleCode.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidModuleCode, "ModuleCode", entry.ModuleCode,
                        ErrorMessage.Messages[ErrorCode.InvalidModuleCode]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (module.ModuleCode != entry.ModuleCode)
                    {
                        bool flag = UnitOfWork.ModuleRepository.HasModuleCodeExisted(entry.ModuleCode);
                        if (flag)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateModuleCode, "ModuleCode",
                                entry.ModuleCode, ErrorMessage.Messages[ErrorCode.DuplicateModuleCode]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            DateTime? startDate = entry.StartDate;
            DateTime? endDate = entry.EndDate;
            if (startDate.HasValue && endDate.HasValue)
            {
                if (DateTime.Compare(startDate.Value, endDate.Value) > 0)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidEndDate, "EndDate", entry.EndDate, LanguageResource.ValidateStartDateEndDate));
                    throw new ValidationError(violations);
                }
            }

            module.ModuleTypeId = (ModuleType)entry.ModuleTypeId;
            module.ModuleName = entry.ModuleName;
            module.ModuleTitle = entry.ModuleTitle;
            module.ModuleCode = entry.ModuleCode;
            module.Description = entry.Description;
            module.Header = entry.Header;
            module.Footer = entry.Footer;
            module.StartDate = entry.StartDate ?? DateTime.UtcNow;
            module.EndDate = entry.EndDate;
            module.InheritViewPermissions = entry.InheritViewPermissions;
            module.AllPages = entry.AllPages ?? false;
            module.IsSecured = entry.IsSecured ?? false;

            UnitOfWork.ModuleRepository.Update(module);
            UnitOfWork.SaveChanges();

            //Update page module
            if (entry.SelectedPages != null)
            {
                var pageIds = UnitOfWork.PageModuleRepository.GetPageModulesByModuleId(module.ModuleId, null).Select(x => x.PageId).ToList();
                var selectedPageIds = entry.SelectedPages;

                //Remove unused pages
                var differentpageList = pageIds.Except(selectedPageIds).ToList();
                if (differentpageList.Any())
                {
                    foreach (var pageId in differentpageList)
                    {
                        var pageModules = UnitOfWork.PageModuleRepository.GetListByPageId(null, pageId, null);
                        if (pageModules != null)
                        {
                            foreach (var pageModule in pageModules)
                            {
                                var item = new PageModule
                                {
                                    PageId = pageModule.PageId,
                                    ModuleId = pageModule.ModuleId
                                };
                                UnitOfWork.PageModuleRepository.Delete(item);
                                UnitOfWork.SaveChanges();
                            }
                        }
                    }
                }

                //Just insert new pages
                var newPageIds = selectedPageIds.Except(pageIds).ToList();
                if (newPageIds.Any())
                {
                    foreach (var pageId in newPageIds)
                    {
                        bool isDuplicate = UnitOfWork.PageModuleRepository.HasDataExisted(pageId, module.ModuleId);
                        if (!isDuplicate)
                        {
                            var pageModule = new PageModule
                            {
                                PageId = pageId,
                                ModuleId = module.ModuleId,
                                IsVisible = true,
                                ModuleOrder = UnitOfWork.PageModuleRepository.GetNewModuleOrder()
                            };
                            UnitOfWork.PageModuleRepository.Insert(pageModule);
                            UnitOfWork.SaveChanges();
                        }
                    }
                }
            }

            if (entry.ExistedModuleCapabilities != null)
            {
                var capabilityIds = UnitOfWork.ModuleCapabilityRepository.GetModuleCapabilitiesByModuleId(module.ModuleId, null).Select(x => x.CapabilityId).ToList();
                var selectedCapilityIds = entry.ExistedModuleCapabilities.Select(x => x.CapabilityId).ToList();

                //Remove unused capabilities
                var differentCapabilityIds = capabilityIds.Except(selectedCapilityIds).ToList();
                if (differentCapabilityIds.Any())
                {
                    foreach (var capabilityId in differentCapabilityIds)
                    {
                        var capability = UnitOfWork.ModuleCapabilityRepository.FindById(capabilityId);
                        if (capability != null)
                        {
                            UpdateModuleCapabilityStatus(capability.CapabilityId, ModuleCapabilityStatus.InActive);

                            var modulePermissions = GetModulePermissions(entry.ModuleId, capabilityId);
                            if (modulePermissions != null)
                            {
                                foreach (var modulePermission in modulePermissions)
                                {
                                    UpdateModulePermissionStatus(modulePermission.RoleId, modulePermission.ModuleId, modulePermission.CapabilityId, false);
                                }
                            }
                        }
                    }
                }
            }

            //Just insert new capabilities
            if (entry.ModuleCapabilities != null && entry.ModuleCapabilities.Any())
            {
                CreateModuleCapabilities(roleId, entry.ModuleId, entry.ModuleCapabilities);
            }
        }
        public void UpdateModuleStatus(int moduleId, ModuleStatus status)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.ModuleRepository.FindById(moduleId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundModule, "ModuleId", moduleId));
                throw new ValidationError(dataViolations);
            }

            entity.IsActive = status;
            UnitOfWork.ModuleRepository.Update(entity);
            UnitOfWork.SaveChanges();

            //Update page module
            var pageModules = UnitOfWork.PageModuleRepository.GetPageModulesByModuleId(moduleId, null);
            if (pageModules != null)
            {
                foreach (var pageModule in pageModules)
                {
                    var pageModuleEntity = UnitOfWork.PageModuleRepository.GetDetailsByPageIdModuleId(pageModule.PageId, pageModule.ModuleId);
                    pageModule.IsVisible = Convert.ToBoolean(status);
                    UnitOfWork.PageModuleRepository.Update(pageModuleEntity);
                }
                UnitOfWork.SaveChanges();
            }

            var modulePermissions = GetModulePermissions(moduleId);
            if (modulePermissions != null)
            {
                foreach (var modulePermission in modulePermissions)
                {
                    UpdateModulePermissionStatus(modulePermission.RoleId, modulePermission.ModuleId, modulePermission.CapabilityId, false);
                }
            }

        }
        public void DeleteModule(int id)
        {
            var modules = UnitOfWork.PageModuleRepository.GetPageModulesByModuleId(id, null);
            if (modules != null)
            {
                foreach (var pageModuleId in modules.Select(x => x.PageModuleId))
                {
                    var pageModule = UnitOfWork.PageModuleRepository.FindById(pageModuleId);
                    UnitOfWork.PageModuleRepository.Delete(pageModule);
                }
            }

            var entity = UnitOfWork.ModuleRepository.FindById(id);
            UnitOfWork.ModuleRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }


        #endregion

        #region MODULE CAPABILITY
        public List<ModuleCapabilityEditEntry> GetModuleCapabilities(int moduleId)
        {
            var capabilities = new List<ModuleCapabilityEditEntry>();
            var lst = UnitOfWork.ModuleCapabilityRepository.GetModuleCapabilitiesByModuleId(moduleId, null).ToList();
            if (lst.Any())
            {
                capabilities.AddRange(lst.Select(item => new ModuleCapabilityEditEntry
                {
                    ModuleId = item.ModuleId,
                    CapabilityId = item.CapabilityId,
                    CapabilityName = item.CapabilityName,
                    CapabilityCode = item.CapabilityCode,
                    Description = item.Description,
                    IsActive = (item.IsActive != null && item.IsActive == ModuleCapabilityStatus.Active)
                }));
            }
            return capabilities;
        }
        public IEnumerable<ModuleCapabilityDetail> GetModuleCapabilitiesByModuleId(int moduleId, int? isActive)
        {
            var lst = UnitOfWork.ModuleCapabilityRepository.GetModuleCapabilitiesByModuleId(moduleId, isActive);
            return lst.ToDtos<ModuleCapability, ModuleCapabilityDetail>();
        }
        public MultiSelectList PopulateModuleCapabilityMultiSelectList(int moduleId, ModuleCapabilityStatus? isActive, string[] selectedValues)
        {
            return UnitOfWork.ModuleCapabilityRepository.PopulateModuleCapabilityMultiSelectList(moduleId, isActive, selectedValues);
        }
        public ModuleCapabilityDetail CreateModuleCapability(int moduleId, CapabilityEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullCapabilityEntry, "CapabilityEntry"));
                throw new ValidationError(violations);
            }

            var item = UnitOfWork.ModuleCapabilityRepository.GetDetails(moduleId, entry.CapabilityName);
            if (item != null) return null;

            if (string.IsNullOrEmpty(entry.CapabilityName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullCapabilityName, "CapabilityName", entry.CapabilityName, ErrorMessage.Messages[ErrorCode.NullCapabilityName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.CapabilityName.Length > 150)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCapabilityName, "CapabilityName", entry.CapabilityName,
                        ErrorMessage.Messages[ErrorCode.InvalidCapabilityName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool flag = UnitOfWork.ModuleCapabilityRepository.HasModuleCapabilityNameExisted(entry.CapabilityName, moduleId);
                    if (flag)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateCapabilityName, "CapabilityName",
                            entry.CapabilityName, ErrorMessage.Messages[ErrorCode.DuplicateCapabilityName]));
                        throw new ValidationError(violations);
                    }
                }
            }

            if (string.IsNullOrEmpty(entry.CapabilityCode))
            {
                violations.Add(new RuleViolation(ErrorCode.NullCapabilityCode, "CapabilityCode", entry.CapabilityCode, ErrorMessage.Messages[ErrorCode.NullCapabilityName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.CapabilityCode.Length > 256)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCapabilityCode, "CapabilityCode", entry.CapabilityCode,
                        ErrorMessage.Messages[ErrorCode.InvalidCapabilityCode]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool flag = UnitOfWork.ModuleCapabilityRepository.HasModuleCapabilityCodeExisted(entry.CapabilityCode, moduleId);
                    if (flag)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateCapabilityCode, "CapabilityCode",
                            entry.CapabilityCode, ErrorMessage.Messages[ErrorCode.DuplicateCapabilityCode]));
                        throw new ValidationError(violations);
                    }
                }
            }

            var capability = new ModuleCapability
            {
                ModuleId = moduleId,
                CapabilityName = entry.CapabilityName,
                CapabilityCode = entry.CapabilityCode,
                Description = entry.Description,
                DisplayOrder = UnitOfWork.ModuleCapabilityRepository.GetListOrder(),
                IsActive = entry.IsActive ? ModuleCapabilityStatus.Active : ModuleCapabilityStatus.InActive
            };

            var entity = UnitOfWork.ModuleCapabilityRepository.Insert(capability);
            UnitOfWork.SaveChanges();

            return entity.ToDto<ModuleCapability, ModuleCapabilityDetail>();
        }
        public void CreateModuleCapabilities(Guid roleId, int moduleId, List<CapabilityEntry> moduleCapabilities)
        {
            foreach (var capabilityEntry in moduleCapabilities)
            {
                var moduleCapability = CreateModuleCapability(moduleId, capabilityEntry);

                if (moduleCapability != null)
                {
                    var modulePermissionEntry = new ModulePermissionEntry
                    {
                        RoleId = roleId,
                        ModuleId = moduleId,
                        CapabilityId = moduleCapability.CapabilityId,
                        CapabilityCode = moduleCapability.CapabilityCode,
                        UserIds = null,
                        AllowAccess = true
                    };
                    CreateModulePermission(modulePermissionEntry);
                }
            }
        }
        public void UpdateModuleCapability(int id, ModuleCapabilityEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ModuleCapabilityRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundModuleCapability, "ModuleCapabilityId", id));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.CapabilityName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullCapabilityName, "CapabilityName", entry.CapabilityName, ErrorMessage.Messages[ErrorCode.NullCapabilityName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.CapabilityName.Length > 150)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCapabilityName, "CapabilityName", entry.CapabilityName,
                        ErrorMessage.Messages[ErrorCode.InvalidCapabilityName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.CapabilityName != entry.CapabilityName)
                    {
                        bool flag =
                            UnitOfWork.ModuleCapabilityRepository.HasModuleCapabilityNameExisted(entry.CapabilityName, entity.ModuleId);
                        if (flag)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateCapabilityName, "CapabilityName",
                                entry.CapabilityName, ErrorMessage.Messages[ErrorCode.DuplicateCapabilityName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(entry.CapabilityCode))
            {
                violations.Add(new RuleViolation(ErrorCode.NullCapabilityCode, "CapabilityCode", entry.CapabilityCode, ErrorMessage.Messages[ErrorCode.NullCapabilityName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.CapabilityCode.Length > 256)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCapabilityCode, "CapabilityCode", entry.CapabilityCode,
                        ErrorMessage.Messages[ErrorCode.InvalidCapabilityCode]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.CapabilityCode != entry.CapabilityCode)
                    {
                        bool flag =
                            UnitOfWork.ModuleCapabilityRepository.HasModuleCapabilityCodeExisted(entry.CapabilityCode, entity.ModuleId);
                        if (flag)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateCapabilityCode, "CapabilityCode",
                                entry.CapabilityCode, ErrorMessage.Messages[ErrorCode.DuplicateCapabilityCode]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            entity.CapabilityName = entry.CapabilityName;
            entity.CapabilityCode = entry.CapabilityCode;
            UnitOfWork.ModuleCapabilityRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateModuleCapabilityListOrder(int id, int listOrder)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.ModuleCapabilityRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundModuleCapability, "ModuleCapabilityId", id));
                throw new ValidationError(dataViolations);
            }

            entity.DisplayOrder = listOrder;
            UnitOfWork.ModuleCapabilityRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateModuleCapabilityStatus(int id, ModuleCapabilityStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ModuleCapabilityRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundModuleCapability, "ModuleCapabilityId", id));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ModuleCapabilityStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }

            entity.IsActive = status;
            UnitOfWork.ModuleCapabilityRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteModuleCapability(int id)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.ModuleCapabilityRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundModuleCapability, "ModuleCapabilityId", id));
                throw new ValidationError(dataViolations);
            }

            UnitOfWork.PageModuleRepository.Delete(entity);
            UnitOfWork.SaveChanges();

        }
        public void DeleteModuleCapabilityByModuleId(int moduleId)
        {
            var capabilities = UnitOfWork.ModuleCapabilityRepository.GetModuleCapabilitiesByModuleId(moduleId, null);
            if (capabilities != null)
            {
                foreach (var capabilityId in capabilities.Select(x => x.CapabilityId))
                {
                    var pageModule = UnitOfWork.PageModuleRepository.FindById(capabilityId);
                    UnitOfWork.PageModuleRepository.Delete(pageModule);
                }
                UnitOfWork.SaveChanges();
            }
        }

        #endregion

        #region Module Permission

        public ModuleRolePermissionEntry GetModuleRolePermissionEntry(Guid applicationId, int moduleId)
        {
            var module = GetModuleDetails(moduleId);
            if (module == null) return null;

            var roles = RoleService.GetRoles(applicationId, true).ToList();
            if (!roles.Any()) return null;

            var lst = new List<ModuleRolePermissionDetail>();

            var defaultCapabilities = GetModuleCapabilitiesByModuleId(moduleId, null).ToList();
            if (!defaultCapabilities.Any())
            {
                lst.AddRange(roles.Select(role => new ModuleRolePermissionDetail
                {
                    RoleId = role.RoleId,
                    Role = role,
                    ModuleCapabilities = null,
                }));

                return new ModuleRolePermissionEntry
                {
                    Module = module,
                    ModuleCapabilities = defaultCapabilities,
                    ModuleRolePermissions = lst
                };
            }
            else
            {
                foreach (var role in roles)
                {
                    var capabilityAccessList = new List<ModuleCapabilityAccess>();
                    var capabilitiesInModulePermission = GetModuleCapabilityAccessListByModuleId(moduleId, role.RoleId);
                    if (!capabilitiesInModulePermission.Any())
                    {
                        capabilityAccessList.AddRange(defaultCapabilities.Select(item => new ModuleCapabilityAccess
                        {
                            CapabilityId = item.CapabilityId,
                            CapabilityCode = item.CapabilityCode,
                            ModuleId = item.ModuleId,
                            AllowAccess = false
                        }));

                        lst.Add(new ModuleRolePermissionDetail
                        {
                            RoleId = role.RoleId,
                            Role = role,
                            ModuleCapabilities = capabilityAccessList,
                        });
                    }
                    else
                    {
                        //Add capabilities int module permission
                        var defaultCapabilityIds = defaultCapabilities.Select(x => x.CapabilityId).ToList();
                        var capabilityIdsInModulePermission = capabilitiesInModulePermission.Select(x => x.CapabilityId).ToList();
                        var intersectionCapabilityIds = capabilityIdsInModulePermission.Intersect(defaultCapabilityIds).ToList();
                        if (intersectionCapabilityIds.Any())
                        {
                            capabilityAccessList.AddRange(from intersectionCapabilityId in intersectionCapabilityIds
                                                          select capabilitiesInModulePermission.FirstOrDefault(x => x.CapabilityId == intersectionCapabilityId)
                                into capability
                                                          where capability != null
                                                          select new ModuleCapabilityAccess
                                                          {
                                                              CapabilityId = capability.CapabilityId,
                                                              CapabilityCode = capability.CapabilityCode,
                                                              ModuleId = capability.ModuleId,
                                                              AllowAccess = true
                                                          });
                        }

                        //Add the different capabilities of default module capabilities
                        var differentCapabilityIds = defaultCapabilityIds.Except(capabilityIdsInModulePermission).ToList();
                        if (differentCapabilityIds.Any())
                        {
                            capabilityAccessList.AddRange(from differentCapabilityId in differentCapabilityIds
                                                          select defaultCapabilities.FirstOrDefault(x => x.CapabilityId == differentCapabilityId)
                                                          into capability
                                                          where capability != null
                                                          select new ModuleCapabilityAccess
                                                          {
                                                              CapabilityId = capability.CapabilityId,
                                                              CapabilityCode = capability.CapabilityCode,
                                                              ModuleId = capability.ModuleId,
                                                              AllowAccess = false
                                                          });
                        }

                        lst.Add(new ModuleRolePermissionDetail
                        {
                            RoleId = role.RoleId,
                            ModuleCapabilities = capabilityAccessList,
                            Role = role
                        });
                    }
                }
            }

            var entry = new ModuleRolePermissionEntry
            {
                Module = module,
                ModuleCapabilities = defaultCapabilities,
                ModuleRolePermissions = lst
            };

            return entry;
        }

        public List<ModuleRolePermissionDetail> GetModuleRolePermission(Guid applicationId, int moduleId)
        {
            var roles = RoleService.GetRoles(applicationId, true).ToList();
            if (!roles.Any()) return null;

            var defaultCapabilities = GetModuleCapabilitiesByModuleId(moduleId, null).ToList();
            if (!defaultCapabilities.Any()) return null;

            var lst = new List<ModuleRolePermissionDetail>();

            foreach (var role in roles)
            {
                var moduleCapabilitiesInModulePermission = GetModuleCapabilityAccessListByModuleId(moduleId, role.RoleId);
                if (!moduleCapabilitiesInModulePermission.Any()) return lst;

                var moduleCapabilityAccessList = new List<ModuleCapabilityAccess>();
                moduleCapabilityAccessList.AddRange(moduleCapabilitiesInModulePermission.Select(item => new ModuleCapabilityAccess
                {
                    CapabilityId = item.CapabilityId,
                    CapabilityCode = item.CapabilityCode,
                    ModuleId = item.ModuleId,
                    AllowAccess = true
                }));

                lst.Add(new ModuleRolePermissionDetail
                {
                    RoleId = role.RoleId,
                    ModuleCapabilities = moduleCapabilityAccessList,
                    Role = role
                });
            }
            return lst;
        }

        private List<ModuleCapabilityDetail> GetModuleCapabilityAccessListByModuleId(int moduleId, Guid roleId)
        {
            var capabilities = new List<ModuleCapabilityDetail>();
            var lst = UnitOfWork.ModulePermissionRepository.GetModulePermissionsByRoleId(roleId, moduleId).ToList();
            if (lst.Any())
            {
                capabilities.AddRange(lst.Select(item => new ModuleCapabilityDetail
                {
                    ModuleId = item.ModuleId,
                    CapabilityId = item.CapabilityId,
                    CapabilityCode = item.CapabilityCode,
                    CapabilityName = item.ModuleCapability.CapabilityName,
                    Description = item.ModuleCapability.Description,
                    DisplayOrder = item.ModuleCapability.DisplayOrder,
                    IsActive = item.ModuleCapability.IsActive,
                }));
            }
            return capabilities;
        }

        public IEnumerable<ModulePermissionDetail> GetModulePermissions(int moduleId)
        {
            var lst = UnitOfWork.ModulePermissionRepository.GetModulePermissions(moduleId);
            return lst.ToDtos<ModulePermission, ModulePermissionDetail>();
        }
        public IEnumerable<ModulePermissionDetail> GetModulePermissions(int moduleId, int capabilityId)
        {
            var lst = UnitOfWork.ModulePermissionRepository.GetModulePermissions(moduleId, capabilityId);
            return lst.ToDtos<ModulePermission, ModulePermissionDetail>();
        }
        public IEnumerable<ModulePermissionDetail> GetModulePermissionsByRoleId(Guid roleId)
        {
            var lst = UnitOfWork.ModulePermissionRepository.GetModulePermissionsByRoleId(roleId);
            return lst.ToDtos<ModulePermission, ModulePermissionDetail>();
        }
        public IEnumerable<ModulePermissionDetail> GetModulePermissionsByRoleId(Guid roleId, int moduleId)
        {
            var lst = UnitOfWork.ModulePermissionRepository.GetModulePermissionsByRoleId(roleId, moduleId);
            return lst.ToDtos<ModulePermissionInfo, ModulePermissionDetail>();
        }
        public ModulePermissionDetail GetModulePermissionDetail(int id)
        {
            var entity = UnitOfWork.ModulePermissionRepository.FindById(id);
            return entity.ToDto<ModulePermission, ModulePermissionDetail>();
        }
        public void CreateModulePermission(ModulePermissionEntry entry)
        {
            var item = UnitOfWork.ModulePermissionRepository.GetDetails(entry.RoleId, entry.CapabilityId);
            if (item != null) return;

            var entity = new ModulePermission
            {
                RoleId = entry.RoleId,
                ModuleId = entry.ModuleId,
                CapabilityId = entry.CapabilityId,
                CapabilityCode = entry.CapabilityCode,
                UserIds = entry.UserIds,
                AllowAccess = entry.AllowAccess,
            };

            UnitOfWork.ModulePermissionRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void EditModulePermission(int id, ModulePermissionEntry entry)
        {
            var entity = UnitOfWork.ModulePermissionRepository.FindById(id);
            if (entity == null) return;

            UnitOfWork.ModulePermissionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateModulePermissionStatus(Guid roleId, int moduleId, int capabilityId, bool status)
        {
            var item = UnitOfWork.ModulePermissionRepository.GetDetails(roleId, capabilityId);
            if (item == null)
            {
                var entity = new ModulePermission
                {
                    RoleId = roleId,
                    ModuleId = moduleId,
                    CapabilityId = capabilityId,
                    AllowAccess = status
                };

                UnitOfWork.ModulePermissionRepository.Insert(entity);
                UnitOfWork.SaveChanges();
            }
            else
            {
                item.AllowAccess = status;
                UnitOfWork.ModulePermissionRepository.Update(item);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteModulePermission(int id)
        {
            var pageModule = UnitOfWork.ModulePermissionRepository.FindById(id);
            if (pageModule == null) return;

            UnitOfWork.ModulePermissionRepository.Delete(pageModule);
            UnitOfWork.SaveChanges();
        }
        public void DeleteModulePermissionByModuleId(int moduleId)
        {
            var modulePermissions = UnitOfWork.ModulePermissionRepository.GetModulePermissions(moduleId);
            if (modulePermissions == null) return;

            foreach (var item in modulePermissions)
            {
                UnitOfWork.ModulePermissionRepository.Delete(item);
            }
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region PAGE MODULE

        public void CreatePageModules(int moduleId, List<int> selectedPage)
        {
            foreach (var pageId in selectedPage)
            {
                bool isDuplicate = UnitOfWork.PageModuleRepository.HasDataExisted(pageId, moduleId);
                if (!isDuplicate)
                {
                    var pageModule = new PageModule
                    {
                        PageId = pageId,
                        ModuleId = moduleId,
                        IsVisible = true,
                        ModuleOrder = UnitOfWork.PageModuleRepository.GetNewModuleOrder()
                    };
                    UnitOfWork.PageModuleRepository.Insert(pageModule);
                }
            }
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Dipose

        private bool _disposed = false;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    RoleService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
