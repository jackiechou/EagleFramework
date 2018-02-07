using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Business.Vendors;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Vendor
{
    public class VendorAddressRepository : RepositoryBase<VendorAddress>, IVendorAddressRepository
    {
        public VendorAddressRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<VendorAddress> GetVendorAddresses(int vendorId)
        {
            return (from v in DataContext.Get<VendorAddress>()
                    where v.VendorId == vendorId select v).AsEnumerable();
        }
        public VendorAddress GetDetails(int vendorId, int addressId)
        {
            return (from p in DataContext.Get<VendorAddress>() where p.VendorId == vendorId && p.AddressId == addressId select p).FirstOrDefault();
        }

        public bool HasDataExisted(int vendorId, int addressId)
        {
            var result = (from p in DataContext.Get<VendorAddress>()
                          where p.VendorId == vendorId && p.AddressId == addressId
                          select p).FirstOrDefault();
            return result != null;
        }
    }
}
