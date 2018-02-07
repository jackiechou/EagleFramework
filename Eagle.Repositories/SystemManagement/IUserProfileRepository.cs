using System;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IUserProfileRepository : IRepositoryBase<UserProfile>
    {
        UserProfileInfo GetProfile(Guid userId);
        UserProfile GetDetails(Guid userId);
        UserProfile GetDetails(Guid userId, int contactId);
        bool HasDataExisted(Guid userId, int contactId);
    }
}
