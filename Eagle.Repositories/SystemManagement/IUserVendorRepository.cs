using System;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IUserVendorRepository : IRepositoryBase<UserVendor>
    {
        UserVendor GetDetails(Guid userId,int vendorId);
        bool HasDataExisted(Guid userId, int vendorId);
    }
}
