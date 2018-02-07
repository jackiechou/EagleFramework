using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<Role> GetRoles(Guid applicationId, bool? status)
        {
            return (from r in DataContext.Get<Role>()
                    where r.ApplicationId == applicationId && (status == null || r.IsActive == status)
                    select r).OrderBy(p => p.ListOrder).AsEnumerable();
        }
        public IEnumerable<RoleInfo> GetRoles(Guid applicationId, Guid userId, bool? status = null)
        {
            return (from r in DataContext.Get<Role>()
                    join ur in DataContext.Get<UserRole>() on r.RoleId equals ur.RoleId into roleLst
                    from usrRole in roleLst.DefaultIfEmpty()
                    where r.ApplicationId == applicationId && (status == null || r.IsActive == status)
                          && usrRole.UserId == userId
                    select new RoleInfo
                    {
                        RoleId = r.RoleId,
                        RoleCode = r.RoleCode,
                        RoleName = r.RoleName,
                        LoweredRoleName = r.LoweredRoleName,
                        Description = r.Description,
                        ListOrder = r.ListOrder,
                        IsActive = r.IsActive,
                        ApplicationId = r.ApplicationId
                    }).AsEnumerable();
        }
        public IEnumerable<Role> GetRoles(Guid applicationId, bool? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            return DataContext.Get<Role>().Where(x => x.ApplicationId == applicationId && (status == null || x.IsActive == status)).WithRecordCount(ref recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<RoleInfo> SearchRoles(Guid applicationId, Guid? groupId, string roleName, bool? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = (from r in DataContext.Get<Role>()
                         join rg in DataContext.Get<RoleGroup>() on r.RoleId equals rg.RoleId into rolelst
                         from rgl in rolelst.DefaultIfEmpty()
                         join g in DataContext.Get<Group>() on rgl.GroupId equals g.GroupId into groupLst
                         from gl in groupLst.DefaultIfEmpty()
                         where r.ApplicationId == applicationId
                          && (status == null || r.IsActive == status)
                          && (groupId == null || gl.GroupId == groupId)
                         select new RoleInfo
                         {
                             RoleId = r.RoleId,
                             RoleCode = r.RoleCode,
                             RoleName = r.RoleName,
                             LoweredRoleName = r.LoweredRoleName,
                             Description = r.Description,
                             ListOrder = r.ListOrder,
                             IsActive = r.IsActive,
                             ApplicationId = r.ApplicationId
                         });
          
            if (!string.IsNullOrEmpty(roleName))
            {
                query = query.Where(r => r.RoleName.ToLower().Contains(roleName.ToLower()));
            }

            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<Role>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
        public SelectList PopulateRoleToDropDownList(bool? status = null, string selectedValue = null, bool? isShowSelectText = false)
        {
            var lst = (from p in DataContext.Get<Role>()
                       where status == null || p.IsActive == status
                       select new SelectListItem
                       {
                           Text = p.RoleName,
                           Value = p.RoleId.ToString(),
                           Selected = (!string.IsNullOrEmpty(selectedValue) && p.RoleId.ToString() == selectedValue)
                       }).ToList();

            if (lst.Count == 0)
            {
                lst.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.None} --" });
            }
            else
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    lst.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.Select} --" });
            }

            SelectList selectlist = new SelectList(lst, "Value", "Text", selectedValue);
            return selectlist;
        }
        public MultiSelectList PopulateRoleMultiSelectList(bool? status = null, string[] selectedValues = null, bool? isShowSelectText = false)
        {
            var list = (from r in DataContext.Get<Role>()
                        where (status == null || r.IsActive == status)
                              && (status == null || r.IsActive == status)
                        select new SelectListItem
                        {
                            Text = r.RoleName,
                            Value = r.RoleId.ToString(),
                        }).OrderByDescending(r => r.Text).ToList();

            if (list.Count == 0)
            {
                list.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.None} --" });
            }
            else
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    list.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.SelectRole} --" });
            }

            return new MultiSelectList(list, "Value", "Text", selectedValues);
        }
        public MultiSelectList PopulateRoleMultiSelectList(Guid userId, bool? status = null, string[] selectedValues = null, bool? isShowSelectText = false)
        {
            var list = (from r in DataContext.Get<Role>()
                        join u in DataContext.Get<UserRole>() on r.RoleId equals u.RoleId into roleLst
                        from role in roleLst.DefaultIfEmpty()
                        where (status == null || r.IsActive == status)
                              && role.UserId == userId
                        select new SelectListItem
                        {
                            Text = r.RoleName,
                            Value = r.RoleId.ToString(),
                        }).OrderByDescending(r => r.Text).ToList();

            if (list.Count > 0)
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    list.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.SelectRole} --" });
            }

            return new MultiSelectList(list, "Value", "Text", selectedValues);
        }
        public bool IsRoleExisted(Guid applicationId, string roleName)
        {
            var query = DataContext.Get<Role>().FirstOrDefault(x => x.ApplicationId == applicationId
             && x.RoleName.ToUpper().Equals(roleName.ToUpper()));
            return (query != null);
        }
    }
}
