using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class MenuPermissionLevelRepository : RepositoryBase<MenuPermissionLevel>, IMenuPermissionLevelRepository
    {
        public MenuPermissionLevelRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<MenuPermissionLevel> GetMenuPermissionLevels(MenuPermissionLevelStatus? isActive)
        {
            return (from p in DataContext.Get<MenuPermissionLevel>()
                    where isActive == null || p.IsActive == isActive
                    select p).AsEnumerable();
        }
    }
}
