using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IProductRepository: IRepositoryBase<Product>
    {
        IEnumerable<Product> GetProductList(int vendorId, string languageCode, string searchText, int? categoryId,
            DateTime? startDate, DateTime? endDate,
            ProductStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<ProductInfo> GetDiscountedProducts(int vendorId, int? categoryId, ProductStatus? status,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<ProductInfo> GetLastestProducts(int vendorId, int? categoryId, ProductStatus? status,
        ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<ProductInfo> GetProductsByManufacturer(int vendorId, int manufacturerId, ProductStatus? status,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<ProductInfo> GetProducts(string languageCode, int vendorId, int? categoryId, string searchText, 
            DateTime? startDate, DateTime? endDate, decimal? minPrice, decimal? maxPrice,
            ProductStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        ProductInfo GetDetailsByProductId(int productId);
        ProductInfo GetDetailsByProductCode(string productCode);
        int GetNewListOrder();
        bool HasDataExisted(int? productTypeId, int categoryId, string productCode, string productName);
        bool HasProductNameExisted(string productName);
        bool HasProductCodeExisted(string productCode);
        string GenerateProductCode(int maxLetters);
        string GenerateProductCode(int maxLetters, string seedId);
        bool HasBrandExisted(int brandId);
    }
}
