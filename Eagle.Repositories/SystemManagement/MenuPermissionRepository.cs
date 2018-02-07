using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.Entities.SystemManagement.FileStorage;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class MenuPermissionRepository : RepositoryBase<MenuPermission>, IMenuPermissionRepository
    {
        public MenuPermissionRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<MenuPage> GetListByRoleId(Guid roleId)
        {
            return (from pm in DataContext.Get<MenuPermission>()
                    join m in DataContext.Get<Menu>() on pm.MenuId equals m.MenuId into permissionlist
                    from mp in permissionlist.DefaultIfEmpty()
                    join p in DataContext.Get<Page>() on mp.PageId equals p.PageId into pagelist
                    from page in pagelist.DefaultIfEmpty()
                    join f in DataContext.Get<DocumentFile>() on mp.IconFile equals f.FileId into filelist
                    from file in filelist.DefaultIfEmpty()
                    join fo in DataContext.Get<DocumentFolder>() on file.FolderId equals fo.FolderId into folderlist
                    from folder in folderlist.DefaultIfEmpty()
                    where pm.RoleId == roleId
                    select new MenuPage
                    {
                        MenuId = mp.MenuId,
                        MenuCode = mp.MenuCode,
                        TypeId = mp.TypeId,
                        PositionId = mp.PositionId,
                        ParentId = mp.ParentId,
                        Depth = mp.Depth,
                        Lineage = mp.Lineage,
                        ListOrder = mp.ListOrder,
                        HasChild = mp.HasChild,
                        MenuName = mp.MenuName,
                        MenuTitle = mp.MenuTitle,
                        MenuAlias = mp.MenuAlias,
                        Description = mp.Description,
                        Target = mp.Target,
                        IconClass = mp.IconClass,
                        CssClass = mp.CssClass,
                        IsSecured = mp.IsSecured,
                        Status = mp.Status,
                        PageId = page.PageId,
                        Page = new PageInfo
                        {
                            PageTitle = page.PageTitle,
                            PageName = page.PageName,
                            PageAlias = page.PageAlias,
                            PageUrl = page.PageUrl,
                            PagePath = page.PagePath,
                            Keywords = page.Keywords,
                            IsExtenalLink = page.IsExtenalLink,
                            DisplayTitle = page.DisplayTitle
                        }
                    }).AsEnumerable();
        }
        public IEnumerable<MenuPage> GetListByRoleId(int typeId, MenuStatus? status, Guid roleId)
        {
            return (from pm in DataContext.Get<MenuPermission>()
                    join m in DataContext.Get<Menu>() on pm.MenuId equals m.MenuId into permissionlist
                    from mp in permissionlist.DefaultIfEmpty()
                    join p in DataContext.Get<Page>() on mp.PageId equals p.PageId into pagelist
                    from page in pagelist.DefaultIfEmpty()
                    join f in DataContext.Get<DocumentFile>() on mp.IconFile equals f.FileId into filelist
                    from file in filelist.DefaultIfEmpty()
                    join fo in DataContext.Get<DocumentFolder>() on file.FolderId equals fo.FolderId into folderlist
                    from folder in folderlist.DefaultIfEmpty()
                    where pm.RoleId == roleId && mp.TypeId == typeId && (status == null || mp.Status == status)
                    select new MenuPage
                    {
                        PageId = page.PageId,
                        MenuId = mp.MenuId,
                        MenuCode = mp.MenuCode,
                        TypeId = mp.TypeId,
                        PositionId = mp.PositionId,
                        ParentId = mp.ParentId,
                        Depth = mp.Depth,
                        Lineage = mp.Lineage,
                        ListOrder = mp.ListOrder,
                        HasChild = mp.HasChild,
                        MenuName = mp.MenuName,
                        MenuTitle = mp.MenuTitle,
                        MenuAlias = mp.MenuAlias,
                        Description = mp.Description,
                        Target = mp.Target,
                        IconClass = mp.IconClass,
                        CssClass = mp.CssClass,
                        IsSecured = mp.IsSecured,
                        Status = mp.Status,
                        Page = new PageInfo
                        {
                            PageTitle = page.PageTitle,
                            PageName = page.PageName,
                            PageAlias = page.PageAlias,
                            PageUrl = page.PageUrl,
                            PagePath = page.PagePath,
                            Keywords = page.Keywords,
                            IsExtenalLink = page.IsExtenalLink,
                            DisplayTitle = page.DisplayTitle
                        }
                    }).AsEnumerable();
        }
        public IEnumerable<MenuPage> GetListByUserId(int typeId, MenuStatus? status, Guid userId)
        {
            return (from pm in DataContext.Get<MenuPermission>()
                    join m in DataContext.Get<Menu>() on pm.MenuId equals m.MenuId into permissionlist
                    from mp in permissionlist.DefaultIfEmpty()
                    join p in DataContext.Get<Page>() on mp.PageId equals p.PageId into pagelist
                    from page in pagelist.DefaultIfEmpty()
                    join f in DataContext.Get<DocumentFile>() on mp.IconFile equals f.FileId into filelist
                    from file in filelist.DefaultIfEmpty()
                    join fo in DataContext.Get<DocumentFolder>() on file.FolderId equals fo.FolderId into folderlist
                    from folder in folderlist.DefaultIfEmpty()
                    where mp.TypeId == typeId && pm.UserIds.Contains(userId.ToString()) && (status == null || mp.Status == status)
                    select new MenuPage
                    {
                        PageId = page.PageId,
                        MenuId = mp.MenuId,
                        MenuCode = mp.MenuCode,
                        TypeId = mp.TypeId,
                        PositionId = mp.PositionId,
                        ParentId = mp.ParentId,
                        Depth = mp.Depth,
                        Lineage = mp.Lineage,
                        ListOrder = mp.ListOrder,
                        HasChild = mp.HasChild,
                        MenuName = mp.MenuName,
                        MenuTitle = mp.MenuTitle,
                        MenuAlias = mp.MenuAlias,
                        Description = mp.Description,
                        Target = mp.Target,
                        IconClass = mp.IconClass,
                        CssClass = mp.CssClass,
                        IsSecured = mp.IsSecured,
                        Status = mp.Status,
                        Page = new PageInfo
                        {
                            PageTitle = page.PageTitle,
                            PageName = page.PageName,
                            PageAlias = page.PageAlias,
                            PageUrl = page.PageUrl,
                            PagePath = page.PagePath,
                            Keywords = page.Keywords,
                            IsExtenalLink = page.IsExtenalLink,
                            DisplayTitle = page.DisplayTitle
                        }
                    }).AsEnumerable();

        }
        public IEnumerable<MenuPermission> GetListByMenuId(int menuId)
        {
            return (from p in DataContext.Get<MenuPermission>()
                    where p.MenuId == menuId
                    select p).AsEnumerable();
        }

        public IEnumerable<MenuPermissionInfo> GetListByMenuId(int menuId, Guid roleId)
        {
            return (from p in DataContext.Get<MenuPermission>()
                    join l in DataContext.Get<MenuPermissionLevel>() on p.PermissionId equals l.PermissionId into permissionlist
                    from pl in permissionlist.DefaultIfEmpty()
                    join r in DataContext.Get<Role>() on p.RoleId equals r.RoleId into roleList
                    from pr in roleList.DefaultIfEmpty()
                    where p.MenuId == menuId && p.RoleId == roleId
                    select new MenuPermissionInfo
                    {
                        RoleId = p.RoleId,
                        MenuId = p.MenuId,
                        PermissionId = p.PermissionId,
                        UserIds = p.UserIds,
                        AllowAccess = p.AllowAccess,
                        MenuPermissionLevel = new MenuPermissionLevelInfo
                        {
                            PermissionId = pl.PermissionId,
                            PermissionName = pl.PermissionName,
                            Description = pl.Description,
                            DisplayOrder = pl.DisplayOrder,
                            IsActive = pl.IsActive
                        },
                        Role = new RoleInfo
                        {
                            RoleId = pr.RoleId,
                            RoleName = pr.RoleName,
                            LoweredRoleName = pr.LoweredRoleName,
                            Description = pr.Description,
                            IsActive = pr.IsActive,
                            ApplicationId = pr.ApplicationId
                        }
                    }).AsEnumerable();
        }

        public MenuPermission GetDetails(int menuId, int levelId, Guid roleId)
        {
            return (from p in DataContext.Get<MenuPermission>()
                    where p.MenuId == menuId && p.PermissionId == levelId && p.RoleId == roleId
                    select p).FirstOrDefault();
        }

        public bool HasDataExisted(int menuId, Guid roleId)
        {
            var result = DataContext.Get<MenuPermission>().FirstOrDefault(x => x.MenuId == menuId && x.RoleId == roleId);
            return (result != null);
        }

        public bool IsMenuAllowedAccessByRoles(int menuId, List<Guid> roles)
        {
            if (!roles.Any()) return false;
            return roles.Select(roleId => (from x in DataContext.Get<MenuPermission>() where x.MenuId == menuId && x.RoleId == roleId select x).FirstOrDefault()).Any();
        }

        public bool IsMenuAllowedAccessByUserId(int menuId, Guid userId)
        {
            return (from x in DataContext.Get<MenuPermission>()
                    where x.MenuId == menuId && x.UserIds != null && x.UserIds.Contains(userId.ToString()) && x.AllowAccess == true
                    select x).FirstOrDefault() != null;
        }

        public void DeleteMenuPermissions(IEnumerable<MenuPermission> permissionLst)
        {
            if (permissionLst != null)
            {
                foreach (var permission in permissionLst)
                {
                    var item = DataContext.Get<MenuPermission>().FirstOrDefault(x => x.MenuId == permission.MenuId && x.RoleId == permission.RoleId);
                    if (item != null)
                    {
                        DataContext.Delete(item);
                    }
                }
            }
        }
    }
}
