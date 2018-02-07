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
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<Group> GetList(Guid applicationId, bool? status = null)
        {
            return (from p in DataContext.Get<Group>().Where(x => x.ApplicationId == applicationId
                    && (status == null || x.IsActive == status) ) select p).AsEnumerable();
        }
        public IEnumerable<Group> GetList(Guid applicationId, Guid roleId, bool? status = null)
        {
            return (from r in DataContext.Get<Group>()
                join rg in DataContext.Get<RoleGroup>() on r.GroupId equals rg.GroupId into roleGroupLst
                from rgl in roleGroupLst.DefaultIfEmpty()
                where (status == null || r.IsActive == status)
                      && rgl.RoleId == roleId
                select r).AsEnumerable();
        }
        public IEnumerable<Group> GetList(Guid applicationId, string groupName, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = DataContext.Get<Group>().Where(x => x.ApplicationId == applicationId);
            if (!string.IsNullOrEmpty(groupName))
            {
                query = query.Where(x => x.GroupName.ToLower().Contains(groupName.ToLower()));
            }
            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<Group>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
        public SelectList PopulateGroupDropDownList(Guid applicationId, bool? status = null, string selectedValue = null, bool? isShowSelectText = false)
        {
            var lst = (from g in DataContext.Get<Group>()
                       join rg in DataContext.Get<RoleGroup>() on g.GroupId equals rg.GroupId into roleGroupLst
                       from rgl in roleGroupLst.DefaultIfEmpty()
                       where g.ApplicationId == applicationId && (status == null || g.IsActive == status)
                       select new SelectListItem
                       {
                           Text = g.GroupName,
                           Value = g.GroupId.ToString(),
                           Selected = (!string.IsNullOrEmpty(selectedValue) && g.GroupId.ToString() == selectedValue)
                       }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText != null & isShowSelectText == true)
                    lst.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.SelectGroup} --" });
            }
            else
            {
                lst.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.None} --" });
            }

            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        public SelectList PopulateGroupDropDownList(Guid applicationId, Guid roleId, bool? status = null, bool? isShowSelectText = false)
        {
            var selectedGroups = (from rg in DataContext.Get<RoleGroup>()
                       where rg.RoleId == roleId
                       select rg.GroupId).ToList();


            var groups = from g in DataContext.Get<Group>()
                       where g.ApplicationId == applicationId
                       && (status == null || g.IsActive == status) select g;

            var lst = new List<SelectListItem>();
            foreach (var item in groups)
            {
                lst.Add(new SelectListItem
                {
                    Text = item.GroupName,
                    Value = item.GroupId.ToString(),
                    Selected = selectedGroups.Contains(item.GroupId)
                });
            }
                  
            if (lst.Count == 0)
            {
                lst.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.None} --" });
            }
            else
            {
                if (isShowSelectText!=null & isShowSelectText==true)
                    lst.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.Select} --" });
            }

            SelectList selectlist = new SelectList(lst, "Value", "Text");
            return selectlist;
        }

        public MultiSelectList PopulateGroupMultiSelectList(Guid applicationId, bool? status = null, string[] selectedValues = null, bool? isShowSelectText = false)
        {
            var list = (from g in DataContext.Get<Group>()
                        where g.ApplicationId == applicationId 
                              && (status == null || g.IsActive == status)
                        select new SelectListItem
                        {
                            Text = g.GroupName,
                            Value = g.GroupId.ToString(),
                        }).OrderByDescending(r => r.Text).ToList();

            if (list.Count > 0)
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    list.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.Select} --" });
            }

            return new MultiSelectList(list, "Value", "Text", selectedValues);
        }
       
        public bool HasGroupExisted(Guid applicationId, string groupName)
        {
            var entity = DataContext.Get<Group>().FirstOrDefault(p => p.ApplicationId== applicationId && p.GroupName.ToLower().Contains(groupName.ToLower()));
            return (entity != null);
        }
    }
}
