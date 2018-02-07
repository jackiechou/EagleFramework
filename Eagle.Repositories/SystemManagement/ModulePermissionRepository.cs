using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class ModulePermissionRepository : RepositoryBase<ModulePermission>, IModulePermissionRepository
    {
        public ModulePermissionRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ModulePermission> GetModulePermissionsByRoleId(Guid roleId)
        {
            return (from p in DataContext.Get<ModulePermission>()
                    where p.RoleId == roleId
                    select p).AsEnumerable();
        }
        public IEnumerable<ModulePermissionInfo> GetModulePermissionsByRoleId(Guid roleId, int moduleId)
        {
            return (from p in DataContext.Get<ModulePermission>()
                    join r in DataContext.Get<Role>() on p.RoleId equals r.RoleId into roleLst
                    from pr in roleLst.DefaultIfEmpty()
                    join c in DataContext.Get<ModuleCapability>() on p.CapabilityId equals c.CapabilityId into capabilityLst
                    from pc in capabilityLst.DefaultIfEmpty()
                    where p.ModuleId == moduleId && p.RoleId == roleId
                    select new ModulePermissionInfo
                    {
                        RoleId = p.RoleId,
                        ModuleId = p.ModuleId,
                        CapabilityId = p.CapabilityId,
                        CapabilityCode = p.CapabilityCode,
                        UserIds = p.UserIds,
                        AllowAccess = p.AllowAccess,
                        ModuleCapability = new ModuleCapabilityInfo
                        {
                            CapabilityId = pc.CapabilityId,
                            CapabilityCode = pc.CapabilityCode,
                            CapabilityName = pc.CapabilityName,
                            Description = pc.Description,
                            DisplayOrder = pc.DisplayOrder,
                            IsActive = pc.IsActive,
                            ModuleId = pc.ModuleId
                        },
                        Role = new RoleInfo
                        {
                            RoleId = pr.RoleId,
                            RoleName = pr.RoleName,
                            LoweredRoleName = pr.LoweredRoleName,
                            Description = pr.Description,
                            IsActive = pr.IsActive,
                            //ApplicationId = pr.ApplicationId,
                            //RoleGroup = g,
                            //UserRole = usrRole
                        }
                    }).AsEnumerable();
        }

        public IEnumerable<ModulePermission> GetModulePermissions(int moduleId)
        {
            return (from p in DataContext.Get<ModulePermission>()
                    where p.ModuleId == moduleId select p).AsEnumerable();
        }
        public IEnumerable<ModulePermission> GetModulePermissions(int moduleId, int capabilityId)
        {
            return (from p in DataContext.Get<ModulePermission>()
                    where p.ModuleId == moduleId && p.CapabilityId == capabilityId
                    select p).AsEnumerable();
        }
       
        public ModulePermission GetDetails(Guid roleId, int capabilityId)
        {
            return (from p in DataContext.Get<ModulePermission>()
                    where p.RoleId == roleId && p.CapabilityId == capabilityId
                    select p).FirstOrDefault();
        }
    }
}
