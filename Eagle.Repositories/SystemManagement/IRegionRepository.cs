using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IRegionRepository: IRepositoryBase<Region>
    {
        IEnumerable<Region> GetList(ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        SelectList PopulateRegionSelectList(int? provinceId, bool? status = null, int? selectedValue = null, bool? isShowSelectText = true);

        bool HasDataExisted(string regionName);

    }
}
