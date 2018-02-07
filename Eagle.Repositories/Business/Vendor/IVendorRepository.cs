using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Vendor
{
   public interface IVendorRepository : IRepositoryBase<Entities.Business.Vendors.Vendor>
   {
       IEnumerable<Entities.Business.Vendors.Vendor> GetVendors(string searchText, VendorStatus? status, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null);
       Entities.Business.Vendors.Vendor FindByVendorName(string vendorName);
       bool HasDataExisted(string vendorName);
       bool HasEmailExisted(string email);
       int GetNewClickThrough();
   }
}
