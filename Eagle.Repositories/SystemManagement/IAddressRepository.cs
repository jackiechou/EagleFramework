using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IAddressRepository : IRepositoryBase<Address>
    {
        AddressInfo GetDetails(int addressId);
    }
}
