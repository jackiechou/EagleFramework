using System.Collections.Generic;
using Eagle.Entities.Business.Vendors;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Vendor
{
    public interface IVendorCurrencyRepository: IRepositoryBase<VendorCurrency>
    {
        IEnumerable<VendorCurrency> GetList(int vendorId, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        VendorCurrency GetDetails(int vendorId, string currencyCode);
        bool HasDataExisted(int vendorId, string currencyCode);
    }
}
