using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Entities.Business.Vendors;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Vendor
{
    public class VendorTransactionMethodRepository : RepositoryBase<VendorTransactionMethod>, IVendorTransactionMethodRepository
    {
        public VendorTransactionMethodRepository(IDataContext dataContext) : base(dataContext) { }
    }
}
