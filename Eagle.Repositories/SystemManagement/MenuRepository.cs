using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.Entities.SystemManagement.FileStorage;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class MenuRepository : RepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(IDataContext dataContext) : base(dataContext) { }

        #region Store Procedure
        public IEnumerable<Menu> GetAllChildrenNodesOfSelectedNode(int? menuId, NewsCategoryStatus? status = null)
        {
            const string strCommand = @"EXEC dbo.Menu_GetAllChildrenNodesOfSelectedNode @menuId = {0}, @status = {1}";
            return DataContext.Get<Menu>(strCommand, menuId, status);
        }
        public IEnumerable<MenuPage> GetListByRoleIdUserIdStatus(Guid roleId, Guid userId, MenuStatus status)
        {
            string strCommand = @"EXEC dbo.Menu_GetListByRoleUserTypeStatus @RoleId={0},@UserId={1},@MenuTypeId = {2}, @Status = {3}";
            return DataContext.Get<MenuPage>(strCommand, roleId, userId, status).AsEnumerable();
        }
        public IEnumerable<MenuPage> GetParentNodesOfSelectedNodeByMenuCode(Guid menuCode)
        {
            string strCommand = @"EXEC dbo.Menu_GetParentNodesOfSelectedNodeByMenuCode @MenuCode={0}";
            return DataContext.Get<MenuPage>(strCommand, menuCode).AsEnumerable();
        }
        public IEnumerable<MenuPage> GetParentNodesOfSelectedNodeByMenuId(int? menuId, MenuStatus? status)
        {
            string strCommand = @"EXEC dbo.Menu_GetParentNodesOfSelectedNodeByMenuId @MenuId={0}";
            return DataContext.Get<MenuPage>(strCommand, menuId).AsEnumerable();
            //var query = from m in DataContext.Get<MenuPage>()
            //            join p in DataContext.Get<Page>() on m.PageId equals p.PageId into pageJoin
            //            from mp in pageJoin.DefaultIfEmpty()
            //            where m.ParentId == menuId && (status == null || m.Status == status)
            //            orderby m.ListOrder
            //            select new MenuPage
            //            {
            //                MenuId = m.MenuId,
            //                PageId = m.PageId,
            //                ParentId = m.ParentId,
            //                MenuName = m.MenuName,
            //                MenuTitle = m.MenuTitle,
            //                MenuAlias = m.MenuAlias,
            //                Description = m.Description,
            //                Depth = m.Depth,
            //                Lineage = m.Lineage,
            //                HasChild = m.HasChild,
            //                ListOrder = m.ListOrder,
            //                Status = m.Status,
            //                IsExtenalLink = mp.IsExtenalLink,
            //                PagePath = mp.PagePath,
            //                PageUrl = mp.PageUrl,
            //                Parents = (from c in DataContext.Get<MenuPage>()
            //                           join sp in DataContext.Get<Page>() on m.PageId equals sp.PageId into cspJoin
            //                           from csp in cspJoin.DefaultIfEmpty()
            //                           where c.MenuId == m.ParentId && (status == null || c.Status == status)
            //                           select new MenuPage
            //                           {
            //                               MenuId = c.MenuId,
            //                               ParentId = c.ParentId,
            //                               MenuName = m.MenuName,
            //                               MenuTitle = m.MenuTitle,
            //                               MenuAlias = m.MenuAlias,
            //                               Description = c.Description,
            //                               Depth = c.Depth,
            //                               Lineage = c.Lineage,
            //                               HasChild = c.HasChild,
            //                               ListOrder = c.ListOrder,
            //                               Status = c.Status,
            //                               IsExtenalLink = csp.IsExtenalLink,
            //                               PagePath = csp.PagePath,
            //                               PageUrl = csp.PageUrl,
            //                           }).ToList()
            //            };
            //return query.ToList();
        }
        public IEnumerable<MenuPage> GetParentNodesOfSelectedNodeByPagePath(string pagePath)
        {
            string strCommand = @"EXEC Cms.Menu_GetParentNodesOfSelectedNodeByPagePath @PagePath={0}";
            return DataContext.Get<MenuPage>(strCommand, pagePath).AsEnumerable();
        }
        public IEnumerable<MenuPage> GetHierarchicalTree()
        {
            string strCommand = @"EXEC dbo.Menu_GetHierarchicalTreeByTypeId @TypeId={0}";
            return DataContext.Get<MenuPage>(strCommand, null).AsEnumerable();
        }

        public int GetMenuLevel(int? menuId)
        {
            var pMenuId = new SqlParameter("MenuId", menuId);
            var oDepth = new SqlParameter("Depth", 1)
            {
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            DataContext.Execute("exec [dbo].[Menu_GetMenuLevelByMenuId] @MenuId,@Depth out", pMenuId, oDepth);
            return (int)oDepth.Value;
        }

        public int UpdateMenuOrder(int menuId, int parentId, int depth, int listOrder)
        {
            var pMenuId = new SqlParameter("MenuId", menuId);
            var pParentId = new SqlParameter("ParentId", parentId);
            var pDepth = new SqlParameter("Depth", depth);
            var pListOrder = new SqlParameter("ListOrder", listOrder);

            var oResult = new SqlParameter("o_return", 1)
            {
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            DataContext.Execute("exec [dbo].[Menu_UpdateOrderList] @MenuId,@Depth out", pMenuId, pParentId, pDepth, pListOrder, oResult);
            return (int)oResult.Value;
        }

        #endregion

        public IEnumerable<MenuPage> GetListByPosition(Guid applicationId, int positionId, List<Guid> roleIds, MenuStatus? status)
        {
            var menuPages = new List<MenuPage>();
            if (!roleIds.Any()) return menuPages;

            var positionEntity =
                (from p in DataContext.Get<MenuPosition>() where p.PositionId == positionId select p).FirstOrDefault();
            if (positionEntity == null) return menuPages;

            List<int> menuIds = new List<int>();
            foreach (var id in roleIds.Select(roleId => (from m in DataContext.Get<Menu>()
                                                         join pe in DataContext.Get<MenuPermission>() on m.MenuId equals pe.MenuId into permissionlist
                                                         from mp in permissionlist.DefaultIfEmpty()
                                                         orderby m.ListOrder ascending
                                                         where m.ApplicationId == applicationId
                                                               && m.PositionId.Contains(positionId.ToString())
                                                               && mp.RoleId == roleId
                                                               && (status == null || m.Status == status)
                                                               && mp.PermissionId == 1
                                                         select m.MenuId).ToList()).SelectMany(ids => ids.Where(id => !menuIds.Contains(id))))
            {
                menuIds.Add(id);
            }

            if (!menuIds.Any()) return menuPages;
            foreach (var menuItem in menuIds.Select(menuId => (from m in DataContext.Get<Menu>()
                                                               where m.MenuId == menuId
                                                               select new MenuPage
                                                               {
                                                                   TypeId = m.TypeId,
                                                                   MenuId = m.MenuId,
                                                                   MenuCode = m.MenuCode,
                                                                   PositionId = m.PositionId,
                                                                   ParentId = m.ParentId,
                                                                   Depth = m.Depth,
                                                                   ListOrder = m.ListOrder,
                                                                   HasChild = m.HasChild,
                                                                   MenuName = m.MenuName,
                                                                   MenuTitle = m.MenuTitle,
                                                                   MenuAlias = m.MenuAlias,
                                                                   Description = m.Description,
                                                                   Target = m.Target,
                                                                   IconClass = m.IconClass ?? "glyphicon-picture",
                                                                   CssClass = m.CssClass,
                                                                   Status = m.Status,
                                                                   PageId = m.PageId,
                                                                   ApplicationId = m.ApplicationId
                                                               }).FirstOrDefault()))
            {
                if (menuItem?.PageId != null)
                {
                    menuItem.Page = (from p in DataContext.Get<Page>()
                                     where p.PageId == menuItem.PageId
                                     select new PageInfo
                                     {
                                         PageId = p.PageId,
                                         PageTitle = p.PageTitle,
                                         PageName = p.PageName,
                                         PageAlias = p.PageAlias,
                                         PageUrl = p.PageUrl,
                                         PagePath = p.PagePath,
                                         ListOrder = p.ListOrder,
                                         IconClass = p.IconClass,
                                         Description = p.Description,
                                         Keywords = p.Keywords,
                                         PageHeadText = p.PageHeadText,
                                         PageFooterText = p.PageFooterText,
                                         StartDate = p.StartDate,
                                         EndDate = p.EndDate,
                                         DisableLink = p.DisableLink,
                                         DisplayTitle = p.DisplayTitle,
                                         IsExtenalLink = p.IsExtenalLink ?? false,
                                         IsSecured = p.IsSecured ?? false,
                                         IsMenu = p.IsMenu ?? false,
                                         IsActive = p.IsActive,
                                         ApplicationId = p.ApplicationId,
                                         TemplateId = p.TemplateId,
                                         LanguageCode = p.LanguageCode
                                     }).FirstOrDefault();
                }
                menuPages.Add(menuItem);
            }
            return menuPages;
        }

        public IEnumerable<MenuPage> GetListByStatus(Guid applicationId, int? menuTypeId, int positionId, MenuStatus? status)
        {
            var query = from m in DataContext.Get<Menu>()
                        join p in DataContext.Get<Page>() on m.PageId equals p.PageId into pagelist
                        from page in pagelist.DefaultIfEmpty()
                        where m.ApplicationId == applicationId 
                        && (menuTypeId == null || m.TypeId == menuTypeId)
                        && m.PositionId.Contains(positionId.ToString())
                        && (status == null || m.Status == status)
                        orderby m.ListOrder
                        select new MenuPage
                        {
                            MenuId = m.MenuId,
                            MenuCode = m.MenuCode,
                            TypeId = m.TypeId,
                            PositionId = m.PositionId,
                            ParentId = m.ParentId,
                            Depth = m.Depth,
                            ListOrder = m.ListOrder,
                            HasChild = m.HasChild,
                            MenuName = m.MenuName,
                            MenuTitle = m.MenuTitle,
                            MenuAlias = m.MenuAlias,
                            Description = m.Description,
                            Target = m.Target,
                            IconClass = m.IconClass,
                            CssClass = m.CssClass,
                            Status = m.Status,
                            PageId = page.PageId,
                            ApplicationId = m.ApplicationId,
                            TemplateId = page.TemplateId,
                            Page = new PageInfo
                            {
                                PageId = page.PageId,
                                PageTitle = page.PageTitle,
                                PageName = page.PageName,
                                PageAlias = page.PageAlias,
                                PageUrl = page.PageUrl,
                                PagePath = page.PagePath,
                                ListOrder = page.ListOrder,
                                IconClass = page.IconClass,
                                Description = page.Description,
                                Keywords = page.Keywords,
                                PageHeadText = page.PageHeadText,
                                PageFooterText = page.PageFooterText,
                                StartDate = page.StartDate,
                                EndDate = page.EndDate,
                                DisableLink = page.DisableLink,
                                DisplayTitle = page.DisplayTitle,
                                IsExtenalLink = page.IsExtenalLink ?? false,
                                IsSecured = page.IsSecured ?? false,
                                IsMenu = page.IsMenu ?? false,
                                IsActive = page.IsActive,
                                ApplicationId = page.ApplicationId,
                                TemplateId = page.TemplateId,
                                LanguageCode = page.LanguageCode
                            }
                        };
            return query.AsEnumerable();
        }
        public IEnumerable<MenuPage> GetListByRoleIdStatus(Guid applicationId, Guid roleId, bool? isSecured, MenuStatus? status)
        {
            var query = from m in DataContext.Get<Menu>()
                            //join pe in DataContext.Get<MenuPermission>() on m.MenuId equals pe.MenuId into permissionlist
                            //from mp in permissionlist.DefaultIfEmpty()
                        join p in DataContext.Get<Page>() on m.PageId equals p.PageId into pagelist
                        from page in pagelist.DefaultIfEmpty()
                            //join f in DataContext.Get<DocumentFile>() on m.IconFile equals f.FileId into filelist
                            //from file in filelist.DefaultIfEmpty()
                            //join fo in DataContext.Get<DocumentFolder>() on file.FolderId equals fo.FolderId into folderlist
                            //from folder in folderlist.DefaultIfEmpty()
                        where m.ApplicationId == applicationId
                        && (isSecured == null || m.IsSecured == isSecured)
                        && (status == null || m.Status == status)
                        //&& mp.RoleId == roleId
                        orderby m.ListOrder
                        select new MenuPage
                        {
                            TypeId = m.TypeId,
                            MenuId = m.MenuId,
                            MenuCode = m.MenuCode,
                            PositionId = m.PositionId,
                            ParentId = m.ParentId,
                            Depth = m.Depth,
                            ListOrder = m.ListOrder,
                            HasChild = m.HasChild,
                            MenuName = m.MenuName,
                            MenuTitle = m.MenuTitle,
                            MenuAlias = m.MenuAlias,
                            Description = m.Description,
                            Target = m.Target,
                            IconClass = m.IconClass ?? "glyphicon-picture",
                            CssClass = m.CssClass,
                            Status = m.Status,
                            PageId = page.PageId,
                            ApplicationId = m.ApplicationId,
                            TemplateId = page.TemplateId,
                            Page = new PageInfo
                            {
                                PageTitle = page.PageTitle,
                                PageName = page.PageName,
                                PageAlias = page.PageAlias,
                                PageUrl = page.PageUrl,
                                PagePath = page.PagePath,
                                ListOrder = page.ListOrder,
                                IconClass = page.IconClass,
                                Description = page.Description,
                                Keywords = page.Keywords,
                                PageHeadText = page.PageHeadText,
                                PageFooterText = page.PageFooterText,
                                StartDate = page.StartDate,
                                EndDate = page.EndDate,
                                DisableLink = page.DisableLink,
                                DisplayTitle = page.DisplayTitle,
                                IsExtenalLink = page.IsExtenalLink ?? false,
                                IsSecured = page.IsSecured ?? false,
                                IsMenu = page.IsMenu ?? false,
                                IsActive = page.IsActive,
                                ApplicationId = page.ApplicationId,
                                TemplateId = page.TemplateId,
                                LanguageCode = page.LanguageCode
                            }
                        };
            return query.ToList();
        }
        public IEnumerable<MenuPage> GetListByRoles(Guid applicationId, int typeId, List<Guid> roleIds, MenuStatus? status)
        {
            var menuPages = new List<MenuPage>();
            if (!roleIds.Any()) return menuPages;

            List<int> menuIds = new List<int>();
            foreach (var id in roleIds.Select(roleId => (from m in DataContext.Get<Menu>()
                                                         join pe in DataContext.Get<MenuPermission>() on m.MenuId equals pe.MenuId into permissionlist
                                                         from mp in permissionlist.DefaultIfEmpty()
                                                         orderby m.ListOrder ascending
                                                         where m.ApplicationId == applicationId
                                                               && m.TypeId == typeId
                                                               && mp.RoleId == roleId
                                                               && (status == null || m.Status == status)
                                                               && mp.PermissionId == 1
                                                         select m.MenuId).ToList()).SelectMany(ids => ids.Where(id => !menuIds.Contains(id))))
            {
                menuIds.Add(id);
            }

            if (!menuIds.Any()) return menuPages;
            foreach (var menuItem in menuIds.Select(menuId => (from m in DataContext.Get<Menu>()
                                                               where m.MenuId == menuId
                                                               select new MenuPage
                                                               {
                                                                   TypeId = m.TypeId,
                                                                   MenuId = m.MenuId,
                                                                   MenuCode = m.MenuCode,
                                                                   PositionId = m.PositionId,
                                                                   ParentId = m.ParentId,
                                                                   Depth = m.Depth,
                                                                   ListOrder = m.ListOrder,
                                                                   HasChild = m.HasChild,
                                                                   MenuName = m.MenuName,
                                                                   MenuTitle = m.MenuTitle,
                                                                   MenuAlias = m.MenuAlias,
                                                                   Description = m.Description,
                                                                   Target = m.Target,
                                                                   IconClass = m.IconClass ?? "glyphicon-picture",
                                                                   CssClass = m.CssClass,
                                                                   Status = m.Status,
                                                                   PageId = m.PageId,
                                                                   ApplicationId = m.ApplicationId
                                                               }).FirstOrDefault()))
            {
                if (menuItem?.PageId != null)
                {
                    menuItem.Page = (from p in DataContext.Get<Page>()
                                     where p.PageId == menuItem.PageId
                                     select new PageInfo
                                     {
                                         PageTitle = p.PageTitle,
                                         PageName = p.PageName,
                                         PageAlias = p.PageAlias,
                                         PageUrl = p.PageUrl,
                                         PagePath = p.PagePath,
                                         ListOrder = p.ListOrder,
                                         IconClass = p.IconClass,
                                         Description = p.Description,
                                         Keywords = p.Keywords,
                                         PageHeadText = p.PageHeadText,
                                         PageFooterText = p.PageFooterText,
                                         StartDate = p.StartDate,
                                         EndDate = p.EndDate,
                                         DisableLink = p.DisableLink,
                                         DisplayTitle = p.DisplayTitle,
                                         IsExtenalLink = p.IsExtenalLink ?? false,
                                         IsSecured = p.IsSecured ?? false,
                                         IsMenu = p.IsMenu ?? false,
                                         IsActive = p.IsActive,
                                         ApplicationId = p.ApplicationId,
                                         TemplateId = p.TemplateId,
                                         LanguageCode = p.LanguageCode
                                     }).FirstOrDefault();
                }
                menuPages.Add(menuItem);
            }
            return menuPages;
        }
        public IEnumerable<MenuPage> GetPublishedList(Guid applicationId, int? menuTypeId, string languageCode)
        {
            var query = (from m in DataContext.Get<Menu>()
                         join p in DataContext.Get<Page>() on m.PageId equals p.PageId into pageLst
                         from page in pageLst.DefaultIfEmpty()
                         where m.ApplicationId == applicationId && (menuTypeId == null || m.TypeId == menuTypeId) && m.Status == MenuStatus.Published
                         orderby m.ListOrder
                         select new MenuPage
                         {
                             TypeId = m.TypeId,
                             PageId = m.PageId,
                             MenuId = m.MenuId,
                             ParentId = m.ParentId,
                             Depth = m.Depth,
                             ListOrder = m.ListOrder,
                             MenuName = m.MenuName,
                             MenuTitle = m.MenuTitle,
                             MenuAlias = m.MenuAlias,
                             Target = m.Target,
                             IconClass = m.IconClass ?? "glyphicon-picture",
                             CssClass = m.CssClass,
                             Description = m.Description,
                             Status = m.Status,
                             Page = new PageInfo
                             {
                                 PageTitle = page.PageTitle,
                                 PageName = page.PageName,
                                 PageAlias = page.PageAlias,
                                 PageUrl = page.PageUrl,
                                 PagePath = page.PagePath,
                                 ListOrder = page.ListOrder,
                                 IconClass = page.IconClass,
                                 Description = page.Description,
                                 Keywords = page.Keywords,
                                 PageHeadText = page.PageHeadText,
                                 PageFooterText = page.PageFooterText,
                                 StartDate = page.StartDate,
                                 EndDate = page.EndDate,
                                 DisableLink = page.DisableLink,
                                 DisplayTitle = page.DisplayTitle,
                                 IsExtenalLink = page.IsExtenalLink ?? false,
                                 IsSecured = page.IsSecured ?? false,
                                 IsMenu = page.IsMenu ?? false,
                                 IsActive = page.IsActive,
                                 ApplicationId = page.ApplicationId,
                                 TemplateId = page.TemplateId,
                                 LanguageCode = page.LanguageCode
                             }
                         }).ToList();
            return query;
        }
        public IEnumerable<MenuPage> Search(string strSearch)
        {
            return DataContext.Get<Menu>()
             .Where(p => p.MenuName.Contains(strSearch))
             .OrderBy(p => p.ListOrder)
             .Select(p => new MenuPage() { MenuTitle = p.MenuTitle, MenuId = p.MenuId, IconClass = p.IconClass, ListOrder = p.ListOrder });

        }
        public List<int> GetListByParentId(int parentId)
        {
            return (from x in DataContext.Get<Menu>()
                    where x.ParentId == parentId
                    select x.MenuId).ToList();
        }
        public IEnumerable<MenuTree> GetList()
        {
            return
                DataContext.Get<Menu>()
                    .OrderBy(m => m.ListOrder)
                    .Select(m => new MenuTree()
                    {
                        Id = m.MenuId,
                        Key = m.MenuId,
                        ParentId = m.ParentId,
                        Name = m.MenuName,
                        Text = m.MenuTitle,
                        Title = m.MenuTitle,
                        Tooltip = m.Description,
                        IsParent = m.HasChild,
                        Open = (m.ParentId == 0)
                    }).AsEnumerable();
        }
        public IEnumerable<MenuTree> GetTreeList(int typeId, MenuStatus? status = null)
        {
            var list = (from m in DataContext.Get<Menu>()
                        join p in DataContext.Get<Page>() on m.PageId equals p.PageId into pagelist
                        from page in pagelist.DefaultIfEmpty()
                        join f in DataContext.Get<DocumentFile>() on m.IconFile equals f.FileId into filelist
                        from file in filelist.DefaultIfEmpty()
                        join folder in DataContext.Get<DocumentFolder>() on file.FolderId equals folder.FolderId into folderlist
                        from fo in folderlist.DefaultIfEmpty()
                        where m.TypeId == typeId && (status == null || m.Status == status)
                        orderby m.ListOrder
                        select new MenuTree
                        {
                            Id = m.MenuId,
                            Key = m.MenuId,
                            ParentId = m.ParentId,
                            Name = m.MenuName,
                            Text = m.MenuTitle,
                            Title = m.MenuTitle,
                            Tooltip = m.Description,
                            Url = page.PageUrl,
                            IsParent = m.HasChild,
                            Open = (m.ParentId == 0)
                        }).ToList();
            var recursiveObjects = RecursiveFillTree(list, 0);

            return recursiveObjects;
        }
        public IEnumerable<MenuTreeNode> GetHierachicalList(int typeId, MenuStatus? status = null, bool? isRootShowed = false)
        {
            var list = (from m in DataContext.Get<Menu>()
                        join p in DataContext.Get<Page>() on m.PageId equals p.PageId into pagelist
                        from page in pagelist.DefaultIfEmpty()
                        where m.TypeId == typeId && (status == null || m.Status == status)
                        orderby m.ListOrder
                        select new MenuTreeNode
                        {
                            id = m.MenuId,
                            key = m.MenuId,
                            parentId = m.ParentId,
                            name = m.MenuName,
                            text = m.MenuTitle,
                            title = m.MenuTitle,
                            tooltip = m.Description,
                            url = page.PageUrl,
                            isParent = m.HasChild,
                            open = (m.ParentId == 0 || m.ParentId == null)
                        }).ToList();

            var recursiveObjects = RecursiveFillMenuTreeNode(list, 0);

            if (isRootShowed != null && isRootShowed == true)
                recursiveObjects.Insert(0, new MenuTreeNode()
                {
                    id = 0,
                    key = 0,
                    parentId = 0,
                    name = " --- " + LanguageResource.Root + " --- ",
                    text = " --- " + LanguageResource.Root + " --- ",
                    title = " --- " + LanguageResource.Root + " --- ",
                    tooltip = " --- " + LanguageResource.Root + " --- ",
                    isParent = true,
                    open = true
                });

            return recursiveObjects;
        }
        private List<MenuTreeNode> RecursiveFillMenuTreeNode(List<MenuTreeNode> list, int? id)
        {
            List<MenuTreeNode> items = new List<MenuTreeNode>();
            List<MenuTreeNode> nodes = list.Where(m => m.parentId == id).Select(
               m => new MenuTreeNode
               {
                   id = m.id,
                   key = m.key,
                   parentId = m.parentId,
                   name = m.name,
                   text = m.text,
                   title = m.title,
                   tooltip = m.tooltip,
                   url = m.url,
                   isParent = m.isParent,
                   open = m.open
               }).ToList();

            if (nodes.Count > 0)
            {
                items.AddRange(nodes.Select(child => new MenuTreeNode()
                {
                    id = child.id,
                    key = child.key,
                    parentId = child.parentId,
                    name = child.name,
                    text = child.text,
                    title = child.title,
                    tooltip = child.tooltip,
                    url = child.url,
                    isParent = child.isParent,
                    open = child.open,
                    children = RecursiveFillMenuTreeNode(list, child.id)
                }));
            }
            return items;
        }

        public IEnumerable<Menu> GetMenuListByStatus(MenuStatus? menuStatus)
        {
            var menuList = (from x in DataContext.Get<Menu>()
                            where (menuStatus == null || x.Status == menuStatus)
                            orderby x.ListOrder ascending
                            select x).ToList();
            return menuList;
        }
        public IEnumerable<MenuPage> GetListByParentIdStatus(int parentId, MenuStatus? status)
        {
            var query = from m in DataContext.Get<Menu>()
                        join p in DataContext.Get<Page>() on m.PageId equals p.PageId into pagelist
                        from page in pagelist.DefaultIfEmpty()
                        where m.ParentId == parentId
                        && (status == null || m.Status == status)
                        orderby m.ListOrder
                        select new MenuPage
                        {
                            MenuId = m.MenuId,
                            MenuCode = m.MenuCode,
                            TypeId = m.TypeId,
                            PositionId = m.PositionId,
                            ParentId = m.ParentId,
                            Depth = m.Depth,
                            ListOrder = m.ListOrder,
                            HasChild = m.HasChild,
                            MenuName = m.MenuName,
                            MenuTitle = m.MenuTitle,
                            MenuAlias = m.MenuAlias,
                            Description = m.Description,
                            Target = m.Target,
                            IconClass = m.IconClass,
                            CssClass = m.CssClass,
                            Status = m.Status,
                            PageId = page.PageId,
                            ApplicationId = m.ApplicationId,
                            TemplateId = page.TemplateId,
                            Page = new PageInfo
                            {
                                PageId = page.PageId,
                                PageTitle = page.PageTitle,
                                PageName = page.PageName,
                                PageAlias = page.PageAlias,
                                PageUrl = page.PageUrl,
                                PagePath = page.PagePath,
                                ListOrder = page.ListOrder,
                                IconClass = page.IconClass,
                                Description = page.Description,
                                Keywords = page.Keywords,
                                PageHeadText = page.PageHeadText,
                                PageFooterText = page.PageFooterText,
                                StartDate = page.StartDate,
                                EndDate = page.EndDate,
                                DisableLink = page.DisableLink,
                                DisplayTitle = page.DisplayTitle,
                                IsExtenalLink = page.IsExtenalLink ?? false,
                                IsSecured = page.IsSecured ?? false,
                                IsMenu = page.IsMenu ?? false,
                                IsActive = page.IsActive,
                                ApplicationId = page.ApplicationId,
                                TemplateId = page.TemplateId,
                                LanguageCode = page.LanguageCode
                            }
                        };
            return query.AsEnumerable();
        }

        public List<Menu> GetChildren(int menuId, MenuStatus? status = null)
        {
            var query = from x in DataContext.Get<Menu>()
                        orderby x.ListOrder ascending
                        where x.ParentId == menuId && (status == null && x.Status == status)
                        select x;
            return query.ToList();
        }
      

        public int GetMenuDepth(int? menuId)
        {
            return (from x in DataContext.Get<Menu>() where x.MenuId == menuId select x.Depth).FirstOrDefault();
        }

        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<Menu>() select u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }

        public MenuPage GetDetails(int menuId)
        {
            var query = (from m in DataContext.Get<Menu>()
                         join p in DataContext.Get<Page>() on m.PageId equals p.PageId into pageLst
                         from page in pageLst.DefaultIfEmpty()
                         where m.MenuId == menuId
                         orderby m.ListOrder
                         select new MenuPage
                         {
                             PageId = m.PageId,
                             MenuId = m.MenuId,
                             ParentId = m.ParentId,
                             Depth = m.Depth,
                             ListOrder = m.ListOrder,
                             MenuName = m.MenuName,
                             MenuTitle = m.MenuTitle,
                             MenuAlias = m.MenuAlias,
                             Target = m.Target,
                             IconClass = m.IconClass,
                             CssClass = m.CssClass,
                             Description = m.Description,
                             Status = m.Status,
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
                         }).FirstOrDefault();
            return query;
        }

        public MenuPage GetDetailsByMenuCode(Guid menuCode)
        {
            var query = (from m in DataContext.Get<Menu>()
                         join p in DataContext.Get<Page>() on m.PageId equals p.PageId into pageLst
                         from page in pageLst.DefaultIfEmpty()
                         where m.MenuCode == menuCode
                         orderby m.ListOrder
                         select new MenuPage
                         {
                             PageId = m.PageId,
                             MenuId = m.MenuId,
                             ParentId = m.ParentId,
                             Depth = m.Depth,
                             ListOrder = m.ListOrder,
                             MenuName = m.MenuName,
                             MenuTitle = m.MenuTitle,
                             MenuAlias = m.MenuAlias,
                             Target = m.Target,
                             IconClass = m.IconClass,
                             CssClass = m.CssClass,
                             Description = m.Description,
                             Status = m.Status,
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
                         }).FirstOrDefault();
            return query;
        }

        //public JsonResult UpdateListOrder(string Ids)
        //{
        //    string message = string.Empty;
        //    bool flag = false;
        //    List<int> lst = Ids.Split(',').Select(s => int.Parse(s)).ToList();
        //    if (lst.Count() > 0)
        //    {
        //        int id = 0;
        //        for (int i = 0; i < lst.Count(); i++)
        //        {
        //            id = lst[i];
        //            flag = PageRepository.UpdateListOrder(id, i + 1, out message);
        //        }
        //    }
        //    return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        //}

        public static string ConvertListToXmlFormat(List<Menu> menuList)
        {
            string strResult = string.Empty;
            if (menuList.Count > 0)
            {
                var elements = new XElement("Menus", from x in menuList
                                                     select new XElement("Menu",
                                                             new XElement("MenuId", x.MenuId),
                                                             new XElement("ParentId", x.ParentId),
                                                             new XElement("Depth", x.Depth),
                                                             new XElement("SortKey", x.ListOrder),
                                                             new XElement("Name", x.MenuName),
                                                             new XElement("Alias", x.MenuAlias),
                                                             new XElement("PageId", x.PageId),
                                                             new XElement("Target", x.Target),
                                                             new XElement("IconFile", x.IconFile),
                                                             new XElement("IconClass", x.IconClass),
                                                             new XElement("CssClass", x.CssClass),
                                                             new XElement("Description", x.Description),
                                                             new XElement("Status", x.Status)
                                                     )
                                            );
                XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), elements);

                strResult = document.ToString();
            }
            return strResult;
        }

        private List<MenuTree> RecursiveFillTree(List<MenuTree> list, int? id)
        {
            List<MenuTree> items = new List<MenuTree>();
            List<MenuTree> nodes = list.Where(m => m.ParentId == id).Select(
               m => new MenuTree
               {
                   Id = m.Id,
                   Key = m.Key,
                   ParentId = m.ParentId,
                   Name = m.Name,
                   Text = m.Text,
                   Title = m.Title,
                   Tooltip = m.Tooltip,
                   Url = m.Url,
                   IsParent = m.IsParent,
                   Open = m.Open
               }).ToList();

            if (nodes.Count > 0)
            {
                items.AddRange(nodes.Select(child => new MenuTree()
                {
                    Id = child.Id,
                    Key = child.Key,
                    ParentId = child.ParentId,
                    Name = child.Name,
                    Text = child.Text,
                    Title = child.Title,
                    Tooltip = child.Tooltip,
                    Url = child.Url,
                    IsParent = child.IsParent,
                    Open = child.Open,
                    Children = RecursiveFillTree(list, child.Id)
                }));
            }
            return items;
        }

        public bool HasChildren(int menuId)
        {
            var query = from x in DataContext.Get<Menu>()
                        orderby x.ListOrder ascending
                        where x.ParentId == menuId
                        select x;
            return query.Any();
        }
        public SelectList PopulateIsSecuredSelectList(int? selectedValue, bool? isShowSelectText = false)
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Admin, Value = "1", Selected = true},
                new SelectListItem {Text = LanguageResource.Desktop, Value = "0"}
            };

            if (isShowSelectText != null && isShowSelectText == true)
                list.Insert(0, new SelectListItem() { Value = "-1", Text = $"-- {LanguageResource.Select} --" });

            return new SelectList(list, "Value", "Text", selectedValue);
        }
    }
}
