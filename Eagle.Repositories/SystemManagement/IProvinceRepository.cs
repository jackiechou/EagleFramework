using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IProvinceRepository: IRepositoryBase<Province>
    {
        IEnumerable<Province> GetList(ref int? recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);

        SelectList PopulateProvinceSelectList(int countryId, bool? status=null, int? selectedValue=null,
            bool? isShowSelectText = true);
        bool HasDataExisted(string provinceName);
    }
}
