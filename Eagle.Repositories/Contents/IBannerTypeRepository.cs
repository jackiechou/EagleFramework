using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Contents.Banners;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IBannerTypeRepository : IRepositoryBase<BannerType>
    {
        IEnumerable<BannerType> GetActiveList();
        IEnumerable<BannerType> GetList(ref int? recordCount, int? page = null, int? pageSize = null);
        SelectList PopulateBannerTypeSelectList(int? selectedValue, bool? isShowSelectText);
        bool IsIdExisted(int id);
        bool HasDataExisted(string bannerTypeName);
    }
}
