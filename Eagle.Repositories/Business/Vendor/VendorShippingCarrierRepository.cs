using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Entities.Business.Vendors;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Vendor
{
    public class VendorShippingCarrierRepository : RepositoryBase<VendorShippingCarrier>, IVendorShippingCarrierRepository
    {
        public VendorShippingCarrierRepository(IDataContext dataContext) : base(dataContext) { }
    }
}
