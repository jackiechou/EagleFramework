using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Extension;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Companies;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Business
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(IDataContext dataContext) : base(dataContext) { }

        #region TREE GRID

        public IEnumerable<TreeGrid> GetCompanyTreeGrid(CompanyStatus? status, int? selectedId, bool? isRootShowed = false)
        {
            var list = (from x in DataContext.Get<Company>()
                        where status == null || x.Status == status
                        orderby x.ListOrder ascending
                        select new TreeGrid
                        {
                            id = x.CompanyId.ToString(),
                            parentId = x.ParentId.ToString(),
                            level = x.Depth,
                            name = x.CompanyName,
                            title = x.CompanyName,
                            text = x.CompanyName,
                            state = new TreeGridState
                            {
                                opened = (x.HasChild != null && x.HasChild == true),
                                selected = (selectedId != null && x.CompanyId == selectedId),
                            },
                            data = new TreeGridData
                            {
                                status = (x.Status == CompanyStatus.InActive) ? LanguageResource.InActive : LanguageResource.Active,
                                action = @"<a data-id='" + x.CompanyId + "' title=" + LanguageResource.Reset + " class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='" + x.CompanyId + "' title = " + LanguageResource.Edit + " class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                            }
                        }).ToList();

            var recursiveObjects = new List<TreeGrid>();
            if (list.Any())
            {
                recursiveObjects = RecursiveFillTreeGrid(list, "0");
                if (isRootShowed != null && isRootShowed == true)
                {
                    recursiveObjects.Insert(0, new TreeGrid
                    {
                        id = "0",
                        parentId = "0",
                        level = 1,
                        name = LanguageResource.Root,
                        title = LanguageResource.Root,
                        text = LanguageResource.Root,
                        type = "glyphicon glyphicon-folder-close",
                        icon = "glyphicon glyphicon-file",
                        state = new TreeGridState
                        {
                            opened = false,
                            selected = (selectedId != null && selectedId == 0),
                        },
                        data = new TreeGridData
                        {
                            status = LanguageResource.Published,
                            action = @"<a data-id='0' title='" + LanguageResource.Reset + "' class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='0' title='" + LanguageResource.Edit + "' class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                        }
                    });
                }
            }
            else
            {
                recursiveObjects.Insert(0, new TreeGrid
                {
                    id = "0",
                    parentId = "0",
                    level = 1,
                    name = LanguageResource.NonSpecified,
                    title = LanguageResource.NonSpecified,
                    text = LanguageResource.NonSpecified,
                    type = "glyphicon glyphicon-folder-close",
                    icon = "glyphicon glyphicon-file",
                    state = new TreeGridState
                    {
                        opened = false,
                        selected = (selectedId != null && selectedId == 0),
                    },
                    data = new TreeGridData
                    {
                        status = LanguageResource.Published,
                        action = @"<a data-id='0' title='" + LanguageResource.Reset + "' class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='0' title = '" + LanguageResource.Edit + "' class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                    }
                });
            }

            return recursiveObjects;
        }
        private List<TreeGrid> RecursiveFillTreeGrid(List<TreeGrid> elements, string parentid)
        {
            if (elements == null) return null;
            List<TreeGrid> items = new List<TreeGrid>();
            List<TreeGrid> children = elements.Where(p => p.parentId == parentid).Select(
               p => new TreeGrid
               {
                   id = p.id,
                   parentId = p.parentId,
                   level = p.level,
                   name = p.name,
                   title = p.title,
                   text = p.text,
                   state = new TreeGridState
                   {
                       opened = (p.hasChild != null && p.hasChild == true),
                       selected = p.state.selected,
                   },
                   data = new TreeGridData
                   {
                       status = p.data.status,
                       action = @"<a data-id='" + p.id + "' title='" + LanguageResource.Reset + "' class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='" + p.id + "' title = '" + LanguageResource.Edit + "' class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                   },
               }).ToList();

            if (children.Count > 0)
            {
                items.AddRange(children.Select(child => new TreeGrid
                {
                    id = child.id,
                    parentId = child.parentId,
                    level = child.level,
                    name = child.name,
                    title = child.title,
                    text = child.text,
                    state = new TreeGridState
                    {
                        opened = (child.hasChild != null && child.hasChild == true),
                        selected = child.state.selected,
                    },
                    data = new TreeGridData
                    {
                        status = child.data.status,
                        action = @"<a data-id='" + child.id + "' title='" + LanguageResource.Reset + "' class='btn btn-small btn-warning deleteItem'><span class='icon-trash'></span></a> " +
                                     "<a data-id ='" + child.id + "' title = '" + LanguageResource.Edit + "' class='btn btn-small btn-success editItem'><span class='icon-edit'></span></a>"
                    },
                    children = RecursiveFillTreeGrid(elements, child.id)
                }));
            }
            return items;
        }

        #endregion

        public IEnumerable<Company> GetCompanies(CompanyStatus? status)
        {
            return DataContext.Get<Company>().Where(x=> status == null || x.Status == status).AsEnumerable();
        }


        public bool HasDataExisted(string name, int? parentId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var query = DataContext.Get<Company>().FirstOrDefault(c => c.ParentId == parentId &&
                c.CompanyName.Equals(name, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }
        public int GetDepth(int? companyId)
        {
            int depth = 1;
            if (companyId != null && companyId > 0)
            {
                var query = from x in DataContext.Get<Company>() where x.CompanyId == companyId select (int)x.Depth;
                if (query.Any())
                {
                    depth = query.Max() + 1;
                }
            }
            return depth;
        }
        public string GetSlogan(int companyId)
        {
            return DataContext.Get<Company>().Where(x =>
             x.CompanyId == companyId).Select(x => x.Slogan).FirstOrDefault();
        }
        public string GetSupportOnline(int companyId)
        {
            return DataContext.Get<Company>().Where(x =>
             x.CompanyId == companyId).Select(x => x.SupportOnline).FirstOrDefault();
        }

        public int GetNewListOrder()
        {
            return DataContext.Get<Company>().Max(x => x.ListOrder == null ? 0 : x.ListOrder + 1) ?? 0;
        }

        public bool HasChildren(int companyId)
        {
            var query = from x in DataContext.Get<Company>()
                        orderby x.ListOrder ascending
                        where x.ParentId == companyId
                        select x;
            return query.Any();
        }
    }
}
