using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Business;

namespace Eagle.Services.Business
{
    public interface IVendorService: IBaseService
    {
        #region Vendor
        IEnumerable<VendorDetail> GetVendors(VendorSearchEntry filter, ref int? recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);

        VendorDetail GetVendorDetail(int id);
        VendorInfoDetail GetDefaultVendor();
        void InsertVendor(Guid applicationId, Guid userId, VendorEntry entry);
        void UpdateVendor(Guid applicationId, Guid userId, VendorEditEntry entry);
        void UpdateClickThroughs(Guid userId, int vendorId);
        void UpdateVendorStatus(Guid userId, int id, VendorStatus status);
        #endregion

        #region Vendor Address

        List<VendorAddressDetail> GetVendorAddresses(int vendorId);

        #endregion

        #region Vendor Partner

        IEnumerable<VendorPartnerInfoDetail> GetPartners(VendorPartnerSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        VendorPartnerDetail GetPartnerDetail(int id);
        SelectList PopulatePartnerStatus(bool? selectedValue = true, bool isShowSelectText = false);
        void InsertPartner(Guid applicationId, Guid userId, int vendorId, VendorPartnerEntry entry);
        void UpdatePartner(Guid applicationId, Guid userId, VendorPartnerEditEntry entry);
        void UpdatePartnerStatus(int id, bool status);

        #endregion


    }
}