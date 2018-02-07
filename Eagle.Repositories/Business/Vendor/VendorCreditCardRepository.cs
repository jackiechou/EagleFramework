using Eagle.Entities.Business.Vendors;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Vendor
{
    public class VendorCreditCardRepository : RepositoryBase<VendorCreditCard>, IVendorCreditCardRepository
    {
        public VendorCreditCardRepository(IDataContext dataContext) : base(dataContext) { }
    }
}
