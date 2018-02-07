using System;
using System.Linq;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class UserProfileRepository : RepositoryBase<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(IDataContext dataContext) : base(dataContext) { }
        public UserProfile GetDetails(Guid userId)
        {
            return (from u in DataContext.Get<User>()
                    join up in DataContext.Get<UserProfile>() on u.UserId equals up.UserId into profileInfo
                    from profile in profileInfo.DefaultIfEmpty()
                    join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                    from contact in contactInfo.DefaultIfEmpty()
                    where profile.UserId == userId
                    select profile).FirstOrDefault();
        }
        public UserProfileInfo GetProfile(Guid userId)
        {
            return (from u in DataContext.Get<User>() 
                    join up in DataContext.Get<UserProfile>() on u.UserId equals up.UserId into profileInfo
                    from profile in profileInfo.DefaultIfEmpty()
                    join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                    from contact in contactInfo.DefaultIfEmpty()
                    where profile.UserId == userId
                    select new UserProfileInfo
                    {
                        ProfileId = profile.ProfileId,
                        UserId = profile.UserId,
                        ContactId = profile.ContactId,
                        User = u,
                        Contact = contact
                    }).FirstOrDefault();
        }
        public UserProfile GetDetails(Guid userId, int contactId)
        {
            return (from u in DataContext.Get<User>()
                    join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId
                          where p.UserId == userId && p.ContactId == contactId select p).FirstOrDefault();
        }
        public bool HasDataExisted(Guid userId, int contactId)
        {
            var query = from p in DataContext.Get<UserProfile>()
                where p.UserId == userId && p.ContactId == contactId
                select p;
            return query.Any();
        }
    }
}
