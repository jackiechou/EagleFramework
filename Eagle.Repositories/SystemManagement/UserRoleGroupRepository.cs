using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class UserRoleGroupRepository : RepositoryBase<UserRoleGroup>, IUserRoleGroupRepository
    {
        public UserRoleGroupRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<UserRoleGroupInfo> GetUserRoleGroups(Guid userId)
        {
            return (from urg in DataContext.Get<UserRoleGroup>()
                    join rg in DataContext.Get<RoleGroup>() on urg.RoleGroupId equals rg.RoleGroupId into roleGroupJoin
                    from rgj in roleGroupJoin.DefaultIfEmpty()
                    join r in DataContext.Get<Role>() on rgj.RoleId equals r.RoleId into roleJoin
                    from rj in roleJoin.DefaultIfEmpty()
                    join g in DataContext.Get<Group>() on rgj.GroupId equals g.GroupId into groupInfo
                    from gj in groupInfo.DefaultIfEmpty()
                    where urg.UserId == userId
                    select new UserRoleGroupInfo
                    {
                        RoleGroupId = rgj.RoleGroupId,
                        UserId = urg.UserId,
                        EffectiveDate = urg.EffectiveDate,
                        ExpiryDate = urg.ExpiryDate,
                        IsDefault = urg.IsDefault,
                        RoleGroup = rgj,
                        Role = rj,
                        Group = gj
                    }).AsEnumerable();
        }

        public IEnumerable<UserRoleGroup> GetUserRoleGroupsByUserId(Guid userId)
        {
            return (from p in DataContext.Get<UserRoleGroup>() where p.UserId == userId select p).AsEnumerable();
        }
        public UserRoleGroup GetDetail(Guid userId,  int roleGroupId)
        {
            return (from p in DataContext.Get<UserRoleGroup>()
                          where p.UserId == userId && p.RoleGroupId == roleGroupId
                    select p).FirstOrDefault();
        }
        public bool HasDataExisted(Guid userId, int roleGroupId)
        {
            var result = (from p in DataContext.Get<UserRoleGroup>()
                          where p.UserId == userId && p.RoleGroupId == roleGroupId
                          select p).FirstOrDefault();
            return result != null;
        }
    }
}
