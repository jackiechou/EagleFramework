using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Manufacturers;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IManufacturerCategoryRepository : IRepositoryBase<ManufacturerCategory>
    {
        IEnumerable<ManufacturerCategory> GetManufacturerCategories(int vendorId, string searchText, ManufacturerCategoryStatus? status,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        ManufacturerCategory GetDetails(int manufacturerCategoryId);

        SelectList PopulateManufacturerCategorySelectList(ManufacturerCategoryStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false);
    }
}
