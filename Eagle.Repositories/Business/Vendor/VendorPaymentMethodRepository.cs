using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Entities.Business.Vendors;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Vendor
{
    public class VendorPaymentMethodRepository : RepositoryBase<VendorPaymentMethod>, IVendorPaymentMethodRepository
    {
        public VendorPaymentMethodRepository(IDataContext dataContext) : base(dataContext) { }
    }
}
