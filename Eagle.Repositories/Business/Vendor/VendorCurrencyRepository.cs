using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Entities.Business.Vendors;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Vendor
{
    public class VendorCurrencyRepository : RepositoryBase<VendorCurrency>, IVendorCurrencyRepository
    {
        public VendorCurrencyRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<VendorCurrency> GetList(int vendorId, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = DataContext.Get<VendorCurrency>().Where(x => x.VendorId == vendorId);
            return lst.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public VendorCurrency GetDetails(int vendorId, string currencyCode)
        {
            return (from p in DataContext.Get<VendorCurrency>() where p.VendorId == vendorId && p.CurrencyCode == currencyCode select p).FirstOrDefault();
        }

        public bool HasDataExisted(int vendorId, string currencyCode)
        {
            var result = (from p in DataContext.Get<VendorCurrency>()
                          where p.VendorId == vendorId && p.CurrencyCode == currencyCode
                          select p).FirstOrDefault();
            return result != null;
        }
    }
}
