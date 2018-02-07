using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IMenuPermissionLevelRepository : IRepositoryBase<MenuPermissionLevel>
    {
        IEnumerable<MenuPermissionLevel> GetMenuPermissionLevels(MenuPermissionLevelStatus? isActive);
    }
}
