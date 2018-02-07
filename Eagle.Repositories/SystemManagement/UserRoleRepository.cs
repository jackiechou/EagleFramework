using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDataContext dataContext) : base(dataContext) { }
      
        public IEnumerable<UserRoleInfo> GetUserRolesByUserId(Guid userId)
        {
            var roles = from ur in DataContext.Get<UserRole>()
                        join u in DataContext.Get<User>() on ur.UserId equals u.UserId into userInfo
                        from uri in userInfo.DefaultIfEmpty()
                        join r in DataContext.Get<Role>() on ur.RoleId equals r.RoleId into rolelist
                        from rl in rolelist.DefaultIfEmpty()
                        where ur.UserId == userId
                        select new UserRoleInfo
                        {
                            UserRoleId = ur.UserRoleId,
                            RoleId = ur.RoleId,
                            UserId = ur.UserId,
                            EffectiveDate = ur.EffectiveDate,
                            ExpiryDate = ur.ExpiryDate,
                            IsTrialUsed = ur.IsTrialUsed ?? false,
                            IsDefaultRole = ur.IsDefaultRole ?? false,
                            User = uri,
                            Role = rl
                        };
            return roles.AsEnumerable();
        }

        public IEnumerable<UserRoleInfo> GetUserRolesByUserName(string userName, bool? isActive)
        {
            var roles = from ur in DataContext.Get<UserRole>()
                join u in DataContext.Get<User>() on ur.UserId equals u.UserId into userInfo
                from uri in userInfo.DefaultIfEmpty()
                join r in DataContext.Get<Role>() on ur.RoleId equals r.RoleId into rolelist
                from rl in rolelist.DefaultIfEmpty()
                where uri.UserName.ToLower() == userName.ToLower() && (isActive == null || rl.IsActive == isActive)
                        select new UserRoleInfo
                {
                    UserRoleId = ur.UserRoleId,
                    RoleId = ur.RoleId,
                    UserId = ur.UserId,
                    EffectiveDate = ur.EffectiveDate,
                    ExpiryDate = ur.ExpiryDate,
                    IsTrialUsed = ur.IsTrialUsed ?? false,
                    IsDefaultRole = ur.IsDefaultRole ?? false,
                    User = uri,
                    Role = rl
                };
            return roles.AsEnumerable();
        }

        public IEnumerable<UserRoleInfo> GetRoles(Guid userId, bool? isActive)
        {
            var roles = from ur in DataContext.Get<UserRole>()
                        join u in DataContext.Get<User>() on ur.UserId equals u.UserId into userInfo
                        from uri in userInfo.DefaultIfEmpty()
                        join r in DataContext.Get<Role>() on ur.RoleId equals r.RoleId into rolelist
                        from rl in rolelist.DefaultIfEmpty()
                        where ur.UserId == userId && (isActive == null || rl.IsActive == isActive)
                        select new UserRoleInfo
                        {
                            UserRoleId = ur.UserRoleId,
                            RoleId = ur.RoleId,
                            UserId = ur.UserId,
                            EffectiveDate = ur.EffectiveDate,
                            ExpiryDate = ur.ExpiryDate,
                            IsTrialUsed = ur.IsTrialUsed ?? false,
                            IsDefaultRole = ur.IsDefaultRole??false,
                            User = uri,
                            Role = rl
                        };
            return roles.AsEnumerable();
        }


        public IEnumerable<UserRole> GetUsersByRoleId(Guid roleId)
        {
            return (from p in DataContext.Get<UserRole>() where p.RoleId == roleId select p).AsEnumerable();
        }

        public IEnumerable<User> GetUsers(Guid roleId)
        {
            return (from ur in DataContext.Get<UserRole>()
                    join u in DataContext.Get<User>() on ur.UserId equals u.UserId into userInfo
                    from uri in userInfo.DefaultIfEmpty()
                    where ur.RoleId == roleId select uri).AsEnumerable();
        }

        public UserRole GetDetails(Guid userId, Guid roleId)
        {
            return (from p in DataContext.Get<UserRole>() where p.UserId == userId && p.RoleId == roleId select p).FirstOrDefault();
        }

        public bool HasDataExisted(Guid userId, Guid roleId)
        {
            var result = (from p in DataContext.Get<UserRole>()
                          where p.UserId == userId && p.RoleId == roleId
                          select p).FirstOrDefault();
            return result != null;
        }
    }
}
