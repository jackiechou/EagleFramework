using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Entities.Business.Vendors;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Vendor
{
    public interface IVendorShippingMethodRepository : IRepositoryBase<VendorShippingMethod>
    {
        //IEnumerable<VendorShippingMethod> GetVendorShippingMethods(int vendorId);
        //VendorShippingMethod GetDetails(int vendorId, int addressId);
        //bool HasDataExisted(int vendorId, int addressId);
    }
}
