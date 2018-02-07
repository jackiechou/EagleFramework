using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Booking;

namespace Eagle.Services.Service
{
    public interface IBookingService : IBaseService
    {
        #region Service Pack
        IEnumerable<ServicePackInfoDetail> GetServicePacks(ServicePackSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<ServicePackInfoDetail> GetServicePacks(int categoryId, ServicePackStatus? status);
        IEnumerable<ServicePackInfoDetail> GetServicePacks(int typeId, int categoryId, ServicePackStatus? status);
        IEnumerable<ServicePackInfoDetail> GetDiscountedServicePackages(int? categoryId, ServicePackStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<ServicePackInfoDetail> GetServicePacks(int categoryId, ServicePackStatus? status,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        ServicePackInfoDetail GetServicePackDetail(int id);
        SelectList PopulateServicePackStatus(bool? selectedValue = true, bool isShowSelectText = false);

        SelectList PopulateServicePackSelectList(ServicePackStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);

        void DeleteServicePack(int packageId);
        ServicePackDetail InsertServicePack(Guid applicationId, Guid userId, ServicePackEntry entry);
        void UpdateServicePack(Guid applicationId, Guid userId, ServicePackEditEntry entry);
        void UpdateServicePackStatus(int id, ServicePackStatus status);
        void UpdateServicePackListOrder(int id, int listOrder);
        void UpdateServicePackTotalViews(int id);

        #endregion

        #region ServicePackType

        IEnumerable<ServicePackTypeDetail> GetServicePackTypes(ServicePackTypeStatus? status);
        SelectList PopulateServicePackTypeSelectList(ServicePackTypeStatus? status, int? selectedValue = null,
            bool isShowSelectText = false);
        ServicePackTypeDetail InsertServicePackType(ServicePackTypeEntry entry);
        void UpdateServicePackType(ServicePackTypeEditEntry entry);
        void UpdateServicePackTypeStatus(int id, ServicePackTypeStatus status);

        #endregion

        #region Service Pack Option

        IEnumerable<ServicePackOptionDetail> GeServicePackOptions(int packageId);

        ServicePackOptionDetail GetServicePackOptionDetails(int optionId);

        void InsertServicePackOption(int packageId, ServicePackOptionEntry entry);

        void InsertServicePackOptions(int packageId, List<ServicePackOptionEntry> entries);

        void UpdateServicePackOption(ServicePackOptionEditEntry entry);

        void UpdateServicePackOption(int packageId, List<ServicePackOptionEditEntry> entries);

        void UpdateServicePackOptionStatus(int id, ServicePackOptionStatus status);

        #endregion

        #region Service Pack Duration 

        IEnumerable<ServicePackDurationDetail> GetServicePackDurations(ServicePackDurationSearchEntry filter,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
       

        ServicePackDurationDetail GetServicePackDurationDetail(int id);

        ServicePackDurationDetail GetDurationByServicePackId(int packageId);

        SelectList PopulateServicePackDurationStatus(bool? selectedValue = true, bool isShowSelectText = false);

        SelectList PopulateServicePackDurationSelectList(bool? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);


        ServicePackDurationDetail InsertServicePackDuration(ServicePackDurationEntry entry);

        void UpdateServicePackDuration(ServicePackDurationEditEntry entry);

        void UpdateServicePackDurationStatus(int id, bool status);

        #endregion

        #region Service Pack Provider - Employees by Package

        List<EmployeeInfoDetail> GetServicePackProviders(int packageId, EmployeeStatus? status=null);
        SelectList PopulateProviderSelectList(int packageId, EmployeeStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = true);
        MultiSelectList PopulateAvailableProviderMultiSelectList(int? packageId = null,
            EmployeeStatus? status = null, string[] selectedValues = null);
        MultiSelectList PopulateSelectedProviderMultiSelectList(int? packageId = null, EmployeeStatus? status = null,
            string[] selectedValues = null);

        void InsertServicePackProvider(ServicePackProviderEntry entry);
        #endregion

        #region Service Pack Rating

        int GetDefaultServicePackRating(Guid applicationId);
        IEnumerable<ServicePackRatingDetail> GetServicePackRatings(int packageId);
        decimal InsertServicePackRating(ServicePackRatingEntry entry);

        #endregion
        #region Service Tax Rate

        IEnumerable<ServiceTaxRateDetail> GeServiceTaxRates(ServiceTaxRateSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        SelectList PopulateServiceTaxRateSelectList(bool? status = null,
            int? selectedValue = null, bool? isShowSelectText = true);

        ServiceTaxRateDetail GetServiceTaxRateDetails(int serviceTaxRateId);

        void InsertServiceTaxRate(ServiceTaxRateEntry entry);

        void UpdateServiceTaxRate(ServiceTaxRateEditEntry entry);

        void UpdateServiceTaxRateStatus(int id, bool status);
        #endregion

        #region Service Period
        IEnumerable<ServicePeriodDetail> GetServicePeriods(bool? status, ref int? recordCount, string orderBy = null,
           int? page = null, int? pageSize = null);

        ServicePeriodDetail GetServicePeriodDetail(int id);
        SelectList PopulatePeriod(int? selectedValue = null, bool isShowSelectText = true);
        SelectList PopulateServicePeriodStatus(bool? selectedValue = true, bool isShowSelectText = false);
        ServicePeriodDetail InsertServicePeriod(ServicePeriodEntry entry);
        void UpdateServicePeriod(ServicePeriodEditEntry entry);
        void UpdateServicePeriodStatus(int id, bool status);

        #endregion

        #region Service Booking

        List<CalendarEventItem> GetCalendarEvents(int vendorId, ItemType type, DateTime? startDate, DateTime? endDate,
            OrderProductStatus? status=null);
        void BookingSinglePackagesToCart(Guid applicationId, BookingSingleKindEntry entry);
        void BookingFullPackagesToCart(Guid applicationId, BookingPackageKindEntry entry);

        #endregion

        #region Service Discount

        IEnumerable<ServiceDiscountDetail> GeServiceDiscounts(ServiceDiscountSearchEntry filter,
            ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        SelectList PopulateServiceDiscountSelectList(ServiceDiscountStatus? status = null, int? selectedValue = null,
          bool? isShowSelectText = true);
        SelectList PopulateServiceDiscountSelectList(DiscountType type, ServiceDiscountStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = true);
        SelectList PopulateServicePackSelectListNotCode(ServicePackStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true);
        SelectList PopulateServicePackSelectListByCode(string discountCode, ServicePackStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);
        SelectList PopulateServicePackSelectListByCateId(int cateId, ServicePackStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);
        ServiceDiscountDetail GetServiceDiscountDetails(int discountId);

        void InsertServiceDiscount(ServiceDiscountEntry entry);

        void UpdateServiceDiscount(ServiceDiscountEditEntry entry);

        void UpdateServiceDiscountStatus(int id, ServiceDiscountStatus status);

        #endregion

        #region Service Category

        IEnumerable<TreeDetail> GetServiceCategorySelectTree(ServiceType typeId = ServiceType.Single,
            ServiceCategoryStatus? status = null, int? selectedId=null, bool? isRootShowed = false);
        IEnumerable<TreeDetail> GetServiceCategoryTree(ServiceCategoryStatus? status, int? selectedId = null, bool? isRootShowed = true);
        ServiceCategoryDetail GetServiceCategoryDetail(int id);
        SelectList GetServiceCategoryChildList(ServiceCategoryStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true);
        SelectList GetServiceCategoryChildListByCode(string discountCode, ServiceCategoryStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true);
        void InsertServiceCategory(Guid userId, ServiceCategoryEntry entry);
        void UpdateServiceCategory(Guid userId, ServiceCategoryEditEntry entry);
        void UpdateServiceCategoryStatus(Guid userId, int id, ServiceCategoryStatus status);
        void UpdateServiceCategoryListOrder(Guid userId, int id, int listOrder);
        void DeleteServiceCategory(int categoryId);

        #endregion
    }
}
