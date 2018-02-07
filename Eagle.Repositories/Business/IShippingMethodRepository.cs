using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Business.Shipping;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IShippingMethodRepository: IRepositoryBase<ShippingMethod>
    {
        IEnumerable<ShippingMethod> GetShippingMethods(string shippingMethodName, bool? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<ShippingMethod> GetShippingMethods(bool? status);
        int GetNewListOrder();
        bool HasDataExisted(string shippingMethodName);
        SelectList PopulateShippingMethodStatus(bool? selectedValue = true, bool isShowSelectText = false);
        SelectList PopulateShippingMethodSelectList(bool? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);
    }
}
