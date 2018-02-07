using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Business;

namespace Eagle.Services.Business
{
    public interface ISupplierService: IBaseService
    {
        #region Manufacturer

        IEnumerable<ManufacturerDetail> GetManufacturers(int vendorId, ManufacturerSearchEntry filter,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<ManufacturerInfoDetail> GetManufacturerList(int vendorId, ManufacturerStatus? status,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        ManufacturerDetail GetManufacturerDetail(int id);
        void InsertManufacturer(Guid applicationId, Guid userId, int vendorId, ManufacturerEntry entry);
        void UpdateManufacturer(Guid applicationId, Guid userId, int vendorId, ManufacturerEditEntry entry);
        void UpdateManufacturerStatus(int id, ManufacturerStatus status);
        #endregion

        #region Manufacturer Category
        IEnumerable<ManufacturerCategoryDetail> GeManufacturerCategories(int vendorId, ManufacturerCategorySearchEntry filter,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        ManufacturerCategoryDetail GetManufacturerCategoryDetail(int id);

        SelectList PoplulateManufacturerCategorySelectList(ManufacturerCategoryStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false);
        void InsertManufacturerCategory(int vendorId, ManufacturerCategoryEntry entry);
        void UpdateManufacturerCategory(ManufacturerCategoryEditEntry entry);
        void UpdateManufacturerCategoryStatus(int id, ManufacturerCategoryStatus status);
        #endregion

    }
}
