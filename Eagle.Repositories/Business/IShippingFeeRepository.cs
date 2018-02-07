using Eagle.Entities.Business.Shipping;
using Eagle.EntityFramework.Repositories;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Eagle.Repositories.Business
{
    public interface IShippingFeeRepository : IRepositoryBase<ShippingFee>
    {
        IEnumerable<ShippingFee> GetShippingFees(string shippingFeeName, bool? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        ShippingFee GetShippingFee(int shippingCarrierId, int shippingMethodId, string zipCode, decimal weight);
        int GetNewListOrder();
        bool HasDataExisted(string shippingFeeName);
        SelectList PopulateShippingFeeStatus(bool? selectedValue = null, bool isShowSelectText = true);
        IEnumerable<int> GetShippingCarrierProvicedService(int shippingMethodId, bool? status);
    }
}
