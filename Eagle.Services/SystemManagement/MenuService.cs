using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Transactions;
using System.Web.Mvc;
using System.Xml.Linq;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.SystemManagement.Validation;
using Eagle.Services.Validations;
using MenuPermissionAccessLevel = Eagle.Services.Dtos.SystemManagement.MenuPermissionAccessLevel;


namespace Eagle.Services.SystemManagement
{
    public class MenuService : BaseService, IMenuService
    {
        public IRoleService RoleService { get; set; }
        public IDocumentService DocumentService { get; set; }
        public ICacheService CacheService { get; set; }

        public MenuService(IUnitOfWork unitOfWork, ICacheService cacheService, IDocumentService documentService, IRoleService roleService) : base(unitOfWork)
        {
            CacheService = cacheService;
            DocumentService = documentService;
            RoleService = roleService;
        }

        public SelectList GenerateMenuStatuses(int? selectedValue = null, bool? isShowSelectText = false)
        {
            var list = (from MenuStatus x in Enum.GetValues(typeof(MenuStatus)).Cast<MenuStatus>()
                        select new SelectListItem
                        {
                            Text = x.ToString(),
                            Value = ((int)x).ToString(),
                            Selected = x.Equals(selectedValue)
                        }).ToList();

            if (isShowSelectText != null && isShowSelectText == true)
                list.Insert(0, new SelectListItem() { Value = "-1", Text = $"-- {LanguageResource.Select} --" });

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public SelectList PopulateLinkTargets(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.LoadInANewWindow, Value = "_blank"},
                new SelectListItem {Text = LanguageResource.LoadInTheSameFrameAsItWasClicked, Value = "_self"},
                new SelectListItem {Text = LanguageResource.LoadInTheParentFrameset, Value = "_parent"},
                new SelectListItem {Text = LanguageResource.LoadInTheFullBodyOfTheWindow, Value = "_top"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        #region Desktop Mega Menu ====================================================================================================================
        public string LoadDesktopMegaMenu(Guid applicationId, MenuPositionSetting position)
        {
            //string menu = UnitOfWork.CacheManager.Get<string>(CacheKeySetting.MenuDesktop);
            string menu = CacheService.Get<string>(CacheKeySetting.MenuDesktop);
            if (!string.IsNullOrEmpty(menu))
            {
                return menu;
            }
            else
            {
                string strHtml = string.Empty, strResult = string.Empty;
                var menuList = UnitOfWork.MenuRepository.GetListByStatus(applicationId, (int)MenuTypeSetting.Site, (int)position, MenuStatus.Published).ToList();
                if (menuList.Any())
                {
                    var parentLst = menuList.Where(p => p.ParentId == null || p.ParentId == 0).ToList();
                    if (parentLst.Any())
                    {
                        bool first = true;
                        foreach (var item in parentLst)
                        {
                            string liStyle = string.Empty;
                            if (first)
                            {
                                liStyle = "class='current'";
                                first = false;
                            }

                            var iconClass = (!string.IsNullOrEmpty(item.IconClass)) ? "<i class='glyphicon " + item.IconClass + "'></i>" : "<i class='icon-menu'></i>";
                            string menuLink;
                            if (item.Page.IsExtenalLink != null && item.Page.IsExtenalLink == true)
                                menuLink = item.Page.PageUrl;
                            else
                            {
                                menuLink = !string.IsNullOrEmpty(item.Page.PagePath) ? item.Page.PagePath.ToLower() : GlobalSettings.DefaultDesktopPageUrl;
                            }

                            int? pageId = item.PageId ?? 1;
                            menuLink += $"/{pageId}/{item.MenuId}";

                            strResult += "<li id='" + item.MenuId + "' " + liStyle + ">";
                            strResult += "<a href='" + menuLink + "' class='menu-item'>" + iconClass + "  " + item.MenuTitle + "</a>";
                            if (item.HasChild != null && item.HasChild == true)
                            {
                                strResult += LoopMegaMenu(item.MenuId, menuList);
                            }
                            strResult += "</li>";
                        }
                    }

                    strHtml = "<ul class='sf-menu'>" + strResult + "</ul>";
                }
                CacheService.Add(CacheKeySetting.MenuDesktop, strHtml, new CacheItemPolicy
                {
                    AbsoluteExpiration = System.Web.Caching.Cache.NoAbsoluteExpiration.ToUniversalTime(),
                   // SlidingExpiration = TimeSpan.FromMinutes(20),
                    Priority = CacheItemPriority.Default
                });

                return strHtml;
            }
           
        }
        private string LoopMegaMenu(int menuId, List<MenuPage> lst)
        {
            string strResult = string.Empty;
            var list = lst.Where(p => p.ParentId == menuId).ToList();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    var icon = (!string.IsNullOrEmpty(item.IconClass)) ? "<i class='glyphicon " + item.IconClass + "'></i>" : "<i class='icon-menu'></i>";

                    string menuLink;
                    if (item.Page.IsExtenalLink != null && item.Page.IsExtenalLink == true)
                        menuLink = item.Page.PageUrl;
                    else
                    {
                        menuLink = !string.IsNullOrEmpty(item.Page.PagePath) ? item.Page.PagePath.ToLower() : GlobalSettings.DefaultDesktopPageUrl;   
                    }

                    int pageId = item.PageId ?? 1;
                    menuLink += "/" + pageId + "/" + item.MenuId;

                    strResult += "<div class='sf-mega-section'>";
                    strResult += "<h2>" + item.MenuTitle + "</h2>";
                    strResult += "<ul>";
                    strResult += "<li id='" + item.MenuId + "'>";
                    strResult += "<a href='" + menuLink + "' class='menu-item'><span> " + icon + "  " + item.MenuTitle + "</span></a>";
                    if (item.HasChild != null && item.HasChild == true)
                    {
                        strResult += LoopMegaMenu(item.MenuId, lst);
                    }
                    strResult += "</li>";
                    strResult += "</ul>";
                    strResult += "</div>";
                }
            }
            return strResult;
        }
        #endregion =================================================================================================================================

        #region Desktop Menu =============================================================================================================

        //Bootstrap Menu
        public string LoadDesktopBootstrapMenu(Guid applicationId, MenuPositionSetting position)
        {
            string menu = CacheService.Get<string>(CacheKeySetting.MenuDesktop);
            if (!string.IsNullOrEmpty(menu))
            {
                return menu;
            }
            else
            {
                string strHtml = string.Empty, strResult = string.Empty, menuLink = string.Empty;
                var menuList = UnitOfWork.MenuRepository.GetListByStatus(applicationId, (int)MenuTypeSetting.Site, (int)position, MenuStatus.Published).ToList();
                if (menuList.Any())
                {
                    bool first = true;
                    var parentList = menuList.Where(p => p.ParentId == null || p.ParentId == 0).ToList();
                    if (parentList.Any())
                    {
                        strResult += "<ul class='nav navbar-nav'>";
                        foreach (var item in parentList)
                        {
                            if (first)
                            {
                                first = false;
                            }

                            var icon = (!string.IsNullOrEmpty(item.IconClass)) ? "<i class='glyphicon " + item.IconClass + "'></i>" : "<i class='icon-menu'></i>";
                            if (item.Page.IsExtenalLink != null && item.Page.IsExtenalLink == true)
                                menuLink = item.Page.PageUrl;
                            else
                            {
                                if (!string.IsNullOrEmpty(item.Page.PagePath))
                                    menuLink = item.Page.PagePath.ToLower();
                            }
                            //int? pageId = item.PageId ?? 1;
                            menuLink += $"/{item.MenuCode}";
                            
                            if (item.HasChild != null && item.HasChild == true) { 
                                strResult += "<li id='" + item.PageId + "' class='dropdown'>";
                            }
                            else { 
                                strResult += "<li id='" + item.PageId + "' >";
                            }

                            if (item.HasChild != null && item.HasChild == true)
                                strResult += "<a href='" + menuLink + "' data-toggle='dropdown' class='dropdown-toggle menu-item'>" + icon + "  " + item.MenuTitle + "<b class='caret'></b></a>";
                            else
                                strResult += "<a href='" + menuLink + "' class='menu-item'>" + icon + "  " + item.MenuTitle + "</a>";

                            if (item.HasChild != null && item.HasChild == true)
                            {
                                strResult += LoopDesktopBootstrapMenu(item.MenuId, menuList);
                            }
                            strResult += "</li>";
                           
                        }
                        strResult += "</ul>";
                    }

                    strHtml = "<div class='navbar yamm navbar-default'><div class='container'>" + strResult + "</div></div>";
                }

                CacheService.Add(CacheKeySetting.MenuDesktop, strHtml, new CacheItemPolicy
                {
                    AbsoluteExpiration = System.Web.Caching.Cache.NoAbsoluteExpiration.ToUniversalTime(),
                   // SlidingExpiration = TimeSpan.FromMinutes(20),
                    Priority = CacheItemPriority.Default
                });

                return strHtml;
            }
        }

        private string LoopDesktopBootstrapMenu(int menuId, List<MenuPage> lst)
        {
            string strResult = string.Empty, menuLink = string.Empty;
            var list = lst.Where(p => p.ParentId == menuId).ToList();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    var icon = (!string.IsNullOrEmpty(item.IconClass)) ? "<i class='glyphicon " + item.IconClass + "'></i>" : "<i class='icon-menu-list'></i>";
                    if (item.Page.IsExtenalLink != null && item.Page.IsExtenalLink == true)
                        menuLink = item.Page.PageUrl;
                    else
                    {
                        if (!string.IsNullOrEmpty(item.Page.PagePath))
                            menuLink = item.Page.PagePath.ToLower();
                    }

                    //int? pageId = item.PageId ?? 1;
                    menuLink += "/" + item.MenuCode;

                    strResult += "<ul class='dropdown-menu'>";
                    strResult += "<li>";
                    strResult += "<div class='yamm-content'>";
                    strResult += "<div class='row'>";
                    strResult += "<ul class='col-sm-4 list-unstyled'>";
                    strResult += "<li id='" + item.MenuId + "'>";
                    strResult += "<a href='" + menuLink + "' class='menu-item'><span> " + icon + "  " + item.MenuTitle + "</span></a>";
                    if (item.HasChild != null && item.HasChild == true)
                    {
                        strResult += LoopSubDesktopBootstrapMenu(item.MenuId, lst);
                    }
                    strResult += "</li>";
                    strResult += "</ul>";
                    strResult += "</div>";
                    strResult += "</div>";
                    strResult += "</li>";
                    strResult += "</ul>";
                }
            }
            return strResult;
        }

        private static string LoopSubDesktopBootstrapMenu(int menuId, IEnumerable<MenuPage> lst)
        {
            string strResult = string.Empty, menuLink = string.Empty;
            var menuPages = lst.Where(p => p.ParentId == menuId).ToList();
            if (menuPages.Any())
            {
                foreach (var item in menuPages)
                {
                    var icon = (!string.IsNullOrEmpty(item.IconClass)) ? "<i class='glyphicon " + item.IconClass + "'></i>" : "<i class='icon-menu-list'></i>";
                    if (item.Page.IsExtenalLink != null && item.Page.IsExtenalLink == true)
                        menuLink = item.Page.PageUrl;
                    else
                    {
                        if (!string.IsNullOrEmpty(item.Page.PagePath))
                            menuLink = item.Page.PagePath.ToLower();
                    }

                    //int? pageId = item.PageId ?? 1;
                    menuLink += "/" + item.MenuCode;

                    strResult += "<ul>";
                    strResult += "<li id='" + item.MenuId + "' class='list-unstyled'>";
                    strResult += "<a href='" + menuLink + "' class='menu-item'><span> " + icon + "  " + item.MenuTitle + "</span></a>";
                    strResult += "</li>";
                    strResult += "</ul>";
                }
            }
            return strResult;
        }
        #endregion =======================================================================================================================

        #region Dekstop SiteMap
        public string PopulateDesktopSiteMapByMenuCode(string menuCode)
        {
            string strHtml = "";
            if (!string.IsNullOrEmpty(menuCode) && menuCode != "Index")
            {
                strHtml = CacheService.Get<string>(CacheKeySetting.SiteMapDesktop);
                if (string.IsNullOrEmpty(strHtml))
                {
                    var lst = UnitOfWork.MenuRepository.GetParentNodesOfSelectedNodeByMenuCode(Guid.Parse(menuCode)).ToList();
                    if (lst.Any())
                    {
                        foreach (var item in lst)
                        {
                            string menuLink;
                            string menuTitle = item.MenuTitle;
                            string pageUrl = item.Page.PageUrl;
                            bool? isExtenalLink = Convert.ToBoolean(item.Page.IsExtenalLink);
                            if (isExtenalLink == true)
                                menuLink = pageUrl;
                            else
                                menuLink = item.Page.PagePath.ToLower() + "/" + item.MenuId;

                            strHtml += "<li>";
                            strHtml += "<a href='" + menuLink + "'><span>" + menuTitle + "</span></a> <small><i class='glyphicon glyphicon-chevron-right'></i></small>";
                            strHtml += "</li>";
                        }

                        CacheService.Add(CacheKeySetting.SiteMapDesktop, strHtml, new CacheItemPolicy
                        {
                            AbsoluteExpiration = System.Web.Caching.Cache.NoAbsoluteExpiration.ToUniversalTime(),
                            Priority = CacheItemPriority.Default
                        });
                    }
                }
            }
            var result = "<ul class='breadcrumb'>";
            result += "<li><a href='/Home/Index'>";
            result += "<span><i class='glyphicon glyphiconicon-home'></i> " + LanguageResource.Home + "</span></a> <small><i class='glyphicon glyphicon-chevron-right'></i></small>";
            result += "</li>";
            result += strHtml;
            result += "</ul>";
            return result;
        }
        #endregion

        #region Menu Tree =====================================================================================================================
        public string LoadMenu(Guid applicationId, Guid userId, Guid roleId)
        {
            string signal = GetUrl().ToLower();
            signal = signal == "home" ? "" : signal;

            string menu = CacheService.Get<string>(CacheKeySetting.MenuByRole);
            if (!string.IsNullOrEmpty(menu))
            {
                return menu.Replace("/" + signal + "'", "/" + signal + "' isview='1' ");
            }
            else
            {
                string strResult = string.Empty, menuLink = string.Empty;

                var roleIds = UnitOfWork.UserRoleRepository.GetUserRolesByUserId(userId).Select(x => x.RoleId).ToList();
                var menuList = UnitOfWork.MenuRepository.GetListByRoles(applicationId, Convert.ToInt32(MenuTypeSetting.Admin), roleIds,MenuStatus.Published).ToList();

                if (menuList.Any())
                {
                    foreach (var item in menuList.Where(p => p.ParentId == null || p.ParentId == 0))
                    {
                        var icon = (!string.IsNullOrEmpty(item.IconClass)) ? "<i class='glyphicon " + item.IconClass + "'></i>" : "<i class='icon-menu'></i>";
                        if (item.Page != null)
                        {
                            if (item.Page.IsExtenalLink != null && item.Page.IsExtenalLink == true)
                                menuLink = item.Page.PageUrl;
                            else
                            {
                                if (!string.IsNullOrEmpty(item.Page.PagePath))
                                    menuLink = item.Page.PagePath.ToLower();
                            }
                        }
                        else
                        {
                            menuLink = GlobalSettings.DefaultPageUrl;
                        }
                       
                        int? pageId = item.PageId ?? 1;
                        menuLink += "/" + pageId + "/" + item.MenuId;

                        strResult += "<li id='" + item.MenuId + "'>";
                        strResult += "<a href='" + menuLink + "' class='menu-item'><span> " + icon + "  " + item.MenuTitle + "</span></a>";
                        strResult += LoopMenu(item.MenuId, menuList);
                        strResult += "</li>";
                    }
                    strResult = strResult.Replace("/" + signal + "'", "/" + signal + "' isview='1' ");

                    CacheService.Add(CacheKeySetting.MenuByRole, strResult, new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddDays(30),
                        Priority = CacheItemPriority.Default
                    });
                }
                return strResult;
            }
        }
        private static string GetUrl()
        {
            var flag = false;
            foreach (var item in System.Web.HttpContext.Current.Request.Url.AbsolutePath.Split('/'))
            {
                if (flag)
                    return item;
                if (item.ToLower().Contains("admin"))
                    flag = true;
            }
            return "";
        }
        private string LoopMenu(int menuId, List<MenuPage> lst)
        {
            string strResult = string.Empty, menuLink = string.Empty;
            var list = lst.Where(p => p.ParentId == menuId);
            if (list.Any())
            {
                strResult = "<ul style='display:none;'>";
                foreach (var item in lst.Where(p => p.ParentId == menuId))
                {
                    var icon = (!string.IsNullOrEmpty(item.IconClass)) ? "<i class='glyphicon " + item.IconClass + "'></i>" : "<i class='icon-menu-list'></i>";
                    if (item.Page != null)
                    {
                        if (item.Page.IsExtenalLink != null && item.Page.IsExtenalLink == true)
                            menuLink = item.Page.PageUrl;
                        else
                        {
                            if (!string.IsNullOrEmpty(item.Page.PagePath))
                                menuLink = item.Page.PagePath.ToLower();
                        }
                    }
                    else
                    {
                        menuLink = GlobalSettings.DefaultPageUrl;
                    }

                    int? pageId = item.PageId ?? 1;
                    menuLink += $"/{pageId}/{item.MenuId}";

                    strResult += "<li id='" + item.MenuId + "'>";
                    strResult += "<a href='" + menuLink + "' class='menu-item'><span> " + icon + "  " + item.MenuTitle + "</span></a>";
                    strResult += LoopMenu(item.MenuId, lst);
                    strResult += "</li>";
                }

                strResult += "</ul>";
            }
            return strResult;
        }

        #endregion =======================================================================================================================

        #region Site Map
        public IEnumerable<TreeDetail> GetSiteMapSelectTree(bool? status, int? selectedId,bool? isRootShowed = false)
        {
            var lst = UnitOfWork.SiteMapRepository.GetSiteMapSelectTree(status, selectedId, isRootShowed);
            return lst.ToDtos<TreeEntity, TreeDetail>();
        }
        public IEnumerable<SiteMapDetail> GetSiteMap(string controller = "home", string action = "index")
        {
            var item = UnitOfWork.SiteMapRepository.GetDetail(controller, action);
            if (item == null) return null;

            var lst = UnitOfWork.SiteMapRepository.GetAllParentNodesOfSelectedNode(item.SiteMapId, true);
            return lst.ToDtos<SiteMap, SiteMapDetail>();
        }

        public string GetSiteMapDocument(IEnumerable<SiteMapDetail> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");
            foreach (var sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                xmlns + "url",
                new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)),
                sitemapNode.LastModified == null ? null : new XElement(
                xmlns + "lastmod",
                sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                sitemapNode.Frequency == null ? null : new XElement(
                xmlns + "changefreq",
                sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
                sitemapNode.Priority == null ? null : new XElement(
                xmlns + "priority",
                sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
                root.Add(urlElement);
            }
            XDocument document = new XDocument(root);
            return document.ToString();
        }
        public string PopulateSiteMapByMenuCode(string menuCode)
        {
            bool isAdmin = false;
            string strHtml = "", strHomeUrl;

            if (ValidatorUtils.IsValidGuid(menuCode))
            {
                var lst =
                    UnitOfWork.MenuRepository.GetParentNodesOfSelectedNodeByMenuCode(Guid.Parse(menuCode)).ToList();
                if (lst.Any())
                {
                    foreach (var item in lst)
                    {
                        string menuLink;
                        string menuTitle = item.MenuTitle;

                        if (item.Page != null)
                        {
                            string pageUrl = item.Page.PageUrl;
                            bool? isExtenalLink = Convert.ToBoolean(item.Page.IsExtenalLink);
                            if (isExtenalLink == true)
                                menuLink = pageUrl;
                            else
                                menuLink = item.Page.PagePath.ToLower() + "/" + item.PageId + "/" + item.MenuId;
                        }
                        else
                        {
                            if (item.IsExtenalLink != null && item.IsExtenalLink == true)
                                menuLink = item.PageUrl;
                            else
                                menuLink = item.PagePath.ToLower() + "/" + item.PageId + "/" + item.MenuId;
                        }

                        strHtml += "<li>";
                        strHtml += "<a href='" + menuLink + "'><span data-pageid='" + item.PageId
                                   + "' data-menuid='" + item.MenuId + "' data-menucode='" + item.MenuCode + "'> "
                                   + menuTitle +
                                   " </span></a> <small><i class='glyphicon glyphicon-chevron-right'></i></small>";
                        strHtml += "</li>";

                        if (item.TypeId == (int)MenuTypeSetting.Admin)
                        {
                            isAdmin = true;
                        }
                    }
                }
            }

            if (isAdmin)
            {
                strHomeUrl = "<li><a href='/admin/dashboard/index'>";
                strHomeUrl += "<span><i class='glyphicon glyphicon-home'></i> " + LanguageResource.Home + "</span></a> <small><i class='glyphicon glyphicon-chevron-right'></i></small>";
                strHomeUrl += "</li>";
            }
            else
            {
                strHomeUrl = "<li><a href='/home/index'>"
                            + "<span><i class='glyphicon glyphiconicon-home'></i> " + LanguageResource.Home + "</span></a> <small><i class='glyphicon glyphicon-chevron-right'></i></small>"
                            + "</li>";
            }

            var result = "<ul class='breadcrumb'>";
            result += strHomeUrl;
            result += strHtml;
            result += "</ul>";
            return result;
        }
        public string PopulateSiteMapByMenuId(string menuId)
        {
            bool isAdmin = false;
            string strHtml = string.Empty, strHomeUrl;
            if (string.IsNullOrEmpty(menuId))
            {
                strHtml = string.Empty;
            }
            else
            {
                var isNumeric = !string.IsNullOrEmpty(menuId) && menuId.All(char.IsDigit);
                if (isNumeric)
                {
                    strHtml = CacheService.Get<string>(CacheKeySetting.SiteMap);
                    if (string.IsNullOrEmpty(strHtml))
                    {
                        var lst =
                            UnitOfWork.MenuRepository.GetParentNodesOfSelectedNodeByMenuId(Convert.ToInt32(menuId),
                                MenuStatus.Active).ToList();
                        if (lst.Any())
                        {
                            foreach (var item in lst)
                            {
                                string menuLink = string.Empty;
                                string menuTitle = item.MenuTitle;

                                if (item.Page != null)
                                {
                                    string pageUrl = item.Page.PageUrl;
                                    bool? isExtenalLink = Convert.ToBoolean(item.Page.IsExtenalLink);
                                    if (isExtenalLink == true)
                                        menuLink = pageUrl;
                                    else
                                        menuLink = item.Page.PagePath.ToLower() + "/" + item.Page.PageId;
                                }

                                strHtml += "<li>";
                                strHtml += "<a href='" + menuLink + "'><span>" + menuTitle +
                                           "</span></a> <small><i class='glyphicon glyphicon-chevron-right'></i></small>";
                                strHtml += "</li>";

                                if (item.TypeId == (int)MenuTypeSetting.Admin)
                                {
                                    isAdmin = true;
                                }
                            }

                            CacheService.Add(CacheKeySetting.SiteMap, strHtml, new CacheItemPolicy
                            {
                                AbsoluteExpiration = System.Web.Caching.Cache.NoAbsoluteExpiration.ToUniversalTime(),
                                Priority = CacheItemPriority.Default
                            });
                        }
                    }
                }
            }

            if (isAdmin)
            {
                strHomeUrl = "<li><a href='/admin/dashboard/index'>";
                strHomeUrl += "<span><i class='glyphicon glyphicon-home'></i> " + LanguageResource.Home + "</span></a> <small><i class='glyphicon glyphicon-chevron-right'></i></small>";
                strHomeUrl += "</li>";
            }
            else
            {
                strHomeUrl = "<li><a href='/home/index'>"
                            + "<span><i class='glyphicon glyphiconicon-home'></i> " + LanguageResource.Home + "</span></a> <small><i class='glyphicon glyphicon-chevron-right'></i></small>"
                            + "</li>";
            }

            var result = "<ul class='breadcrumb'>";
            result += strHomeUrl;
            result += strHtml;
            result += "</ul>";
            return result;
        }
        public string PopulateSiteMap(string controller = "home", string action = "index")
        {
            string strHtml = "";
            var siteMap = UnitOfWork.SiteMapRepository.GetDetail(controller, action);
            if (siteMap == null)
            {
                strHtml = "<li><a href='/home/index'>"
                             + "<span><i class='glyphicon glyphiconicon-home'></i> " + LanguageResource.Home + "</span></a> <small><i class='glyphicon glyphicon-chevron-right'></i></small>"
                             + "</li>";

                var siteMapResult = "<ul class='breadcrumb'>";
                siteMapResult += strHtml;
                siteMapResult += "</ul>";
                return siteMapResult;
            }

            var lst = UnitOfWork.SiteMapRepository.GetAllParentNodesOfSelectedNode(siteMap.SiteMapId, true).ToList();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    string menuTitle = item.Title;
                    string menuLink = item.Url;

                    strHtml += "<li>";
                    strHtml += "<a href='" + menuLink + "'><span> " + menuTitle +
                               " </span></a> <small><i class='glyphicon glyphicon-chevron-right'></i></small>";
                    strHtml += "</li>";
                }
            }
            else
            {
                strHtml = "<li><a href='/home/index'>"
                            + "<span><i class='glyphicon glyphiconicon-home'></i> " + LanguageResource.Home + "</span></a> <small><i class='glyphicon glyphicon-chevron-right'></i></small>"
                            + "</li>";
            }

            var result = "<ul class='breadcrumb'>";
            result += strHtml;
            result += "</ul>";
            return result;
        }

        public SiteMapDetail GetSiteMapDetail(int id)
        {
            var entity = UnitOfWork.SiteMapRepository.FindById(id);
            return entity.ToDto<SiteMap, SiteMapDetail>();
        }
        public void InsertSiteMap(SiteMapEntry entry)
        {
            ISpecification<SiteMapEntry> validator = new SiteMapEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var listOrder = UnitOfWork.SiteMapRepository.GetNewListOrder();
            var entity = entry.ToEntity<SiteMapEntry, SiteMap>();
            entity.HasChild = false;
            entity.ListOrder = listOrder;
            entity.LastModified = DateTime.UtcNow;

            UnitOfWork.SiteMapRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.ParentId != null && entry.ParentId > 0)
            {
                var parentEntity = UnitOfWork.SiteMapRepository.FindById(Convert.ToInt32(entry.ParentId));
                if (parentEntity == null) return;

                parentEntity.HasChild = true;
                UnitOfWork.SiteMapRepository.Update(parentEntity);

                var lineage = $"{parentEntity.Lineage},{entity.SiteMapId}";
                entity.Lineage = lineage;
                entity.Depth = lineage.Split(',').Count();
                entity.ParentId = entry.ParentId;
            }
            else
            {
                entity.ParentId = 0;
                entity.Lineage = $"{entity.SiteMapId}";
                entity.Depth = 1;
            }

            UnitOfWork.SiteMapRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateSiteMap(SiteMapEditEntry entry)
        {
            ISpecification<SiteMapEditEntry> validator = new SiteMapEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.SiteMapRepository.FindById(entry.SiteMapId);
            if (entity == null) return;

            if (entity.Title != entry.Title)
            {
                bool isDuplicate = UnitOfWork.SiteMapRepository.HasDataExisted(entry.Title, entry.ParentId);
                if (isDuplicate)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.DuplicateTitle, "Title", entry.Title, ErrorMessage.Messages[ErrorCode.DuplicateTitle]));
                    throw new ValidationError(dataViolations);
                }
            }

            if (entity.Action != entry.Action || entity.Controller != entry.Controller)
            {
                bool isDuplicate = UnitOfWork.SiteMapRepository.HasDataExisted(entry.Action, entry.Controller, entry.ParentId);
                if (isDuplicate)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.DuplicateControllerAction, "ControllerAction", $"{entry.Controller} - {entry.Action}" , ErrorMessage.Messages[ErrorCode.DuplicateControllerAction]));
                    throw new ValidationError(dataViolations);
                }
            }

            if (entry.ParentId != entity.ParentId && entry.ParentId != entity.SiteMapId)
            {
                if (entry.ParentId != null && entry.ParentId > 0)
                {
                    var children = UnitOfWork.SiteMapRepository.GetAllChildrenNodesOfSelectedNode(Convert.ToInt32(entry.SiteMapId)).ToList();
                    if (children.Any())
                    {
                        var parentIds = children.Select(x => x.SiteMapId).ToList();
                        if (parentIds.Contains(Convert.ToInt32(entry.ParentId)))
                        {
                            dataViolations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                            throw new ValidationError(dataViolations);
                        }
                    }

                    //Update parent entry
                    var parentEntryEntity = UnitOfWork.SiteMapRepository.FindById(Convert.ToInt32(entry.ParentId));
                    if (parentEntryEntity == null)
                    {
                        dataViolations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.NotFoundParentId]));
                        throw new ValidationError(dataViolations);
                    }
                    else
                    {
                        if (parentEntryEntity.HasChild == null || parentEntryEntity.HasChild == false)
                        {
                            parentEntryEntity.HasChild = true;
                            UnitOfWork.SiteMapRepository.Update(parentEntryEntity);
                        }
                    }

                    //Update parent entity
                    var parentEntity = UnitOfWork.SiteMapRepository.FindById(entity.ParentId);
                    if (parentEntity != null)
                    {
                        var childList = UnitOfWork.SiteMapRepository.GetAllChildrenNodesOfSelectedNode(Convert.ToInt32(entity.ParentId)).ToList();
                        if (childList.Any())
                        {
                            childList = childList.Where(x => (x.SiteMapId != entity.ParentId) && (x.SiteMapId != entity.SiteMapId)).ToList();
                            parentEntity.HasChild = childList.Any();
                            UnitOfWork.SiteMapRepository.Update(parentEntity);
                        }
                    }

                    var lineage = $"{parentEntryEntity.Lineage},{entry.SiteMapId}";
                    entity.Lineage = lineage;
                    entity.Depth = lineage.Split(',').Count();
                    entity.ParentId = entry.ParentId;
                }
                else
                {
                    entity.ParentId = 0;
                    entity.Lineage = $"{entry.SiteMapId}";
                    entity.Depth = 1;
                }
            }

            //Update entity
            var hasChild = UnitOfWork.SiteMapRepository.HasChild(entity.SiteMapId);
            entity.HasChild = hasChild;
            entity.Title = entry.Title;
            entity.Action = entry.Action;
            entity.Controller = entry.Controller;
            entity.Url = entry.Url;
            entity.Frequency = entry.Frequency;
            entity.Priority = entry.Priority;

            entity.Status = entry.Status;
            entity.LastModified = DateTime.UtcNow;

            UnitOfWork.SiteMapRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateSiteMapStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.SiteMapRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundSiteMap, "SiteMapId", id, ErrorMessage.Messages[ErrorCode.NotFoundSiteMap]));
                throw new ValidationError(violations);
            }

            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModified = DateTime.UtcNow;

            UnitOfWork.SiteMapRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        public IEnumerable<MenuPageDetail> GetListByPosition(Guid applicationId, Guid userId, int positionId, int? status)
        {
            var roleIds = UnitOfWork.UserRoleRepository.GetUserRolesByUserId(userId).Select(x => x.RoleId).ToList();
            var lst = UnitOfWork.MenuRepository.GetListByPosition(applicationId, positionId, roleIds, (MenuStatus?)status).ToList();
            var menus = new List<MenuPageDetail>();
            if (lst.Any())
            {
                menus.AddRange(lst.Select(item => new MenuPageDetail
                {
                    TypeId = item.TypeId, MenuId = item.MenuId, MenuCode = item.MenuCode, PositionId = item.PositionId, ParentId = item.ParentId, Depth = item.Depth, ListOrder = item.ListOrder, HasChild = item.HasChild, MenuName = item.MenuName, MenuTitle = item.MenuTitle, MenuAlias = item.MenuAlias, Description = item.Description, Target = item.Target, IconClass = item.IconClass ?? "glyphicon-picture", CssClass = item.CssClass, Status = item.Status, PageId = item.PageId, ApplicationId = item.ApplicationId, Page = new PageInfoDetail
                    {
                        PageId = item.Page.PageId, PageTitle = item.Page.PageTitle, PageName = item.Page.PageName, PageAlias = item.Page.PageAlias, PageUrl = item.Page.PageUrl, PagePath = item.Page.PagePath, ListOrder = item.Page.ListOrder, IconClass = item.Page.IconClass, Description = item.Page.Description, Keywords = item.Page.Keywords, PageHeadText = item.Page.PageHeadText, PageFooterText = item.Page.PageFooterText, StartDate = item.Page.StartDate, EndDate = item.Page.EndDate, DisableLink = item.Page.DisableLink, DisplayTitle = item.Page.DisplayTitle, IsExtenalLink = item.Page.IsExtenalLink ?? false, IsSecured = item.Page.IsSecured ?? false, IsMenu = item.Page.IsMenu ?? false, IsActive = item.Page.IsActive, ApplicationId = item.Page.ApplicationId, TemplateId = item.Page.TemplateId, LanguageCode = item.Page.LanguageCode
                    }
                }));
            }
            return menus.AsEnumerable();
        }
        public IEnumerable<MenuPageDetail> GetMenuListByStatus(Guid applicationId, int? menuTypeId, int positionId, MenuStatus? status)
        {
            var lst = UnitOfWork.MenuRepository.GetListByStatus(applicationId, menuTypeId, positionId, status).ToList();
            return lst.ToDtos<MenuPage, MenuPageDetail>();
        }
        public IEnumerable<MenuPageDetail> GetMenuListByParentId(int parentId, MenuStatus? status)
        {
            var lst = UnitOfWork.MenuRepository.GetListByParentIdStatus(parentId, status).ToList();
            var menus = new List<MenuPageDetail>();
            if (lst.Any())
            {
                menus.AddRange(lst.Select(item => new MenuPageDetail
                {
                    TypeId = item.TypeId,
                    MenuId = item.MenuId,
                    MenuCode = item.MenuCode,
                    PositionId = item.PositionId,
                    ParentId = item.ParentId,
                    Depth = item.Depth,
                    ListOrder = item.ListOrder,
                    HasChild = item.HasChild,
                    MenuName = item.MenuName,
                    MenuTitle = item.MenuTitle,
                    MenuAlias = item.MenuAlias,
                    Description = item.Description,
                    Target = item.Target,
                    IconClass = item.IconClass ?? "glyphicon-picture",
                    CssClass = item.CssClass,
                    Status = item.Status,
                    PageId = item.PageId,
                    ApplicationId = item.ApplicationId,
                    Page = new PageInfoDetail
                    {
                        PageId = item.Page.PageId,
                        PageTitle = item.Page.PageTitle,
                        PageName = item.Page.PageName,
                        PageAlias = item.Page.PageAlias,
                        PageUrl = item.Page.PageUrl,
                        PagePath = item.Page.PagePath,
                        ListOrder = item.Page.ListOrder,
                        IconClass = item.Page.IconClass,
                        Description = item.Page.Description,
                        Keywords = item.Page.Keywords,
                        PageHeadText = item.Page.PageHeadText,
                        PageFooterText = item.Page.PageFooterText,
                        StartDate = item.Page.StartDate,
                        EndDate = item.Page.EndDate,
                        DisableLink = item.Page.DisableLink,
                        DisplayTitle = item.Page.DisplayTitle,
                        IsExtenalLink = item.Page.IsExtenalLink ?? false,
                        IsSecured = item.Page.IsSecured ?? false,
                        IsMenu = item.Page.IsMenu ?? false,
                        IsActive = item.Page.IsActive,
                        ApplicationId = item.Page.ApplicationId,
                        TemplateId = item.Page.TemplateId,
                        LanguageCode = item.Page.LanguageCode
                    }
                }));
            }
            return menus.AsEnumerable();
        }

        public IEnumerable<MenuTreeDetail> GetTreeList(int typeId, int? status = null)
        {
            var lst = UnitOfWork.MenuRepository.GetTreeList(typeId, (MenuStatus?)status).ToList();
            return lst.ToDtos<MenuTree, MenuTreeDetail>();
        }
        public IEnumerable<MenuTreeNodeDetail> GetHierachicalList(int typeId, int? status = null, bool? isRootShowed = false)
        {
            var lst = UnitOfWork.MenuRepository.GetHierachicalList(typeId, (MenuStatus?)status, isRootShowed).ToList();
            return lst.ToDtos<MenuTreeNode, MenuTreeNodeDetail>();
        }
        public MenuDetail GetMenuDetails(int id)
        {
            var entity = UnitOfWork.MenuRepository.FindById(id);
            var item = entity.ToDto<Menu, MenuDetail>();
            if (entity.IconFile != null)
            {
                var documentFileInfo = DocumentService.GetFileInfoDetail((int)entity.IconFile);
                if (documentFileInfo != null)
                {
                    item.DocumentFileInfo = documentFileInfo;
                }
            }
            return item;
        }
      
        #region INSERT- UPDATE - DELETE 
        public int Insert(Guid applicationId, Guid userId, Guid roleId, int vendorId, MenuEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            var dataViolations = new List<RuleViolation>();
            if (entry == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NullMenuEntry, "MenuEntry"));
                throw new ValidationError(dataViolations);
            }

            using (var transcope = new TransactionScope())
            {
                int depth = UnitOfWork.MenuRepository.GetMenuDepth(entry.ParentId) + 1;
                int listOrder = UnitOfWork.MenuRepository.GetNewListOrder();

                var entity = entry.ToEntity<MenuEntry, Menu>();
                entity.IconClass = entry.IconClass??GlobalSettings.DefaultIconClass;
                entity.ApplicationId = applicationId;
                entity.MenuCode = Guid.NewGuid();
                entity.MenuAlias = StringUtils.ConvertTitle2Link(entry.MenuName);
                entity.Depth = depth;
                entity.HasChild = false;
                entity.IsSecured = entry.IsSecured??false;
                entity.ListOrder = listOrder;
                entity.Ip = ip;
                entity.CreatedDate = DateTime.UtcNow;
                entity.CreatedByUserId = userId;

                //Set positions
                if (entry.SelectedPositions.Any())
                {
                    var positionIds = (from selectedPositionId in entry.SelectedPositions select UnitOfWork.MenuPositionRepository.FindById(selectedPositionId) into position where position != null select position.PositionId).ToList();
                    if (positionIds.Any())
                    {
                        entity.PositionId = string.Join(", ", positionIds);
                    }
                }

                //Set icon
                if (entry.FileUpload != null && entry.FileUpload.ContentLength > 0)
                {
                    int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                    string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                    if (!allowedFileExtensions.Contains(entry.FileUpload.FileName.Substring(entry.FileUpload.FileName.LastIndexOf('.'))))
                    {
                        dataViolations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                        throw new ValidationError(dataViolations);
                    }
                    else if (entry.FileUpload.ContentLength > maxContentLength)
                    {
                        dataViolations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize  + maxContentLength + " MB"));
                        throw new ValidationError(dataViolations);
                    }
                    else
                    {
                        var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.FileUpload, (int)FileLocation.Menu, StorageType.Local);
                        entity.IconFile = fileInfo.FileId;
                    }
                }

                UnitOfWork.MenuRepository.Insert(entity);
                UnitOfWork.SaveChanges();

                //Update lineage and depth for menu
                if (entry.ParentId != null && entry.ParentId > 0)
                {
                    var parentEntity = UnitOfWork.MenuRepository.FindById(Convert.ToInt32(entry.ParentId));
                    if (parentEntity == null)
                    {
                        dataViolations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId"));
                        throw new ValidationError(dataViolations);
                    }
                    if (parentEntity.HasChild == null || parentEntity.HasChild == false)
                    {
                        parentEntity.HasChild = true;
                        UnitOfWork.MenuRepository.Update(parentEntity);
                        UnitOfWork.SaveChanges();
                    }

                    string lastCharacter = parentEntity.Lineage.Substring(parentEntity.Lineage.Length - 1, 1);
                    string lineage = lastCharacter == "," ? $"{parentEntity.Lineage}{entity.MenuId}" : $"{parentEntity.Lineage},{entity.MenuId}";

                    entity.Lineage = lineage;
                    entity.Depth = lineage.Split(',').Length;
                    entity.ParentId = entry.ParentId;
                }
                else
                {
                    entity.ParentId = 0;
                    entity.Lineage = $"{entity.MenuId}";
                    entity.Depth = 1;
                }

                UnitOfWork.MenuRepository.Update(entity);
                UnitOfWork.SaveChanges();

                //Insert permission for menu
                var menuId = entity.MenuId;
                if (menuId > 0)
                {
                    InsertDefaultMenuPermissions(menuId, roleId);
                }

                //Set Page Status
                if (entry.PageId != null)
                {
                    var page = UnitOfWork.PageRepository.FindById(entry.PageId);
                    if (page != null && page.IsMenu == false)
                    {
                        page.IsMenu = true;
                        UnitOfWork.PageRepository.Update(page);
                        UnitOfWork.SaveChanges();
                    }
                }

                //Update Menu Cache
                CacheService.Remove(CacheKeySetting.MenuDesktop);
                CacheService.Remove(CacheKeySetting.MenuByRole);
                LoadMenu(applicationId, userId, roleId);

                transcope.Complete();
                return menuId;
            }
        }

        public void Update(Guid applicationId, Guid userId, Guid roleId, int vendorId, MenuEditEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            var dataViolations = new List<RuleViolation>();
            if (entry == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NullMenuEditEntry, "MenuEditEntry"));
                throw new ValidationError(dataViolations);
            }

            //Update lineage and depth for menu
            var entity = UnitOfWork.MenuRepository.FindById(entry.MenuId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundMenu, "Menu"));
                throw new ValidationError(dataViolations);
            }

            if (entry.ParentId != entity.ParentId && entry.ParentId != entity.MenuId)
            {
                if (entry.ParentId != null && entry.ParentId > 0)
                {
                    var children = UnitOfWork.MenuRepository.GetAllChildrenNodesOfSelectedNode(Convert.ToInt32(entry.MenuId)).ToList();
                    if (children.Any())
                    {
                        var parentIds = children.Select(x => x.MenuId).ToList();
                        if (parentIds.Contains(Convert.ToInt32(entry.ParentId)))
                        {
                            dataViolations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                            throw new ValidationError(dataViolations);
                        }
                    }

                    //Update parent entry
                    var parentEntity = UnitOfWork.MenuRepository.FindById(Convert.ToInt32(entry.ParentId));
                    if (parentEntity == null)
                    {
                        dataViolations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId"));
                        throw new ValidationError(dataViolations);
                    }
                    if (parentEntity.HasChild == null || parentEntity.HasChild == false)
                    {
                        parentEntity.HasChild = true;
                        UnitOfWork.MenuRepository.Update(parentEntity);
                        UnitOfWork.SaveChanges();
                    }

                    string lastCharacter = parentEntity.Lineage.Substring(parentEntity.Lineage.Length - 1, 1);
                    string lineage = lastCharacter == "," ? $"{parentEntity.Lineage}{entity.MenuId}" : $"{parentEntity.Lineage},{entity.MenuId}";

                    entity.Lineage = lineage;
                    entity.Depth = lineage.Split(',').Length;
                    entity.ParentId = entry.ParentId;
                }
                else
                {
                    entity.Lineage = $"{entry.MenuId}";
                    entity.Depth = 1;
                    entity.ParentId = 0;
                }
            }

            //Set status of the menu element whether it has child element or not
            var childList = UnitOfWork.MenuRepository.GetChildren(entry.MenuId).ToList();
            entity.HasChild = childList.Any();

            if (entry.FileUpload != null && entry.FileUpload.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.FileUpload.FileName.Substring(entry.FileUpload.FileName.LastIndexOf('.'))))
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(dataViolations);
                }
                else if (entry.FileUpload.ContentLength > maxContentLength)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(dataViolations);
                }
                else
                {
                    if (entity.IconFile != null && entity.IconFile > 0)
                    {
                        DocumentService.UploadAndUpdateFile(userId, (int)entity.IconFile, entry.FileUpload);
                    }
                    else
                    {
                        var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.FileUpload, 
                            (int)FileLocation.Menu, StorageType.Local);
                        entity.IconFile = fileInfo.FileId;
                    }
                }
            }

            //Set positions
            if (entry.SelectedPositions.Any())
            {
                var positionIds = (from selectedPositionId in entry.SelectedPositions select UnitOfWork.MenuPositionRepository.FindById(selectedPositionId) into position where position != null select position.PositionId).ToList();
                if (positionIds.Any())
                {
                    entity.PositionId = string.Join(", ", positionIds);
                }
            }

            entity.HasChild = UnitOfWork.MenuRepository.HasChildren(entity.MenuId);
            entity.PageId = entry.PageId;
            entity.TypeId = entry.TypeId;
            entity.MenuName = entry.MenuName;
            entity.MenuTitle = entry.MenuTitle;
            entity.MenuAlias = StringUtils.ConvertTitle2Link(entry.MenuName);
            entity.Description = entry.Description;
            entity.Target = entry.Target;
            entity.IconClass = entry.IconClass;
            entity.CssClass = entry.CssClass;
            entity.Color = entry.Color;
            entity.IsSecured = entry.IsSecured;
            entity.Status = entry.Status;
            entity.LastUpdatedIp = ip;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.MenuRepository.Update(entity);
            UnitOfWork.SaveChanges();

            //Set Page Status
            if (entity.PageId != entry.PageId)
            {
                var pageEntryEntity = UnitOfWork.PageRepository.FindById(entry.PageId);
                if (pageEntryEntity != null && pageEntryEntity.IsMenu == false)
                {
                    pageEntryEntity.IsMenu = true;
                    UnitOfWork.PageRepository.Update(pageEntryEntity);
                    UnitOfWork.SaveChanges();

                    var pageEntity = UnitOfWork.PageRepository.FindById(entity.PageId);
                    if (pageEntity != null && pageEntity.IsMenu == true)
                    {
                        pageEntryEntity.IsMenu = false;
                        UnitOfWork.PageRepository.Update(pageEntryEntity);
                        UnitOfWork.SaveChanges();
                    }
                }
            }

            //Update Menu Cache
            CacheService.Remove(CacheKeySetting.MenuDesktop);
            CacheService.Remove(CacheKeySetting.MenuByRole);
            LoadMenu(applicationId, userId, roleId);
        }

        public void UpdateListOrder(int id, int? parentId, int listOrder)
        {
            using (var transcope = new TransactionScope())
            {
                var dataViolations = new List<RuleViolation>();

                //Update lineage and depth for menu
                var entity = UnitOfWork.MenuRepository.FindById(id);
                if (entity != null)
                {
                    if (parentId != null && parentId > 0)
                    {
                        var parentEntity = UnitOfWork.MenuRepository.FindById(Convert.ToInt32(parentId));
                        if (parentEntity == null)
                        {
                            dataViolations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId"));
                            throw new ValidationError(dataViolations);
                        }

                        var lineage = $"{parentEntity.Lineage},{id}";
                        entity.Lineage = lineage;
                        entity.Depth = lineage.Split(',').Count();
                    }
                    else
                    {
                        entity.Lineage = $"{id}";
                        entity.Depth = 0;
                    }

                    entity.ListOrder = listOrder;
                    UnitOfWork.MenuRepository.Update(entity);
                    UnitOfWork.SaveChanges();
                    transcope.Complete();
                }
            }
        }
        public void Delete(int id)
        {
            var entity = UnitOfWork.MenuRepository.FindById(id);
            if (entity != null)
            {
                DeleteChildByParentId(id);
                UnitOfWork.MenuRepository.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public void DeleteChildByParentId(int parentId)
        {
            using (var transcope = new TransactionScope())
            {
                var menuList = UnitOfWork.MenuRepository.GetListByParentId(parentId);
                foreach (var id in menuList)
                {
                    Delete(id);
                }
                transcope.Complete();
            }
        }

        #endregion

        #region MENU - XSLT ======================================================================================================================
        public string GenerateXmlFormat(MenuStatus status, bool isAdmin)
        {
            string strResult = string.Empty;
            List<MenuNodeData> menuList = PopulateHierachyList(status, isAdmin);
            if (menuList.Count > 0)
            {
                var elements = from x in menuList
                               select new XElement("Menu",
                                        new XElement("MenuId", x.MenuId),
                                        new XElement("ParentId", x.ParentId),
                                        new XElement("Depth", x.Depth),
                                        new XElement("ListOrder", x.ListOrder),
                                        new XElement("MenuName", x.MenuName),
                                        new XElement("Alias", x.Alias),
                                        new XElement("PageId", x.PageId),
                                        new XElement("Target", x.Target),
                                        new XElement("IconFile", x.IconFile),
                                        new XElement("CssClass", x.CssClass),
                                        new XElement("Description", x.Description),
                                        new XElement("Status", x.Status)
                                );

                XDocument document = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("XML Source Data for Menu"),
                    new XElement("Menus", elements));

                strResult = document.ToString();
            }
            return strResult;
        }

        #endregion ===============================================================================================================================

        #region HIEARACHY List =======================================================================
        public List<MenuNodeData> PopulateHierachyList(MenuStatus? status, bool isAdmin)
        {
            var list = UnitOfWork.MenuRepository.GetMenuListByStatus(status);
            //Output recursive data to list :           
            var recursiveObjects = FillRecursive(list, 0);
            return recursiveObjects;
        }
        private List<MenuNodeData> FillRecursive(IEnumerable<Menu> elements, int parentId)
        {
            var enumerable = elements as Menu[] ?? elements.ToArray();

            return enumerable.Where(x => x.ParentId.Equals(parentId)).Select(item => new MenuNodeData
            {
                MenuId = item.MenuId,
                ParentId = item.ParentId,
                ListOrder = item.ListOrder,
                PageId = item.PageId,
                MenuName = item.MenuName,
                Alias = item.MenuAlias,
                Depth = item.Depth,
                IconFile = item.IconFile,
                IconClass = item.IconClass,
                CssClass = item.CssClass,
                Status = (int)item.Status,
                Description = item.Description,
                Children = FillRecursive(enumerable, item.MenuId)
            }).ToList();
        }
        #endregion ===================================================================================

        #region Menu Permission

        public MenuRolePermissionEntry GetMenuRolePermissionEntry(Guid applicationId, int menuId)
        {
            var menu = GetMenuDetails(menuId);
            if (menu == null) return null;

            var roles = RoleService.GetRoles(applicationId, true).ToList();
            if (!roles.Any()) return null;

            var lst = new List<MenuRolePermissionDetail>();

            var defaultMenuPermissionLevels = GetMenuPermissionLevels(null).ToList();
            if (!defaultMenuPermissionLevels.Any())
            {
                lst.AddRange(roles.Select(role => new MenuRolePermissionDetail
                {
                    RoleId = role.RoleId,
                    Role = role,
                    MenuPermissionAccessLevels = null,
                }));

                return new MenuRolePermissionEntry
                {
                    Menu = menu,
                    MenuPermissionLevels = defaultMenuPermissionLevels,
                    MenuRolePermissions = lst
                };
            }
            else
            {
                foreach (var role in roles)
                {
                    var permissionAccessList = new List<MenuPermissionAccessLevel>();

                    //Get Permission Level in Menu Pemrission
                    var permissionLevelsInMenuPermission = GetMenuPermissionAccessLevelsByMenuId(menuId, role.RoleId);
                    if (!permissionLevelsInMenuPermission.Any())
                    {
                        permissionAccessList.AddRange(defaultMenuPermissionLevels.Select(item => new MenuPermissionAccessLevel
                        {
                            PermissionId = item.PermissionId,
                            MenuId = menuId,
                            AllowAccess = false
                        }));

                        lst.Add(new MenuRolePermissionDetail
                        {
                            RoleId = role.RoleId,
                            Role = role,
                            MenuPermissionAccessLevels = permissionAccessList,
                        });
                    }
                    else
                    {
                        //Add permission levels int menu permission
                        var defaultPermissionIds = defaultMenuPermissionLevels.Select(x => x.PermissionId).ToList();
                        var permissionIdsInModulePermission = permissionLevelsInMenuPermission.Select(x => x.PermissionId).ToList();
                        var intersectionPermissionIds = permissionIdsInModulePermission.Intersect(defaultPermissionIds).ToList();
                        if (intersectionPermissionIds.Any())
                        {
                            permissionAccessList.AddRange(from intersectionCapabilityId in intersectionPermissionIds
                                                          select permissionLevelsInMenuPermission.FirstOrDefault(x => x.PermissionId == intersectionCapabilityId)
                                                            into permission
                                                          where permission != null
                                                          select new MenuPermissionAccessLevel
                                                          {
                                                              PermissionId = permission.PermissionId,
                                                              MenuId = permission.MenuId,
                                                              AllowAccess = true
                                                          });
                        }

                        //Add the different capabilities of default module capabilities
                        var differentPermissionIds = defaultPermissionIds.Except(permissionIdsInModulePermission).ToList();
                        if (differentPermissionIds.Any())
                        {
                            permissionAccessList.AddRange(from differentLevelId in differentPermissionIds
                                                          select defaultMenuPermissionLevels.FirstOrDefault(x => x.PermissionId == differentLevelId)
                                                          into permission
                                                          where permission != null
                                                          select new MenuPermissionAccessLevel
                                                          {
                                                              PermissionId = permission.PermissionId,
                                                              MenuId = menuId,
                                                              AllowAccess = false
                                                          });
                        }

                        lst.Add(new MenuRolePermissionDetail
                        {
                            RoleId = role.RoleId,
                            MenuPermissionAccessLevels = permissionAccessList,
                            Role = role
                        });
                    }
                }
            }

            var entry = new MenuRolePermissionEntry
            {
                Menu = menu,
                MenuPermissionLevels = defaultMenuPermissionLevels,
                MenuRolePermissions = lst
            };

            return entry;
        }
        private List<MenuPermissionAccessLevel> GetMenuPermissionAccessLevelsByMenuId(int menuId, Guid roleId)
        {
            var permissions = new List<MenuPermissionAccessLevel>();
            var lst = UnitOfWork.MenuPermissionRepository.GetListByMenuId(menuId, roleId).ToList();
            if (lst.Any())
            {
                permissions.AddRange(lst.Select(item => new MenuPermissionAccessLevel
                {
                    MenuId = item.MenuId,
                    PermissionId = item.PermissionId,
                    PermissionName = item.MenuPermissionLevel.PermissionName,
                    Description = item.MenuPermissionLevel.Description,
                    DisplayOrder = item.MenuPermissionLevel.DisplayOrder,
                    IsActive = item.MenuPermissionLevel.IsActive,
                }));
            }
            return permissions;
        }
        public IEnumerable<MenuPermissionLevelDetail> GetMenuPermissionLevels(MenuPermissionLevelStatus? isActive)
        {
            var lst = UnitOfWork.MenuPermissionLevelRepository.GetMenuPermissionLevels(isActive);
            return lst.ToDtos<MenuPermissionLevel, MenuPermissionLevelDetail>();
        }

        public IEnumerable<MenuPageDetail> GetListByRoleId(int typeId, MenuStatus? status, Guid roleId)
        {
            var lst = UnitOfWork.MenuPermissionRepository.GetListByRoleId(typeId, status, roleId);
            return lst.ToDtos<MenuPage, MenuPageDetail>();
        }
        public IEnumerable<MenuPageDetail> GetListByUserId(Guid userId, int typeId, MenuStatus? status)
        {
            var menulst = new List<MenuPage>();
            var roles = UnitOfWork.UserRoleRepository.GetUserRolesByUserId(userId).Select(x => x.RoleId).ToList();
            if (!roles.Any()) return null;
            foreach (var roleId in roles)
            {
                var menuListByRole = UnitOfWork.MenuPermissionRepository.GetListByRoleId(typeId, status, roleId);
                menulst.AddRange(menuListByRole);
            }

            var menuListByUserId = UnitOfWork.MenuPermissionRepository.GetListByUserId(typeId, status, userId).ToArray();
            if (menuListByUserId.Any())
            {
                menulst.AddRange(menuListByUserId);
            }

            var lst = menulst.Distinct().ToList();
            return lst.ToDtos<MenuPage, MenuPageDetail>();
        }
        public bool IsMenuAllowedAccess(Guid userId, int menuId)
        {
            var roles = UnitOfWork.UserRoleRepository.GetUserRolesByUserId(userId).Select(x => x.RoleId).ToList();
            if (!roles.Any()) return false;
            var result = UnitOfWork.MenuPermissionRepository.IsMenuAllowedAccessByRoles(menuId, roles);
            if (result == false)
            {
                result = UnitOfWork.MenuPermissionRepository.IsMenuAllowedAccessByUserId(menuId, userId);
            }
            return result;
        }

        //INSERT- UPDATE - DELETE 
        public void InsertMenuPermission(MenuPermissionEntry entry)
        {
            bool isDataDuplicate = UnitOfWork.MenuPermissionRepository.HasDataExisted(entry.MenuId,
                     entry.RoleId);
            if (!isDataDuplicate)
            {
                var entity = new MenuPermission
                {
                    RoleId = entry.RoleId,
                    MenuId = entry.MenuId,
                    PermissionId = entry.PermissionId,
                    UserIds = entry.UserIds,
                    AllowAccess = entry.AllowAccess
                };
                UnitOfWork.MenuPermissionRepository.Insert(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public void InsertDefaultMenuPermissions(int menuId, Guid roleId)
        {
            if (menuId < 0) return;

            var permissionLevels = UnitOfWork.MenuPermissionLevelRepository.GetMenuPermissionLevels(null).Select(x => x.PermissionId).ToList();
            if (permissionLevels.Any())
            {
                foreach (var permissionId in permissionLevels)
                {
                    var entry = new MenuPermissionEntry
                    {
                        MenuId = menuId,
                        RoleId = roleId,
                        PermissionId = permissionId,
                        AllowAccess = true
                    };
                    InsertMenuPermission(entry);
                }
            }
        }

        public void UpdateMenuPermission(int id, MenuPermissionEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MenuPermissionRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForMenuPermission, "MenuPermission", id));
                throw new ValidationError(violations);
            }

            entity.MenuId = entry.MenuId;
            entity.RoleId = entry.RoleId;
            entity.UserIds = string.Join(",", entry.UserIds.ToArray());
            entity.AllowAccess = entry.AllowAccess;

            UnitOfWork.MenuPermissionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateMenuPermissionStatus(Guid roleId, int menuId, int permissionId, bool status)
        {
            ////Update existed menu permissions
            //var existedMenuPermissions = UnitOfWork.MenuPermissionRepository.GetListByMenuId(entry.MenuId).ToList();
            //if (!entry.AccessedMenuPermissions.Any())
            //{
            //    UnitOfWork.MenuPermissionRepository.DeleteMenuPermissions(existedMenuPermissions);
            //}
            //else
            //{
            //    var accessedMenuPermissions = entry.AccessedMenuPermissions.ToEntities<MenuPermissionDetail, MenuPermission>();
            //    var differentList = existedMenuPermissions.Except(accessedMenuPermissions);
            //    UnitOfWork.MenuPermissionRepository.DeleteMenuPermissions(differentList);
            //}

            ////Add New Permission for Menu
            //if (entry.RolePermissions.Any())
            //{
            //    foreach (var rolePermission in entry.RolePermissions)
            //    {
            //        var menuPemmissionEntry = new MenuPermissionEntry
            //        {
            //            MenuId = entity.MenuId,
            //            RoleId = rolePermission.RoleId,
            //            AllowAccess = rolePermission.AllowAccess,
            //            UserIds = rolePermission.UserIds
            //        };
            //        InsertMenuPermission(menuPemmissionEntry);
            //    }
            //}

            var item = UnitOfWork.MenuPermissionRepository.GetDetails(menuId, permissionId, roleId);
            if (item == null)
            {
                var entity = new MenuPermission
                {
                    RoleId = roleId,
                    MenuId = menuId,
                    PermissionId = permissionId,
                    AllowAccess = status
                };

                UnitOfWork.MenuPermissionRepository.Insert(entity);
                UnitOfWork.SaveChanges();
            }
            else
            {
                item.AllowAccess = status;
                UnitOfWork.MenuPermissionRepository.Update(item);
                UnitOfWork.SaveChanges();
            }
        }

        public void DeleteMenuPermission(int levelId, int menuId, Guid roleId)
        {
            var violations = new List<RuleViolation>();
            var item = UnitOfWork.MenuPermissionRepository.GetDetails(levelId, menuId, roleId);
            if (item == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForMenuPermission, "MenuPermission"));
                throw new ValidationError(violations);
            }

            UnitOfWork.MenuPermissionRepository.Delete(item);
            UnitOfWork.SaveChanges();
        }
        public void DeleteMenuPermissions(IEnumerable<MenuPermissionDetail> permissionLst)
        {
            if (permissionLst != null)
            {
                foreach (var permission in permissionLst)
                {
                    var violations = new List<RuleViolation>();
                    var item = UnitOfWork.MenuPermissionRepository.GetDetails(permission.PermissionId, permission.MenuId, permission.RoleId);
                    if (item == null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NotFoundForMenuPermission, "MenuPermission"));
                        throw new ValidationError(violations);
                    }

                    UnitOfWork.MenuPermissionRepository.Delete(item);
                }
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteMenuPermissionByRoleId(Guid roleId)
        {
            var violations = new List<RuleViolation>();
            var lst = UnitOfWork.MenuPermissionRepository.GetListByRoleId(roleId);
            if (lst == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForMenuPermission, "MenuPermission"));
                throw new ValidationError(violations);
            }

            foreach (var item in lst)
            {
                var menuPermmission = new MenuPermission
                {
                    RoleId = roleId,
                    MenuId = item.MenuId,
                    AllowAccess = false
                };
                UnitOfWork.MenuPermissionRepository.Update(menuPermmission);
            }
        }
        #endregion

        #region Menu Position
        public MultiSelectList PopulateMenuPositionMultiSelectList(int? typeId = null, bool? isActive = null, string positionId = null, string[] selectedValues = null)
        {
            return UnitOfWork.MenuPositionRepository.PopulateMenuPositionMultiSelectList(typeId ?? Convert.ToInt32(MenuTypeSetting.Admin), isActive, positionId, selectedValues);
        }

        public MultiSelectList PopulateMenuPositionMultiSelectedList(string positionId, bool? isActive = null)
        {
            return UnitOfWork.MenuPositionRepository.PopulateMenuPositionMultiSelectedList(positionId, isActive);
        }

        public MultiSelectList PopulateMenuPositionMultiSelectedListByMenuId(int? menuId, bool? isActive = null)
        {
            return UnitOfWork.MenuPositionRepository.PopulateMenuPositionMultiSelectedListByMenuId(menuId, isActive);
        }

        #endregion

        #region Menu Type
        public SelectList PopulateMenuTypeSelectList(bool? isActive = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.MenuTypeRepository.PopulateMenuTypeSelectList(isActive, selectedValue, isShowSelectText);
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
                    CacheService = null;
                    DocumentService = null;
                    RoleService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
