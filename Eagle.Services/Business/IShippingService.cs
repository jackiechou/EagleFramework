using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Business.Transaction;

namespace Eagle.Services.Business
{
    public interface IShippingService : IBaseService
    {
        #region Shipping Method

        IEnumerable<ShippingMethodDetail> GetShippingMethods(ShippingMethodSearchEntry filter,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        List<ShippingMethodInfo> GetShippingMethods(bool? status);
        ShippingMethodDetail GetShippingMethodDetail(int id);
        SelectList PopulateShippingMethodStatus(bool? selectedValue = true, bool isShowSelectText = false);

        SelectList PopulateShippingMethodSelectList(bool? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);
        ShippingMethodDetail InsertShippingMethod(ShippingMethodEntry entry);

        void UpdateShippingMethod(ShippingMethodEditEntry entry);

        void UpdateShippingMethodStatus(int id, bool status);

        void UpdateShippingMethodListOrder(int id, int listOrder);

        #endregion

        #region Shipping Carrier

        ShippingCarrierDetail GetSelectedShippingCarriers(int vendorId);
        IEnumerable<ShippingCarrierDetail> GetShippingCarriers(int vendorId, ShippingCarrierSearchEntry filter,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        ShippingCarrierDetail GetShippingCarrierDetail(int id);
        SelectList PopulateShippingCarrierStatus(bool? selectedValue = null, bool isShowSelectText = true);

        SelectList PopulateShippingCarrierSelectList(bool? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);
        ShippingCarrierDetail InsertShippingCarrier(int vendorId, ShippingCarrierEntry entry);

        void UpdateShippingCarrier(int vendorId, ShippingCarrierEditEntry entry);
        void UpdateSelectedShippingCarrier(int vendorId, int shippingCarrierId);
        void UpdateShippingCarrierStatus(int id, bool status);

        void UpdateShippingCarrierListOrder(int id, int listOrder);
        IEnumerable<ShippingCarrierDetail> GetShippingCarriersByShippingMethodId(int shippingMethodId);

        #endregion

        #region Shipping Fee

        IEnumerable<ShippingFeeDetail> GetShippingFees(ShippingFeeSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        ShippingFeeDetail GetShippingFeeDetail(int id);
        SelectList PopulateShippingFeeStatus(bool? selectedValue = null, bool isShowSelectText = true);
        ShippingFeeDetail InsertShippingFee(ShippingFeeEntry entry);

        void UpdateShippingFee(ShippingFeeEditEntry entry);

        void UpdateShippingFeeStatus(int id, bool status);

        void UpdateShippingFeeListOrder(int id, int listOrder);

        ShippingFeeDetail GetShippingFee(ShippingFeeSearchZipCodeEntry filter);
        ShipmentInfo GetShipmentInfo(ShippingFeeSearchZipCodeEntry filter);

        #endregion
    }
}
