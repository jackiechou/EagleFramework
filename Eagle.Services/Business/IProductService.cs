using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Entities.Business.Products;

namespace Eagle.Services.Business
{
    public interface IProductService : IBaseService
    {
        #region Product Category

        IEnumerable<ProductCategoryDetail> GetProductCategories(string languageCode, int vendorId, ProductCategoryStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<ProductCategoryDetail> GetList(int vendorId, string languageCode, Guid categoryCode, ProductCategoryStatus? status,
            ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<TreeDetail> GetProductCategorySelectTree(ProductCategoryStatus? status, int? selectedId=null,
            bool? isRootShowed = false);
        ProductCategoryDetail GetProductCategoryDetails(int categoryId);
        ProductCategoryDetail GetProductCategoryDetailsByCategoryCode(string categoryCode);
        void InsertProductCategory(Guid userId, int vendorId, string languageCode, ProductCategoryEntry entry);
        void UpdateProductCategory(Guid userId, ProductCategoryEditEntry entry);
        void UpdateProductCategoryStatus(Guid userId, int id, ProductCategoryStatus status);
        void UpdateProductCategoryListOrder(Guid userId, int id, int listOrder);
        #endregion

        #region Product

        IEnumerable<ProductDetail> GetProductList(int vendorId, string languageCode, ProductSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<ProductInfoDetail> GetDiscountedProducts(int vendorId, int? categoryId, ProductStatus? status,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<ProductInfoDetail> GetLastestProducts(int vendorId, int? categoryId, ProductStatus? status,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<ProductInfoDetail> GetProductsByManufacturer(int vendorId, int manufacturerId, ProductStatus? status,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<ProductInfoDetail> GetProducts(ProductSearchEntry entry, string languageCode, int vendorId, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        ProductDetail GetProductDetail(int productId);
        ProductInfoDetail GetProductDetails(int productId);
        ProductInfoDetail GetDetailsByProductCode(string productCode);
        string GenerateProductCode(int maxLetters);
        string GenerateProductCode(int maxLetters, string seedId);
        ProductDetail InsertProduct(Guid applicationId, Guid userId, int vendorId, string languageCode, ProductEntry entry);
        void UpdateProduct(Guid applicationId, Guid userId, int vendorId, string languageCode, ProductEditEntry entry);
        void UpdateProductStatus(int id, ProductStatus status);
        void UpdateUnitsOnOrder(int id, int unitsOnOrder);
        void UpdateProductTotalViews(int id);

        void DeleteProduct(int id);

        #endregion

        #region Product Type

        IEnumerable<ProductTypeDetail> GeProductTypes(int vendorId, ProductTypeSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        SelectList PoplulateProductTypeSelectList(int categoryId, ProductTypeStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false);
        ProductTypeDetail GetProductTypeDetails(int productTypeId);
        void InsertProductType(int vendorId, ProductTypeEntry entry);
        void UpdateProductType(int vendorId, ProductTypeEditEntry entry);
        void UpdateProductTypeStatus(int id, ProductTypeStatus status);
        void UpdateProductTypeListOrder(int id, int listOrder);
        void UpdateProductTypeOrder(int currentProductTypeId, bool isUp);

        #endregion

        #region Product Tax Rate

        IEnumerable<ProductTaxRateDetail> GeProductTaxRates(ProductTaxRateSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        SelectList PopulateProductTaxRateSelectList(ProductTaxRateStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false);
        ProductTaxRateDetail GetProductTaxRateDetails(int productTaxRateId);
        void InsertProductTaxRate(ProductTaxRateEntry entry);
        void UpdateProductTaxRate(ProductTaxRateEditEntry entry);
        void UpdateProductTaxRateStatus(int id, ProductTaxRateStatus status);

        #endregion
        
        #region Product Attribute

        IEnumerable<ProductAttributeDetail> GeAttributes(int productId);
        ProductAttributeDetail GetProductAttributeDetails(int productAttributeId);
        void InsertProductAttribute(int productId, ProductAttributeEntry entry);
        void InsertProductAttributes(int productId, List<ProductAttributeEntry> entries);
        void UpdateProductAttribute(int productId, ProductAttributeEditEntry entry);
        void UpdateProductAttributes(int productId, List<ProductAttributeEditEntry> entries);
        void UpdateProductAttributeStatus(int id, ProductAttributeStatus status);

        #endregion

        #region Attribute
        IEnumerable<AttributeDetail> GetAttributes(AttributeDetail filter, ref int? recordCount, string orderBy, int? page, int defaultPageSize);
        IEnumerable<AttributeDetail> GetAttributes(int productId);
        AttributeDetail GetAttributeDetails(int productAttributeId);
        void InsertAttribute(int cagogoryId, AttributeEntry entry);
        void InsertAttributes(int cagogoryId, List<AttributeEntry> entries);
        void UpdateAttribute(int cagogoryId, AttributeEditEntry entry);
        void UpdateAttributes(int cagogoryId, List<AttributeEditEntry> entries);
        void UpdateAttributeStatus(int id, ProductAttributeStatus status);

        #endregion

        #region Attribute Option

        IEnumerable<AttributeOptionDetail> GetAttributeOptions(int attributeId);
        AttributeOptionDetail GetAttributeOptionDetails(int attributeOptionId);
        void InsertAttributeOption(int attributeId, AttributeOptionEntry entry);
        void UpdateAttributeOption(AttributeOptionEditEntry entry);
        void UpdateAttributeOptionStatus(int id, ProductAttributeOptionStatus status);

        #endregion

        #region Product Attribute Option

        IEnumerable<ProductAttributeOptionDetail> GeAttributeOptions(int attributeId);
        ProductAttributeOptionDetail GetProductAttributeOptionDetails(int productAttributeOptionId);
        void InsertProductAttributeOption(int attributeId, ProductAttributeOptionEntry entry);
        void UpdateProductAttributeOption(ProductAttributeOptionEditEntry entry);
        void UpdateProductAttributeOptionStatus(int id, ProductAttributeOptionStatus status);

        #endregion

        #region Product Discount

        SelectList PopulateProductDiscountSelectList(DiscountType type, ProductDiscountStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false);
        IEnumerable<ProductDiscountDetail> GeProductDiscounts(int vendorId, ProductDiscountSearchEntry filter, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null);
        ProductDiscountDetail GetProductDiscountDetails(int discountId);
        void InsertProductDiscount(Guid userId, int vendorId, ProductDiscountEntry entry);
        void UpdateProductDiscount(Guid userId, int vendorId, ProductDiscountEditEntry entry);
        void UpdateProductDiscountStatus(Guid userId, int id, ProductDiscountStatus status);

        #endregion

        #region Product File

        IEnumerable<ProductFileDetail> GetList(ProductFileStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<ProductFileDetail> GetProductFiles(int productId, ProductFileStatus? status);
        ProductFileDetail GetProductFileDetails(int fileId);
        void InsertProductFile(int vendorId, ProductFileEntry entry);
        void UpdateProductFile(int id, ProductFileEntry entry);
        void UpdateProductFileStatus(int id, ProductFileStatus status);

        #endregion

        #region Product Comment

        IEnumerable<ProductCommentDetail> GeProductComments(ProductCommentSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<ProductCommentDetail> GeProductComments(int productId, ProductCommentStatus? status);
        ProductCommentDetail GetProductCommentDetails(int commentId);
        void InsertProductComment(ProductCommentEntry entry);

        void UpdateProductCommentStatus(int id, ProductCommentStatus status);

        #endregion

        #region Product Rating

        int GetDefaultProductRating(Guid applicationId);
        IEnumerable<ProductRatingDetail> GetProductRatings(int productId);

        decimal InsertProductRating(ProductRatingEntry entry);

        #endregion

        #region ProductAlbum
        IEnumerable<ProductAlbumDetail> GetProductAlbum(int productId);
        void DeleteImageProductAlbum(int productId, int fileId);
        #endregion

        #region Product Brand
        SelectList PopulateProductBrandSelectList(BrandStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false);
        #endregion Product Brand
    }
}
