using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Business;

namespace Eagle.Services.Business
{
    public interface IBrandService: IBaseService
    {
        void DeleteBrand(int brandId);
        void UpdateBrand(BrandEditEntry brand);
        void UpdateStatus(int brandId, BrandStatus status);
        void CreateBrand(BrandEntry entry);
        IEnumerable<BrandInfo> GetBrandList(BrandSearchEntry brandSearchEntry, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        BrandDetail GetBrandDetail(int brandId);
    }
}
