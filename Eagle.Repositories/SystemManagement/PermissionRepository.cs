using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        public PermissionRepository(IDataContext dataContext) : base(dataContext)
        {
        }
        public IEnumerable<Permission> GetPermissions(bool? status)
        {
            return (from x in DataContext.Get<Permission>()
                    where status == null || x.IsActive == status
                    select x).OrderBy(x=>x.DisplayOrder).AsEnumerable();
        }
        public bool HasDataExisted(string permissionName)
        {
            var query = DataContext.Get<Permission>().FirstOrDefault(p => p.PermissionName.ToLower().Contains(permissionName.ToLower()));
            return (query != null);
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<Permission>() select u.DisplayOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
    }
}
