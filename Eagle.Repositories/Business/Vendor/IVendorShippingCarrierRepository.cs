using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Eagle.Entities.Business.Vendors;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Vendor
{
    public interface IVendorShippingCarrierRepository : IRepositoryBase<VendorShippingCarrier>
    {
        //IEnumerable<VendorShippingCarrier> GetShippingCarriers(string searchText, bool? status, ref int? recordCount,
        //    string orderBy = null, int? page = null, int? pageSize = null);
        //VendorShippingCarrier FindByPartnerName(string partnerName);
        //bool HasDataExisted(string partnerName);
        //bool HasEmailExisted(string email);
        //SelectList PopulateShippingCarriers(bool? selectedValue = true, bool isShowSelectText = false);
    }
}
