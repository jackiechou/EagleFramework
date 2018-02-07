using System.Collections.Generic;
using Eagle.Entities.Business.Brand;
using Eagle.EntityFramework.Repositories;
using System.Web.Mvc;
using Eagle.Core.Settings;

namespace Eagle.Repositories.Business
{
    public interface IBrandRepository : IRepositoryBase<Brand>
    {
        IEnumerable<Brand> GetBrandList(string searchText, bool? isOnline, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        bool CheckExistedName(string brandName);
        SelectList PopulateProductBrandSelectList(BrandStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false);
    }
}
