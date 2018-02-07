using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Permission;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.SystemManagement.Validation;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork) : base(unitOfWork){ }

        #region Role
        public IEnumerable<RoleDetail> GetRoles(Guid applicationId, bool? status)
        {
            var lst = UnitOfWork.RoleRepository.GetRoles(applicationId, status);
            return lst.ToDtos<Role, RoleDetail>();
        }
       
        public IEnumerable<RoleDetail> GetRoles(Guid applicationId, bool? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.RoleRepository.GetRoles(applicationId, status, ref recordCount, orderBy, page,
                pageSize);
            return lst.ToDtos<Role, RoleDetail>();
        }

        public IEnumerable<RoleInfoDetail> SearchRoles(Guid applicationId, RoleSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.RoleRepository.SearchRoles(applicationId, filter.GroupId, filter.RoleName, filter.Status, ref recordCount, orderBy, page,
                pageSize).ToList();
            var roles = new List<RoleInfoDetail>();
            if (lst.Any())
            {
                roles.AddRange(from item in lst
                               let groups = GetGroups(applicationId, item.RoleId)
                               let users = GetUsers(item.RoleId)
                               select new RoleInfoDetail
                               {
                                   ApplicationId = item.ApplicationId,
                                   RoleId = item.RoleId,
                                   RoleName = item.RoleName,
                                   Description = item.Description,
                                   ListOrder = item.ListOrder,
                                   IsActive = item.IsActive,
                                   Groups = groups,
                                   Users = users
                               });
            }
            return roles;
        }
        public IEnumerable<RoleInfoDetail> GetRolesByUserId(Guid applicationId, Guid userId, bool? status = null)
        {
            var lst = UnitOfWork.RoleRepository.GetRoles(applicationId, userId, status).ToList();
            var roles = new List<RoleInfoDetail>();
            if (lst.Any())
            {
                roles.AddRange(from item in lst
                               let groups = GetGroups(applicationId, item.RoleId)
                               let users = GetUsers(item.RoleId)
                               select new RoleInfoDetail
                               {
                                   ApplicationId = item.ApplicationId,
                                   RoleId = item.RoleId,
                                   RoleName = item.RoleName,
                                   Description = item.Description,
                                   ListOrder = item.ListOrder,
                                   IsActive = item.IsActive,
                                   Groups = groups,
                                   Users = users
                               });
            }
            return roles;
        }
        public SelectList PopulateStatusDropDownList(string selectedValue = null, bool? isShowSelectText = true)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Active, Value = "1"},
                new SelectListItem {Text = LanguageResource.InActive, Value = "0"}
            };
            if (isShowSelectText != null && isShowSelectText == true)
                lst.Insert(0, new SelectListItem
                {
                    Text = $"--- {LanguageResource.Select} ---",
                    Value = ""
                });

            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        public SelectList PopulateRoleDropDownList(bool? status = null, string selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.RoleRepository.PopulateRoleToDropDownList(status, selectedValue, isShowSelectText);
        }
        public MultiSelectList PopulateRoleMultiSelectList(bool? status = null, string[] selectedValues=null, bool? isShowSelectText = false)
        {
            return UnitOfWork.RoleRepository.PopulateRoleMultiSelectList(status, selectedValues, isShowSelectText);
        }

        public MultiSelectList PopulateSelectedRoleMultiSelectList(Guid userId, bool? status = null,
            string[] selectedValues = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.RoleRepository.PopulateRoleMultiSelectList(userId, status, selectedValues, isShowSelectText);
        }
        public RoleDetail GetRoleDetails(Guid roleId)
        {
            var entity = UnitOfWork.RoleRepository.FindById(roleId);
            return entity.ToDto<Role, RoleDetail>();
        }

        public void CreateRole(Guid applicationId, RoleEntry entry)
        {
            ISpecification<RoleEntry> validator = new RoleEntryValidator(UnitOfWork, PermissionLevel.Create);
            var dataViolations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            var result = UnitOfWork.RoleRepository.IsRoleExisted(applicationId, entry.RoleName);
            if (result)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.DuplicateRoleName, "RoleName", entry.RoleName, ErrorMessage.Messages[ErrorCode.DuplicateRoleName]));
                throw new ValidationError(dataViolations);
            }

            var entity = entry.ToEntity<RoleEntry, Role>();
            entity.RoleId = Guid.NewGuid();
            entity.ListOrder = UnitOfWork.RoleRepository.GetNewListOrder();
            entity.ApplicationId = applicationId;
            entity.LoweredRoleName = entry.RoleName.ToLower();

            UnitOfWork.RoleRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            //Insert Groups
            if (entry.SelectedGroups != null)
            {
                CreateRoleGroups(entity.RoleId, entry.SelectedGroups);
            }
        }

        public void UpdateRole(Guid applicationId, RoleEditEntry entry)
        {
            ISpecification<RoleEntry> validator = new RoleEntryValidator(UnitOfWork, PermissionLevel.Create);
            var dataViolations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.RoleRepository.FindById(entry.RoleId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundRole, "RoleId", entry.RoleId, ErrorMessage.Messages[ErrorCode.NotFoundRole]));
                throw new ValidationError(dataViolations);
            }

            if (entity.RoleName != entry.RoleName)
            {
                var result = UnitOfWork.RoleRepository.IsRoleExisted(applicationId, entry.RoleName);
                if (result)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.DuplicateRoleName, "RoleName", entry.RoleName, ErrorMessage.Messages[ErrorCode.DuplicateRoleName]));
                    throw new ValidationError(dataViolations);
                }
            }

            entity.RoleName = entry.RoleName;
            entity.RoleCode = entry.RoleCode;
            entity.LoweredRoleName = entry.RoleName.ToLower();
            entity.Description = entry.Description;
            entity.IsActive = entry.IsActive;

            UnitOfWork.RoleRepository.Update(entity);
            UnitOfWork.SaveChanges();
            
            //Update selected groups
            if (entry.SelectedGroups != null)
            {
                UpdateRoleGroups(applicationId, entity.RoleId, entry.SelectedGroups);
            }
        }

        public void UpdateRoleStatus(Guid id, bool status)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.RoleRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundRole, "RoleId", id, ErrorMessage.Messages[ErrorCode.NotFoundRole]));
                throw new ValidationError(dataViolations);
            }

            entity.IsActive = status;
            UnitOfWork.RoleRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region USER ROLE
        public IEnumerable<UserRoleInfoDetail> GetUserRolesByUserId(Guid applicationId, Guid userId, bool? status=null)
        {
            var lst = UnitOfWork.UserRoleRepository.GetRoles(userId, status).ToList();
            var userRoles = new List<UserRoleInfoDetail>();
            if (lst.Any())
            {
                userRoles.AddRange(from item in lst
                               let groups = GetGroups(applicationId, item.RoleId)
                               let users = GetUsers(item.RoleId)
                               select new UserRoleInfoDetail
                               {
                                   UserRoleId = item.UserRoleId,
                                   UserId = item.UserId,
                                   RoleId = item.RoleId,
                                   EffectiveDate = item.EffectiveDate,
                                   ExpiryDate = item.ExpiryDate,
                                   IsTrialUsed = item.IsTrialUsed,
                                   IsDefaultRole = item.IsDefaultRole??false,
                                   User = item.User.ToDto<User, UserDetail>(),
                                   Role = item.Role.ToDto<Role, RoleDetail>()
                               });
            }
            return userRoles;
        }

        public IEnumerable<UserRoleInfoDetail> GetUserRolesByUserName(Guid applicationId, string userName, bool? status = null)
        {
            var lst = UnitOfWork.UserRoleRepository.GetUserRolesByUserName(userName, status).ToList();
            var userRoles = new List<UserRoleInfoDetail>();
            if (lst.Any())
            {
                userRoles.AddRange(from item in lst
                    let groups = GetGroups(applicationId, item.RoleId)
                    let users = GetUsers(item.RoleId)
                    select new UserRoleInfoDetail
                    {
                        UserRoleId = item.UserRoleId,
                        UserId = item.UserId,
                        RoleId = item.RoleId,
                        EffectiveDate = item.EffectiveDate,
                        ExpiryDate = item.ExpiryDate,
                        IsTrialUsed = item.IsTrialUsed,
                        IsDefaultRole = item.IsDefaultRole ?? false,
                        User = item.User.ToDto<User, UserDetail>(),
                        Role = item.Role.ToDto<Role, RoleDetail>()
                    });
            }
            return userRoles;
        }
        

        public IEnumerable<UserRoleDetail> GetUsersByRoleId(Guid roleId)
        {
            var lst = UnitOfWork.UserRoleRepository.GetUsersByRoleId(roleId);
            return lst.ToDtos<UserRole, UserRoleDetail>();
        }
        public IEnumerable<UserDetail> GetUsers(Guid roleId)
        {
            var lst = UnitOfWork.UserRoleRepository.GetUsers(roleId);
            return lst.ToDtos<User, UserDetail>();
        }
        #endregion

        #region Group

        
        public SelectList PopulateGroupDropDownList(Guid applicationId, bool? status = null, string selectedValue = null,
            bool? isShowSelectText = false)
        {
            return UnitOfWork.GroupRepository.PopulateGroupDropDownList(applicationId, status, selectedValue, isShowSelectText);
        }
        public SelectList PopulateGroupDropDownList(Guid applicationId, Guid roleId, bool? status = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.GroupRepository.PopulateGroupDropDownList(applicationId, roleId, status, isShowSelectText);
        }

        public MultiSelectList PopulateGroupMultiSelectList(Guid applicationId, bool? status = null, string[] selectedValues = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.GroupRepository.PopulateGroupMultiSelectList(applicationId, status, selectedValues, isShowSelectText);
        }

        public MultiSelectList PopulateGroupMultiSelectList(Guid applicationId, Guid roleId, bool? status = null, string[] selectedValues = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.RoleGroupRepository.PopulateGroupMultiSelectList(applicationId, roleId, status, selectedValues, isShowSelectText);
        }
        public IEnumerable<RoleGroupDetail> GetGroups(Guid applicationId, Guid roleId, bool? status = null)
        {
            var lst = UnitOfWork.RoleGroupRepository.GetRoleGroupsByRoleId(applicationId, roleId, status);
            return lst.ToDtos<RoleGroup, RoleGroupDetail>();
        }
        public IEnumerable<GroupDetail> GetGroups(Guid applicationId, GroupSearchEntry entry, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.GroupRepository.GetList(applicationId, entry.GroupName, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<Group, GroupDetail>();
        }
        public GroupDetail GetGroupDetails(Guid roleGroupId)
        {
            var entity = UnitOfWork.GroupRepository.FindById(roleGroupId);
            return entity.ToDto<Group, GroupDetail>();
        }
        public void CreateGroup(Guid applicationId, GroupEntry entry)
        {
            ISpecification<GroupEntry> validator = new RoleGroupEntryValidator(UnitOfWork, PermissionLevel.Create);
            var dataViolations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            var result = UnitOfWork.GroupRepository.HasGroupExisted(applicationId, entry.GroupName);
            if (result)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.DuplicateGroupName, "GroupName", entry.GroupName, ErrorMessage.Messages[ErrorCode.DuplicateGroupName]));
                throw new ValidationError(dataViolations);
            }

            var entity = entry.ToEntity<GroupEntry, Group>();
            entity.ApplicationId = applicationId;
            entity.ListOrder = UnitOfWork.GroupRepository.GetNewListOrder();

            UnitOfWork.GroupRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateGroup(Guid applicationId, Guid roleGroupId, GroupEntry entry)
        {
            ISpecification<GroupEntry> validator = new RoleGroupEntryValidator(UnitOfWork, PermissionLevel.Create);
            var dataViolations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.GroupRepository.FindById(roleGroupId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundForRoleGroup, "RoleGroupId", roleGroupId, ErrorMessage.Messages[ErrorCode.NotFoundForRoleGroup]));
                throw new ValidationError(dataViolations);
            }

            if (entity.GroupName != entry.GroupName)
            {
                var result = UnitOfWork.GroupRepository.HasGroupExisted(applicationId, entry.GroupName);
                if (result)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.DuplicateGroupName, "GroupName", entry.GroupName, ErrorMessage.Messages[ErrorCode.DuplicateGroupName]));
                    throw new ValidationError(dataViolations);
                }
            }

            entity.GroupName = entry.GroupName;
            entity.Description = entry.Description;
            entity.IsActive = entry.IsActive;

            UnitOfWork.GroupRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateGroupStatus(Guid roleGroupId, bool status)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.GroupRepository.FindById(roleGroupId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundForRoleGroup, "RoleGroupId", roleGroupId, ErrorMessage.Messages[ErrorCode.NotFoundForRoleGroup]));
                throw new ValidationError(dataViolations);
            }

            entity.IsActive = status;

            UnitOfWork.GroupRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region ROLE GROUP
        
        public IEnumerable<RoleGroupDetail> GetRoleGroups(Guid applicationId, Guid roleId, bool? status = null)
        {
            var lst = UnitOfWork.RoleGroupRepository.GetRoleGroupsByRoleId(applicationId, roleId, status).ToList();
            var groups = new List<RoleGroupDetail>();
            if (lst.Any())
            {
                groups.AddRange(from item in lst
                               let roleDetail = GetRoleDetails(item.RoleId)
                               let groupDetail = GetGroupDetails(item.GroupId)
                               select new RoleGroupDetail
                               {
                                   RoleId = item.RoleId,
                                   GroupId = item.GroupId,
                                   IsDefaultGroup = item.IsDefaultGroup??false,
                                   Role = roleDetail,
                                   Group = groupDetail
                               });
            }
            return groups;
        }
        
        public RoleGroupDetail GetRoleGroupDetail(int roleGroupId)
        {
            var entity = UnitOfWork.RoleGroupRepository.GetRoleGroupDetail(roleGroupId);
            var item = new RoleGroupDetail
            {
                RoleGroupId = entity.RoleGroupId,
                RoleId = entity.RoleId,
                GroupId = entity.GroupId,
                IsDefaultGroup = entity.IsDefaultGroup,
                Role = entity.Role.ToDto<Role, RoleDetail>(),
                Group = entity.Group.ToDto<Group, GroupDetail>()
            };
            return item;
        }

        public RoleGroupInfoDetail GetRoleGroupDetails(Guid roleId, Guid groupId)
        {
            var entity = UnitOfWork.RoleGroupRepository.GetRoleGroupDetails(roleId, groupId);
            var item = new RoleGroupInfoDetail
            {
                RoleGroupId = entity.RoleGroupId,
                RoleId = entity.RoleId,
                GroupId = entity.GroupId,
                IsDefaultGroup = entity.IsDefaultGroup,
                Role = entity.Role.ToDto<Role, RoleDetail>(),
                Group = entity.Group.ToDto<Group, GroupDetail>()
            };
            return item;
        }


        public void CreateRoleGroups(Guid roleId, List<Guid> groupIds)
        {
            if (groupIds.Any())
            {
                foreach (var groupId in groupIds)
                {
                    bool isDuplicate = UnitOfWork.RoleGroupRepository.HasDataExisted(roleId, groupId);
                    if (!isDuplicate)
                    {
                        var roleGroup = new RoleGroup
                        {
                            GroupId = groupId,
                            RoleId = roleId,
                            IsDefaultGroup = true
                        };
                        UnitOfWork.RoleGroupRepository.Insert(roleGroup);
                    }
                }
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteRoleGroup(Guid roleId, Guid groupId)
        {
            var roleGroup = UnitOfWork.RoleGroupRepository.GetDetails(roleId, groupId);
            if (roleGroup != null)
            {
                UnitOfWork.RoleGroupRepository.Delete(roleGroup);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteRoleGroups(Guid roleId, List<Guid> groupIds)
        {
            if (groupIds.Any())
            {
                foreach (var groupId in groupIds)
                {
                    DeleteRoleGroup(roleId, groupId);
                }
            }
        }
        public void UpdateRoleGroups(Guid applicationId, Guid roleId, List<Guid> selectedGroupIds)
        {
            //Update page module
            if (selectedGroupIds != null)
            {
                var groupIds = UnitOfWork.RoleGroupRepository.GetRoleGroupsByRoleId(applicationId, roleId).Select(x => x.GroupId).ToList();

                //Remove unused pages
                var differentGroupList = groupIds.Except(selectedGroupIds).ToList();
                if (differentGroupList.Any())
                {
                    DeleteRoleGroups(roleId, differentGroupList);
                }

                //Just insert new groups
                var newGroupIds = selectedGroupIds.Except(groupIds).ToList();
                if (newGroupIds.Any())
                {
                    CreateRoleGroups(roleId, newGroupIds);
                }
            }
        }

        #endregion
    }
}
