using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class RoleGroupRepository : RepositoryBase<RoleGroup>, IRoleGroupRepository
    {
        public RoleGroupRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<RoleGroup> GetRoleGroupsByRoleId(Guid applicationId, Guid roleId, bool? status=null)
        {
            return (from rg in DataContext.Get<RoleGroup>()
                    join r in DataContext.Get<Role>() on rg.RoleId equals r.RoleId into roleList
                    from ml in roleList.DefaultIfEmpty()
                    where ml.ApplicationId == applicationId && rg.RoleId == roleId && (status == null || ml.IsActive == status)
                    select rg).AsEnumerable();
        }

        public RoleGroup GetDetails(Guid roleId, Guid groupId)
        {
          return DataContext.Get<RoleGroup>().FirstOrDefault(x => x.RoleId == roleId && x.GroupId == groupId);
        }
        
        public RoleGroupInfo GetRoleGroupDetail(int roleGroupId)
        {
            return (from rg in DataContext.Get<RoleGroup>()
                    join r in DataContext.Get<Role>() on rg.RoleId equals r.RoleId into roleList
                    from ml in roleList.DefaultIfEmpty()
                    join g in DataContext.Get<Group>() on rg.GroupId equals g.GroupId into groupList
                    from gl in groupList.DefaultIfEmpty()
                    where rg.RoleGroupId == roleGroupId
                    select new RoleGroupInfo
                    {
                        RoleGroupId = rg.RoleGroupId,
                        RoleId = rg.RoleId,
                        GroupId = gl.GroupId,
                        IsDefaultGroup = rg.IsDefaultGroup??false,
                        Group = gl,
                        Role= ml
                    }).FirstOrDefault();
        }

        public RoleGroupInfo GetRoleGroupDetails(Guid roleId, Guid groupId)
        {
            return (from rg in DataContext.Get<RoleGroup>()
                join r in DataContext.Get<Role>() on rg.RoleId equals r.RoleId into roleList
                from ml in roleList.DefaultIfEmpty()
                join g in DataContext.Get<Group>() on rg.GroupId equals g.GroupId into groupList
                from gl in groupList.DefaultIfEmpty()
                where rg.RoleId == roleId && gl.GroupId == groupId
                select new RoleGroupInfo
                {
                    RoleGroupId = rg.RoleGroupId,
                    RoleId = rg.RoleId,
                    GroupId = gl.GroupId,
                    IsDefaultGroup = rg.IsDefaultGroup,
                    Group = gl,
                    Role = ml
                }).FirstOrDefault();
        }

        public MultiSelectList PopulateGroupMultiSelectList(Guid applicationId, Guid? roleId, bool? status = null, string[] selectedValues = null, bool? isShowSelectText = false)
        {
            var list = new List<SelectListItem>();
            if (roleId != null)
            {
                list = (from g in DataContext.Get<Group>()
                    join rg in DataContext.Get<RoleGroup>() on g.GroupId equals rg.GroupId into roleGroupLst
                    from rgl in roleGroupLst.DefaultIfEmpty()
                    where
                        g.ApplicationId == applicationId && rgl.RoleId == roleId &&
                        (status == null || g.IsActive == status)
                    select new SelectListItem
                    {
                        Text = g.GroupName,
                        Value = g.GroupId.ToString(),
                    }).OrderByDescending(r => r.Text).ToList();
            }
           
            if (list.Count > 0)
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    list.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.Select} --" });
            }
            return new MultiSelectList(list, "Value", "Text", selectedValues);
        }

        public bool HasDataExisted(Guid roleId, Guid groupId)
        {
            var query = DataContext.Get<RoleGroup>().FirstOrDefault(x => x.RoleId == roleId && x.GroupId == groupId);
            return (query != null);
        }
    }
}
