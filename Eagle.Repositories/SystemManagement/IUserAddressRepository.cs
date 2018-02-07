using System;
using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IUserAddressRepository : IRepositoryBase<UserAddress>
    {
        IEnumerable<UserAddressInfo> GetList(Guid userId);
        UserAddress GetDetails(Guid userId, int addressId);
        bool HasDataExisted(Guid userId, int addressId);
    }
}
