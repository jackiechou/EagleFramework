using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Vendor
{
    public class VendorRepository : RepositoryBase<Entities.Business.Vendors.Vendor>, IVendorRepository
    {
        public VendorRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<Entities.Business.Vendors.Vendor> GetVendors(string searchText, VendorStatus? status, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = DataContext.Get<Entities.Business.Vendors.Vendor>().Where(x =>
            (status == null || x.IsAuthorized == status));
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.VendorName.ToLower().Contains(searchText)
                                         || x.StoreName.ToLower().Contains(searchText));
            }
            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public Entities.Business.Vendors.Vendor FindByVendorName(string vendorName)
        {
            return DataContext.Get<Entities.Business.Vendors.Vendor>().FirstOrDefault(x => x.VendorName.ToLower() == vendorName.ToLower());
        }
        public bool HasDataExisted(string vendorName)
        {
            var entity =
                DataContext.Get<Entities.Business.Vendors.Vendor>().FirstOrDefault(x => x.VendorName.ToLower() == vendorName.ToLower());
            return entity != null;
        }

        public bool HasEmailExisted(string email)
        {
            var entity = (from v in DataContext.Get<Entities.Business.Vendors.Vendor>()
                          where v.Email == email
                          select v).FirstOrDefault();
            return entity != null;
        }
        public int GetNewClickThrough()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<Entities.Business.Vendors.Vendor>() select (int)u.ClickThroughs;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
    }
}
