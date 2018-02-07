using Eagle.Entities.Business.Shipping;
using Eagle.EntityFramework.Repositories;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Eagle.Repositories.Business
{
    public interface IShippingCarrierRepository : IRepositoryBase<ShippingCarrier>
    {
        ShippingCarrier GetSelectedShippingCarrier(int vendorId);
        IEnumerable<ShippingCarrier> GetShippingCarriers(int vendorId, string shippingCarrierName, bool? status, ref int? recordCount,
               string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<ShippingCarrier> GetShippingCarriers(int vendorId, bool? status);
        int GetNewListOrder();
        bool HasDataExisted(int vendorId, string shippingCarrierName);
        SelectList PopulateShippingCarrierStatus(bool? selectedValue = null, bool isShowSelectText = true);
        SelectList PopulateShippingCarrierSelectList(bool? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);
        IEnumerable<ShippingCarrier> GetShippingCarriersByIds(IEnumerable<int> ids, bool? status);
    }
}
