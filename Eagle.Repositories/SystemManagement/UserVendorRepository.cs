using System;
using System.Linq;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class UserVendorRepository : RepositoryBase<UserVendor>, IUserVendorRepository
    {
        public UserVendorRepository(IDataContext dataContext) : base(dataContext) { }

        public UserVendor GetDetails(Guid userId, int vendorId)
        {
            return (from p in DataContext.Get<UserVendor>() where p.UserId == userId && p.VendorId == vendorId select p).FirstOrDefault();
        }

        public bool HasDataExisted(Guid userId, int vendorId)
        {
            var result = (from p in DataContext.Get<UserVendor>()
                          where p.UserId == userId && p.VendorId == vendorId
                          select p).FirstOrDefault();
            return result != null;
        }
    }
}
