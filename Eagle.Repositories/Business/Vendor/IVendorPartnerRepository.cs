using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Business.Vendors;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Vendor
{
    public interface IVendorPartnerRepository : IRepositoryBase<VendorPartner>
    {
        IEnumerable<VendorPartner> GetPartners(string searchText, bool? status, ref int? recordCount,
             string orderBy = null, int? page = null, int? pageSize = null);
        VendorPartner FindByPartnerName(string partnerName);
        bool HasDataExisted(string partnerName);
        bool HasEmailExisted(string email);
        SelectList PopulatePartnerStatus(bool? selectedValue = true, bool isShowSelectText = false);
    }
}
