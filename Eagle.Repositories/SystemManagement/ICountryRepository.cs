using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface ICountryRepository : IRepositoryBase<Country>
    {
        IEnumerable<Country> GetList(ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        SelectList PopulateCountrySelectList(bool? status=null, int? selectedValue = null, bool? isShowSelectText = false);
    }
}
