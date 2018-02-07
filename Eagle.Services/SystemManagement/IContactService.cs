using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Identity;

namespace Eagle.Services.SystemManagement
{
    public interface IContactService : IBaseService
    {
        #region Addresss----------------------------------------------------------------------------------------------

        IEnumerable<UserAddressInfoDetail> GetUserAddresses(Guid userId);
        IEnumerable<AddressDetail> GetAddresses(out int recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);
        AddressInfoDetail GetAddressInfoDetail(int addressId);
        AddressDetail GetAddressDetails(int id);
        AddressDetail InsertAddress(AddressEntry entry);
        void UpdateAddress(AddressEditEntry entry);
        void DeleteAddress(int id);
        int? UpdateEmergencyAddress(EmergencyAddressEditEntry entry);
        int? UpdatePermanentAddress(PermanentAddressEditEntry entry);

        #endregion --------------------------------------------------------------------------------------------------

      
        #region Contact ----------------------------------------------------------------------------------------------

        IEnumerable<ContactDetail> GetContacts(out int recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);

        ContactDetail GetContactDetails(int id);
        ContactInfoDetail GetContactInfoDetails(int id);
        ContactDetail InsertContact(ContactEntry entry);
        void UpdateContact(ContactEditEntry entry);
        void UpdateContactPhoto(int id, int photo);
        void UpdateContactStatus(int id, bool status);

        #endregion ----------------------------------------------------------------------------------------------

        #region Country----------------------------------------------------------------------------------------------

        IEnumerable<CountryDetail> GetCountries(out int recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);
        CountryDetail GetCountryDetails(int id);
        SelectList PopulateCountrySelectList(bool? status = true, int? selectedValue=null, bool isShowSelectText = true);
        int? InsertCountry(CountryEntry entry);
        void UpdateCountry(int id, CountryEntry entry);
        void UpdateCountryStatus(int id, bool status);

        #endregion --------------------------------------------------------------------------------------------------



        #region City - Province -------------------------------------------------------------------------------------

        IEnumerable<ProvinceDetail> GetProvinces(out int recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);
        ProvinceDetail GetProvinceDetails(int id);
        SelectList PopulateProvinceSelectList(int countryId, bool? status = true, int? selectedValue = null, bool? isShowSelectText = true);
        int? InsertProvince(ProvinceEntry entry);
        void UpdateProvince(int id, ProvinceEntry entry);
        void UpdateProvinceStatus(int id, bool status);

        #endregion ------------------------------------------------------------------------------------------------

        #region District - Region ---------------------------------------------------------------------------------

        IEnumerable<RegionDetail> GetRegions(out int recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);
        RegionDetail GetRegionDetails(int id);
        SelectList PopulateRegionSelectList(int? provinceId, bool? status = true, int? selectedValue=null, bool? isShowSelectText = true);
        int? InsertRegion(RegionEntry entry);
        void UpdateRegion(int id, RegionEntry entry);
        void UpdateRegionStatus(int id, bool status);

        #endregion -----------------------------------------------------------------------------------------------


    }
}
