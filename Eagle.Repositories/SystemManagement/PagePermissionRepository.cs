using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Permission;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class PagePermissionRepository: RepositoryBase<PagePermission>, IPagePermissionRepository
    {
        public PagePermissionRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<Permission> GetPagePermissionLevels()
        {
            return (from PagePermissionLevel x in Enum.GetValues(typeof(PagePermissionLevel)).Cast<PagePermissionLevel>()
                    select new Permission
                    {
                        PermissionName = x.ToString(),
                        PermissionId = (int)x
                    }).AsEnumerable();
        }

        public IEnumerable<PagePermission> GetPagePermissions(bool? allowAccess)
        {
            return (from pm in DataContext.Get<PagePermission>()
                    where allowAccess==null || pm.AllowAccess == allowAccess
                    select pm).AsEnumerable();
        }

        public IEnumerable<PagePermission> GetPagePermissionsByRoleId(Guid roleId, bool? allowAccess = null)
        {
            return (from pm in DataContext.Get<PagePermission>()
                where pm.RoleId == roleId && (allowAccess == null || pm.AllowAccess == allowAccess)
                    select pm).AsEnumerable();
        }

        public IEnumerable<PagePermission> GetPagePermissionsByPageId(int pageId)
        {
            return (from pm in DataContext.Get<PagePermission>()
                    where pm.PageId == pageId
                    select pm).AsEnumerable();
        }
        public IEnumerable<PagePermission> GetDetails(Guid roleId, int pageId)
        {
            return (from pm in DataContext.Get<PagePermission>()
                    where pm.RoleId == roleId && pm.PageId == pageId
                    select pm).AsEnumerable();
        }
        public bool HasDataExisted(Guid roleId, int pageId)
        {
            var query = DataContext.Get<PagePermission>().FirstOrDefault(p => p.RoleId == roleId && p.PageId  == pageId);
            return (query != null);
        }
    }
}
