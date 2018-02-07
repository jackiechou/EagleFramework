using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement
{
    public class PermissionService : BaseService, IPermissionService
    {
        private IRoleService RoleService { get; set; }

        public PermissionService(IUnitOfWork unitOfWork, IRoleService roleService) : base(unitOfWork)
        {
            RoleService = roleService;
        }

        #region PERMISSION
        public IEnumerable<PermissionDetail> GetPermissions(bool? status)
        {
            var lst = UnitOfWork.PermissionRepository.GetPermissions(status);
            return lst.ToDtos<Permission, PermissionDetail>();
        }

        public PermissionDetail GetDetails(int id)
        {
            var entity = UnitOfWork.PermissionRepository.FindById(id);
            return entity.ToDto<Permission, PermissionDetail>();
        }

        public void InsertPermission(PermissionEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            if (string.IsNullOrEmpty(entry.PermissionName))
            {
                dataViolations.Add(new RuleViolation(ErrorCode.InvalidPermissionName, "PermissionName", entry.PermissionName, ErrorMessage.Messages[ErrorCode.InvalidPermissionName]));
            }
            else
            {
                bool isDataDuplicate = UnitOfWork.PermissionRepository.HasDataExisted(entry.PermissionName);
                if (isDataDuplicate)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.DuplicatePermissionName, "PermissionName", entry.PermissionName, ErrorMessage.Messages[ErrorCode.DuplicatePermissionName]));
                    throw new ValidationError(dataViolations);
                }
            }

            var entity = entry.ToEntity<PermissionEntry, Permission>();
            entity.DisplayOrder = UnitOfWork.PermissionRepository.GetNewListOrder();
            entity.IsActive = entry.IsActive;

            UnitOfWork.PermissionRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdatePermission(int id, PermissionEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            if (string.IsNullOrEmpty(entry.PermissionName))
            {
                dataViolations.Add(new RuleViolation(ErrorCode.InvalidPermissionName, "PermissionName",
                    entry.PermissionName, ErrorMessage.Messages[ErrorCode.InvalidPermissionName]));
                throw new ValidationError(dataViolations);
            }
            else
            {
                var entity = UnitOfWork.PermissionRepository.FindById(id);
                if (entity == null)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.NotFoundPermission, "PermissionId", entry, ErrorMessage.Messages[ErrorCode.NotFoundPermission]));
                    throw new ValidationError(dataViolations);
                }
                else
                {
                    entity.PermissionName = entry.PermissionName;
                    entity.IsActive = entry.IsActive;

                    UnitOfWork.PermissionRepository.Update(entity);
                    UnitOfWork.SaveChanges();
                }
            }
        }

        public void DeletePermission(int id)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.PermissionRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundPermission, "PermissionId", id, ErrorMessage.Messages[ErrorCode.NotFoundPermission]));
                throw new ValidationError(dataViolations);
            }
            else
            {
                UnitOfWork.PermissionRepository.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

        #region PAGE PERMISSION

        public List<PagePermissionEntry> GetPagePermissions(Guid applicationId, int pageId)
        {
            var pagePermissions = new List<PagePermissionEntry>();

            var roles = RoleService.GetRoles(applicationId, true).ToList();
            if (!roles.Any()) return null;
            
            var existedPagePermissions = UnitOfWork.PagePermissionRepository.GetPagePermissionsByPageId(pageId).ToList();
            foreach (var role in roles)
            {
                bool allowAccess = false;
                string userIds = string.Empty;
                var pagePermission = existedPagePermissions.FirstOrDefault(x => x.PageId == pageId && x.RoleId == role.RoleId);
                if (pagePermission!=null)
                {
                    userIds = pagePermission.UserIds;
                    allowAccess = pagePermission.AllowAccess;
                }

                pagePermissions.Add(new PagePermissionEntry
                {
                    PageId = pageId,
                    RoleId = role.RoleId,
                    AllowAccess = allowAccess,
                    UserIds = userIds,
                    Role = role
                });
            }
            return pagePermissions;
        }
        
        public IEnumerable<PagePermissionDetail> GetPagePermissions(bool? allowAccess)
        {
            var lst = UnitOfWork.PagePermissionRepository.GetPagePermissions(allowAccess);
            return lst.ToDtos<PagePermission, PagePermissionDetail>();
        }
        public IEnumerable<PagePermissionDetail> GetPagePermissionsByPageId(int pageId)
        {
            var lst = UnitOfWork.PagePermissionRepository.GetPagePermissionsByPageId(pageId);
            return lst.ToDtos<PagePermission, PagePermissionDetail>();
        }
        public IEnumerable<PagePermissionDetail> GetPagePermissionsByRoleId(Guid roleId)
        {
            var lst = UnitOfWork.PagePermissionRepository.GetPagePermissionsByRoleId(roleId);
            return lst.ToDtos<PagePermission, PagePermissionDetail>();
        }
        public void InsertPagePermission(PagePermissionEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            if (entry == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NullPagePermissionEntry, "PagePermissionEntry"));
                throw new ValidationError(dataViolations);
            }
            else
            {
                bool isDataDuplicate = UnitOfWork.PagePermissionRepository.HasDataExisted(entry.RoleId, entry.PageId);
                if (isDataDuplicate)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.DuplicateData, "RoleId"));
                    throw new ValidationError(dataViolations);
                }

                var pagePermission = new PagePermission
                {
                    PageId = entry.PageId,
                    RoleId = entry.RoleId,
                    AllowAccess = entry.AllowAccess
                };

                UnitOfWork.PagePermissionRepository.Insert(pagePermission);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdatePagePermission(PagePermissionEntry entry)
        {
            var entity = UnitOfWork.PagePermissionRepository.Find(entry.RoleId, entry.PageId);
            if (entity != null)
            {
                entity.PageId = entry.PageId;
                entity.RoleId = entry.RoleId;
                entity.AllowAccess = entry.AllowAccess;

                UnitOfWork.PagePermissionRepository.Update(entity);
                UnitOfWork.SaveChanges();
            }
            else
            {
                InsertPagePermission(entry);
            }
        }
        public void DeletePagePermissionByPageId(int pageId)
        {
            var lst = UnitOfWork.PagePermissionRepository.GetPagePermissionsByPageId(pageId);
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    var dataViolations = new List<RuleViolation>();
                    var pagePermissions = UnitOfWork.PagePermissionRepository.GetDetails(item.RoleId, item.PageId);
                    if (pagePermissions == null)
                    {
                        dataViolations.Add(new RuleViolation(ErrorCode.NotFoundPagePermission, "RoleId-PageId",
                            $"RoleId: {item.RoleId} - PageId: {item.PageId}", ErrorMessage.Messages[ErrorCode.NotFoundPagePermission]));
                        throw new ValidationError(dataViolations);
                    }
                    else
                    {
                        foreach (var pagePermission in pagePermissions)
                        {
                            pagePermission.AllowAccess = false;
                            UnitOfWork.PagePermissionRepository.Update(pagePermission);
                        }
                    }
                }
                UnitOfWork.SaveChanges();
            }
        }
        public void DeletePagePermission(Guid roleId, int pageId)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.PagePermissionRepository.Find(roleId, pageId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundPagePermission, "PagePermissionId", pageId, ErrorMessage.Messages[ErrorCode.NotFoundPagePermission]));
                throw new ValidationError(dataViolations);
            }
            else
            {
                entity.AllowAccess = false;
                UnitOfWork.PagePermissionRepository.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public void UpdatePermission(int pageId, List<PagePermissionEntry> pagePermissionEntries)
        {
            if (pagePermissionEntries != null)
            {
                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    var newRoleIds = pagePermissionEntries.Select(x => x.RoleId).ToList();
                    var existedPagePermissions = UnitOfWork.PagePermissionRepository.GetPagePermissionsByPageId(pageId).ToList();
                    var existedRoleIdsInExistedPagePermissions = existedPagePermissions.Select(x => x.RoleId).ToList();

                    //Add the different roles against with selected page
                    var differentNewRoleIds = newRoleIds.Except(existedRoleIdsInExistedPagePermissions).ToList();
                    if (differentNewRoleIds.Any())
                    {
                        var newPagePermissions = pagePermissionEntries.Where(x => differentNewRoleIds.Contains(x.RoleId));
                        //Create new page permissions
                        foreach (var pagePermission in newPagePermissions)
                        {
                            InsertPagePermission(pagePermission);
                        }
                    }

                    //Remove the unused roles in page permission
                    var differentUnusedRoleIds = existedRoleIdsInExistedPagePermissions.Except(newRoleIds).ToList();
                    if (differentUnusedRoleIds.Any())
                    {
                        //Remove unused page permissions
                        var unusedPagePermissions = existedPagePermissions.Where(x => differentUnusedRoleIds.Contains(x.RoleId));
                        foreach (var pagePermission in unusedPagePermissions)
                        {
                            DeletePagePermission(pagePermission.RoleId, pagePermission.PageId);
                        }
                    }


                    //Update page permissions
                    var intersectionRoleIds = existedRoleIdsInExistedPagePermissions.Intersect(newRoleIds).ToList();
                    if (intersectionRoleIds.Any())
                    {
                        var updatePagePermissionEntries = pagePermissionEntries.Where(x => intersectionRoleIds.Contains(x.RoleId));
                        foreach (var pagePermission in updatePagePermissionEntries)
                        {
                            UpdatePagePermission(pagePermission);
                        }

                    }
                    UnitOfWork.SaveChanges();
                    transactionScope.Complete();
                }
            }
        }
        #endregion

        #region MENU PERMISSION
        //public void DeleteMenuPermission(Guid roleId, int pageId)
        //{
        //    var dataViolations = new List<RuleViolation>();
        //    var entity = UnitOfWork.PagePermissionRepository.Find(roleId, pageId);
        //    if (entity == null)
        //    {
        //        dataViolations.Add(new RuleViolation(ErrorCodeType.NotFoundPagePermission, "PagePermissionId", pageId, ErrorMessage.Messages[ErrorCodeType.NotFoundPagePermission]));
        //        throw new ValidationError(dataViolations);
        //    }
        //    else
        //    {
        //        entity.AllowAccess = false;
        //        UnitOfWork.PagePermissionRepository.Update(entity);
        //        UnitOfWork.SaveChanges();
        //    }
        //}
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
