using System.Collections.Generic;
using Eagle.Entities.Business.Vendors;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Vendor
{
    public interface IVendorAddressRepository: IRepositoryBase<VendorAddress>
    {
        IEnumerable<VendorAddress> GetVendorAddresses(int vendorId);
        VendorAddress GetDetails(int vendorId, int addressId);
        bool HasDataExisted(int vendorId, int addressId);
    }
}
