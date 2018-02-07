using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eagle.Common.Extensions;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Manufacturers;
using Eagle.Entities.Business.Products;
using Eagle.Entities.Common;
using Eagle.Repositories;
using Eagle.Services.Business.Validation;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.Business
{
    public class ProductService : BaseService, IProductService
    {
        public IApplicationService ApplicationService { get; set; }
        public IDocumentService DocumentService { get; set; }
        public IVendorService VendorService { get; set; }
        public ICurrencyService CurrencyService { get; set; }
        public ProductService(IUnitOfWork unitOfWork, IApplicationService applicationService, IVendorService vendorService, IDocumentService documentService, ICurrencyService currencyService) : base(unitOfWork)
        {
            ApplicationService = applicationService;
            CurrencyService = currencyService;
            DocumentService = documentService;
            VendorService = vendorService;
        }

        #region Product Category
        public IEnumerable<ProductCategoryDetail> GetProductCategories(string languageCode, int vendorId, ProductCategoryStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ProductCategoryRepository.GetList(languageCode, vendorId, status, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ProductCategory, ProductCategoryDetail>();
        }
        public IEnumerable<ProductCategoryDetail> GetList(int vendorId, string languageCode, Guid categoryCode, ProductCategoryStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ProductCategoryRepository.GetList(languageCode, vendorId, categoryCode, status, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ProductCategory, ProductCategoryDetail>();
        }
        public IEnumerable<TreeDetail> GetProductCategorySelectTree(ProductCategoryStatus? status, int? selectedId = null, bool? isRootShowed = false)
        {
            var lst = UnitOfWork.ProductCategoryRepository.GetProductCategorySelectTree(status, selectedId, isRootShowed);
            return lst.ToDtos<TreeEntity, TreeDetail>();
        }
        public ProductCategoryDetail GetProductCategoryDetails(int categoryId)
        {
            if (categoryId <= 0) return new ProductCategoryDetail();
            var entity = UnitOfWork.ProductCategoryRepository.FindById(categoryId);
            return entity.ToDto<ProductCategory, ProductCategoryDetail>();
        }
        public ProductCategoryDetail GetProductCategoryDetailsByCategoryCode(string categoryCode)
        {
            var entity = UnitOfWork.ProductCategoryRepository.FindByCategoryCode(categoryCode);
            return entity.ToDto<ProductCategory, ProductCategoryDetail>();
        }
        public void InsertProductCategory(Guid userId, int vendorId, string languageCode, ProductCategoryEntry entry)
        {
            ISpecification<ProductCategoryEntry> validator = new ProductCategoryEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<ProductCategoryEntry, ProductCategory>();
            entity.CategoryAlias = StringUtils.GenerateFriendlyString(entry.CategoryName);
            entity.CategoryLink = entry.CategoryLink;
            entity.Icon = entry.Icon;
            entity.BriefDescription = entry.BriefDescription;
            entity.Description = entry.Description;
            entity.Status = entry.Status;
            entity.ViewOrder = UnitOfWork.ProductCategoryRepository.GetNewListOrder();
            entity.LanguageCode = languageCode;
            entity.CreatedByUserId = userId;
            entity.CreatedDate = DateTime.UtcNow;
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.VendorId = vendorId;

            UnitOfWork.ProductCategoryRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.ParentId != null && entry.ParentId > 0)
            {
                var parentEntity = UnitOfWork.ProductCategoryRepository.FindById(Convert.ToInt32(entry.ParentId));
                if (parentEntity == null) return;

                var lineage = $"{parentEntity.Lineage},{entity.CategoryId}";
                entity.Lineage = lineage;
                entity.Depth = lineage.Split(',').ToList().Count();
            }
            else
            {
                entity.Lineage = $"{entity.CategoryId}";
                entity.Depth = 0;
            }
            UnitOfWork.ProductCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductCategory(Guid userId, ProductCategoryEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductCategoryEditEntry, "ProductCategoryEditEntry", null, ErrorMessage.Messages[ErrorCode.NotFoundProductCategoryEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ProductCategoryRepository.FindById(entry.CategoryId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductCategory, "ProductCategory", entry.CategoryId, ErrorMessage.Messages[ErrorCode.NotFoundProductCategory]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.CategoryName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullCategoryName, "CategoryName", entry.CategoryName, ErrorMessage.Messages[ErrorCode.NullCategoryName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.CategoryName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCategoryName, "CategoryName", entry.CategoryName, ErrorMessage.Messages[ErrorCode.InvalidCategoryName]));
                    throw new ValidationError(violations);
                }
            }

            if (entry.ParentId != null && entry.ParentId > 0)
            {
                var parentEntity = UnitOfWork.NewsCategoryRepository.FindById(Convert.ToInt32(entry.ParentId));
                if (parentEntity == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullParentNode, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.NullParentNode]));
                    throw new ValidationError(violations);
                }

                var lineage = $"{parentEntity.Lineage},{entry.CategoryId}";
                entity.Lineage = lineage;
                entity.Depth = lineage.Split(',').Count();
            }
            else
            {
                entity.Lineage = $"{entry.CategoryId}";
                entity.Depth = 0;
            }

            entity.CategoryName = entry.CategoryName;
            entity.CategoryCode = entry.CategoryCode;
            entity.CategoryAlias = StringUtils.ConvertTitle2Alias(entry.CategoryName);
            entity.CategoryLink = entry.CategoryLink;
            entity.ParentId = entry.ParentId;
            entity.Icon = entry.Icon;
            entity.BriefDescription = entry.BriefDescription;
            entity.Description = entry.Description;
            entity.Status = entry.Status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ProductCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductCategoryStatus(Guid userId, int id, ProductCategoryStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductCategoryRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductCategory, "ProductCategory", id, ErrorMessage.Messages[ErrorCode.NotFoundProductCategory]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ProductCategoryStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ProductCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductCategoryListOrder(Guid userId, int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductCategoryRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductCategory, "ProductCategory", id));
                throw new ValidationError(violations);
            }

            if (entity.ViewOrder == listOrder) return;

            entity.ViewOrder = listOrder;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ProductCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Product
        public IEnumerable<ProductDetail> GetProductList(int vendorId, string languageCode, ProductSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ProductRepository.GetProductList(vendorId, languageCode, filter.ProductName, filter.CategoryId, filter.FromDate, filter.ToDate, filter.Status,
                ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<Product, ProductDetail>();
        }

        public IEnumerable<ProductInfoDetail> GetDiscountedProducts(int vendorId, int? categoryId, ProductStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            List<ProductInfoDetail> lst = new List<ProductInfoDetail>();
            var products = UnitOfWork.ProductRepository.GetDiscountedProducts(vendorId, categoryId, status, ref recordCount, orderBy, page, pageSize).ToList();
            if (products.Any())
            {
                lst.AddRange(from p in products
                             let productTaxRate = p.ProductTaxRate
                             where productTaxRate != null
                             let taxRate = (productTaxRate != null && productTaxRate.IsPercent) ? (Convert.ToDecimal(p.NetPrice) * productTaxRate.TaxRate) / 100 : productTaxRate.TaxRate
                             let productDiscount = p.ProductDiscount
                             where productDiscount != null
                             let discountRate = (productDiscount != null && productDiscount.IsPercent) ? (Convert.ToDecimal(p.NetPrice) * productDiscount.DiscountRate) / 100 : productDiscount.DiscountRate
                             let comments = UnitOfWork.ProductCommentRepository.GetProductComments(p.ProductId, null)
                             select new ProductInfoDetail
                             {
                                 ProductId = p.ProductId,
                                 ProductCode = p.ProductCode,
                                 ProductName = p.ProductName,
                                 ProductAlias = p.ProductAlias,
                                 NetPrice = p.NetPrice,
                                 GrossPrice = p.GrossPrice,
                                 TaxRate = taxRate,
                                 DiscountRate = discountRate,
                                 UnitsInStock = p.UnitsInStock,
                                 UnitsOnOrder = p.UnitsOnOrder,
                                 UnitsInAPackage = p.UnitsInAPackage,
                                 UnitsInBox = p.UnitsInBox,
                                 Unit = p.Unit,
                                 Weight = p.Weight,
                                 UnitOfWeightMeasure = p.UnitOfWeightMeasure,
                                 Length = p.Length,
                                 Width = p.Width,
                                 Height = p.Height,
                                 UnitOfDimensionMeasure = p.UnitOfDimensionMeasure,
                                 Url = p.Url,
                                 MinPurchaseQty = p.MinPurchaseQty,
                                 MaxPurchaseQty = p.MaxPurchaseQty,
                                 Rating = p.Rating,
                                 ListOrder = p.ListOrder,
                                 Views = p.Views,
                                 SmallPhoto = p.SmallPhoto,
                                 LargePhoto = p.LargePhoto,
                                 ShortDescription = p.ShortDescription,
                                 Specification = !string.IsNullOrEmpty(p.Specification) ? HttpUtility.HtmlDecode(p.Specification) : null,
                                 Availability = p.Availability,
                                 StartDate = p.StartDate,
                                 EndDate = p.EndDate,
                                 PurchaseScope = p.PurchaseScope,
                                 Warranty = p.Warranty,
                                 IsOnline = p.IsOnline,
                                 InfoNotification = p.InfoNotification,
                                 PriceNotification = p.PriceNotification,
                                 QtyNotification = p.QtyNotification,
                                 Status = p.Status,
                                 CategoryId = p.CategoryId,
                                 LanguageCode = p.LanguageCode,
                                 CurrencyCode = p.CurrencyCode,
                                 ManufacturerId = p.ManufacturerId,
                                 VendorId = p.VendorId,
                                 ProductTypeId = p.ProductTypeId,
                                 DiscountId = p.DiscountId,
                                 SmallPhotoUrl = (p.SmallPhoto != null && p.SmallPhoto > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.SmallPhoto)).FileUrl : GlobalSettings.NotFoundFileUrl,
                                 LargePhotoUrl = (p.LargePhoto != null && p.LargePhoto > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.LargePhoto)).FileUrl : GlobalSettings.NotFoundFileUrl,
                                 ProductCategory = p.ProductCategory.ToDto<ProductCategory, ProductCategoryDetail>(),
                                 ProductType = p.ProductType.ToDto<ProductType, ProductTypeDetail>(),
                                 ProductTaxRate = productTaxRate.ToDto<ProductTaxRate, ProductTaxRateDetail>(),
                                 ProductDiscount = p.ProductDiscount.ToDto<ProductDiscount, ProductDiscountDetail>(),
                                 Manufacturer = p.Manufacturer.ToDto<Manufacturer, ManufacturerDetail>(),
                                 Comments = comments.ToDtos<ProductComment, ProductCommentDetail>()
                             });
            }
            return lst;
        }

        public IEnumerable<ProductInfoDetail> GetLastestProducts(int vendorId, int? categoryId, ProductStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            List<ProductInfoDetail> lst = new List<ProductInfoDetail>();
            var products = UnitOfWork.ProductRepository.GetLastestProducts(vendorId, categoryId, status, ref recordCount, orderBy, page, pageSize).ToList();
            if (products.Any())
            {
                lst.AddRange(from p in products
                             let specification = !string.IsNullOrEmpty(p.Specification) ? HttpContext.Current.Server.HtmlDecode(p.Specification) : null
                             let comments = UnitOfWork.ProductCommentRepository.GetProductComments(p.ProductId, null)
                             select new ProductInfoDetail
                             {
                                 ProductId = p.ProductId,
                                 ProductCode = p.ProductCode,
                                 ProductName = p.ProductName,
                                 ProductAlias = p.ProductAlias,
                                 NetPrice = p.NetPrice,
                                 GrossPrice = p.GrossPrice,
                                 UnitsInStock = p.UnitsInStock,
                                 UnitsOnOrder = p.UnitsOnOrder,
                                 UnitsInAPackage = p.UnitsInAPackage,
                                 UnitsInBox = p.UnitsInBox,
                                 Unit = p.Unit,
                                 Weight = p.Weight,
                                 UnitOfWeightMeasure = p.UnitOfWeightMeasure,
                                 Length = p.Length,
                                 Width = p.Width,
                                 Height = p.Height,
                                 UnitOfDimensionMeasure = p.UnitOfDimensionMeasure,
                                 Url = p.Url,
                                 MinPurchaseQty = p.MinPurchaseQty,
                                 MaxPurchaseQty = p.MaxPurchaseQty,
                                 Rating = p.Rating,
                                 ListOrder = p.ListOrder,
                                 Views = p.Views,
                                 SmallPhoto = p.SmallPhoto,
                                 LargePhoto = p.LargePhoto,
                                 ShortDescription = p.ShortDescription,
                                 Specification = specification,
                                 Availability = p.Availability,
                                 StartDate = p.StartDate,
                                 EndDate = p.EndDate,
                                 PurchaseScope = p.PurchaseScope,
                                 Warranty = p.Warranty,
                                 IsOnline = p.IsOnline,
                                 InfoNotification = p.InfoNotification,
                                 PriceNotification = p.PriceNotification,
                                 QtyNotification = p.QtyNotification,
                                 Status = p.Status,
                                 CategoryId = p.CategoryId,
                                 LanguageCode = p.LanguageCode,
                                 CurrencyCode = p.CurrencyCode,
                                 ManufacturerId = p.ManufacturerId,
                                 VendorId = p.VendorId,
                                 ProductTypeId = p.ProductTypeId,
                                 DiscountId = p.DiscountId,
                                 SmallPhotoUrl = (p.SmallPhoto != null && p.SmallPhoto > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.SmallPhoto)).FileUrl : GlobalSettings.NotFoundFileUrl,
                                 LargePhotoUrl = (p.LargePhoto != null && p.LargePhoto > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.LargePhoto)).FileUrl : GlobalSettings.NotFoundFileUrl,
                                 ProductCategory = p.ProductCategory.ToDto<ProductCategory, ProductCategoryDetail>(),
                                 ProductType = p.ProductType.ToDto<ProductType, ProductTypeDetail>(),
                                 ProductDiscount = p.ProductDiscount.ToDto<ProductDiscount, ProductDiscountDetail>(),
                                 Manufacturer = p.Manufacturer.ToDto<Manufacturer, ManufacturerDetail>(),
                                 Comments = comments.ToDtos<ProductComment, ProductCommentDetail>()
                             });
            }
            return lst;
        }

        public IEnumerable<ProductInfoDetail> GetProductsByManufacturer(int vendorId, int manufacturerId, ProductStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            List<ProductInfoDetail> lst = new List<ProductInfoDetail>();
            var products = UnitOfWork.ProductRepository.GetProductsByManufacturer(vendorId, manufacturerId, status, ref recordCount, orderBy, page, pageSize).ToList();
            if (products.Any())
            {
                lst.AddRange(from p in products
                             let productTaxRate = p.ProductTaxRate
                             where productTaxRate != null
                             let taxRate = (productTaxRate != null && productTaxRate.IsPercent) ? (Convert.ToDecimal(p.NetPrice) * productTaxRate.TaxRate) / 100 : productTaxRate.TaxRate
                             let productDiscount = p.ProductDiscount
                             where productDiscount != null
                             let discountRate = (productDiscount != null && productDiscount.IsPercent) ? (Convert.ToDecimal(p.NetPrice) * productDiscount.DiscountRate) / 100 : productDiscount.DiscountRate
                             let comments = UnitOfWork.ProductCommentRepository.GetProductComments(p.ProductId, null)
                             select new ProductInfoDetail
                             {
                                 ProductId = p.ProductId,
                                 ProductCode = p.ProductCode,
                                 ProductName = p.ProductName,
                                 ProductAlias = p.ProductAlias,
                                 NetPrice = p.NetPrice,
                                 GrossPrice = p.GrossPrice,
                                 TaxRate = taxRate,
                                 DiscountRate = discountRate,
                                 UnitsInStock = p.UnitsInStock,
                                 UnitsOnOrder = p.UnitsOnOrder,
                                 UnitsInAPackage = p.UnitsInAPackage,
                                 UnitsInBox = p.UnitsInBox,
                                 Unit = p.Unit,
                                 Weight = p.Weight,
                                 UnitOfWeightMeasure = p.UnitOfWeightMeasure,
                                 Length = p.Length,
                                 Width = p.Width,
                                 Height = p.Height,
                                 UnitOfDimensionMeasure = p.UnitOfDimensionMeasure,
                                 Url = p.Url,
                                 MinPurchaseQty = p.MinPurchaseQty,
                                 MaxPurchaseQty = p.MaxPurchaseQty,
                                 Rating = p.Rating,
                                 ListOrder = p.ListOrder,
                                 Views = p.Views,
                                 SmallPhoto = p.SmallPhoto,
                                 LargePhoto = p.LargePhoto,
                                 ShortDescription = p.ShortDescription,
                                 Specification = !string.IsNullOrEmpty(p.Specification) ? HttpUtility.HtmlDecode(p.Specification) : null,
                                 Availability = p.Availability,
                                 StartDate = p.StartDate,
                                 EndDate = p.EndDate,
                                 PurchaseScope = p.PurchaseScope,
                                 Warranty = p.Warranty,
                                 IsOnline = p.IsOnline,
                                 InfoNotification = p.InfoNotification,
                                 PriceNotification = p.PriceNotification,
                                 QtyNotification = p.QtyNotification,
                                 Status = p.Status,
                                 CategoryId = p.CategoryId,
                                 LanguageCode = p.LanguageCode,
                                 CurrencyCode = p.CurrencyCode,
                                 ManufacturerId = p.ManufacturerId,
                                 VendorId = p.VendorId,
                                 ProductTypeId = p.ProductTypeId,
                                 DiscountId = p.DiscountId,
                                 SmallPhotoUrl = (p.SmallPhoto != null && p.SmallPhoto > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.SmallPhoto)).FileUrl : GlobalSettings.NotFoundFileUrl,
                                 LargePhotoUrl = (p.LargePhoto != null && p.LargePhoto > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.LargePhoto)).FileUrl : GlobalSettings.NotFoundFileUrl,
                                 ProductCategory = p.ProductCategory.ToDto<ProductCategory, ProductCategoryDetail>(),
                                 ProductType = p.ProductType.ToDto<ProductType, ProductTypeDetail>(),
                                 ProductTaxRate = productTaxRate.ToDto<ProductTaxRate, ProductTaxRateDetail>(),
                                 ProductDiscount = p.ProductDiscount.ToDto<ProductDiscount, ProductDiscountDetail>(),
                                 Manufacturer = p.Manufacturer.ToDto<Manufacturer, ManufacturerDetail>(),
                                 Comments = comments.ToDtos<ProductComment, ProductCommentDetail>()
                             });
            }
            return lst;
        }

        public IEnumerable<ProductInfoDetail> GetProducts(ProductSearchEntry entry, string languageCode, int vendorId, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            List<ProductInfoDetail> lst = new List<ProductInfoDetail>();
            var products = UnitOfWork.ProductRepository.GetProducts(languageCode, vendorId, entry.CategoryId, entry.ProductName, entry.FromDate,
                entry.ToDate, entry.MinPrice, entry.MaxPrice, entry.Status, ref recordCount, orderBy, page, pageSize).ToList();

            if (products.Any())
            {
                foreach (var p in products)
                {
                    var entity = UnitOfWork.ProductRepository.GetDetailsByProductId(p.ProductId);
                    if (entity == null) return null;

                    decimal taxRate = 0;
                    if (entity.ProductTaxRate != null && entity.ProductTaxRate.TaxRate > 0 && entity.NetPrice != null)
                    {
                        bool isPercent = entity.ProductTaxRate.IsPercent;
                        taxRate = isPercent ? (Convert.ToDecimal(entity.NetPrice) * entity.ProductTaxRate.TaxRate) / 100 : entity.ProductTaxRate.TaxRate;
                    }

                    decimal discountRate = 0;
                    if (entity.ProductDiscount != null && entity.ProductDiscount.DiscountRate > 0 && entity.NetPrice != null)
                    {
                        bool isPercent = entity.ProductDiscount.IsPercent;
                        discountRate = isPercent
                            ? (Convert.ToDecimal(entity.NetPrice) * entity.ProductDiscount.DiscountRate) / 100
                            : entity.ProductDiscount.DiscountRate;
                    }

                    var comments = UnitOfWork.ProductCommentRepository.GetProductComments(p.ProductId, null);

                    var item = new ProductInfoDetail
                    {
                        ProductId = p.ProductId,
                        ProductCode = p.ProductCode,
                        ProductName = p.ProductName,
                        ProductAlias = p.ProductAlias,
                        NetPrice = p.NetPrice,
                        GrossPrice = p.GrossPrice,
                        TaxRate = taxRate,
                        DiscountRate = discountRate,
                        UnitsInStock = p.UnitsInStock,
                        UnitsOnOrder = p.UnitsOnOrder,
                        UnitsInAPackage = p.UnitsInAPackage,
                        UnitsInBox = p.UnitsInBox,
                        Unit = p.Unit,
                        Weight = p.Weight,
                        UnitOfWeightMeasure = p.UnitOfWeightMeasure,
                        Length = p.Length,
                        Width = p.Width,
                        Height = p.Height,
                        UnitOfDimensionMeasure = p.UnitOfDimensionMeasure,
                        Url = p.Url,
                        MinPurchaseQty = p.MinPurchaseQty,
                        MaxPurchaseQty = p.MaxPurchaseQty,
                        Rating = p.Rating,
                        ListOrder = p.ListOrder,
                        Views = p.Views,
                        SmallPhoto = p.SmallPhoto,
                        LargePhoto = p.LargePhoto,
                        ShortDescription = p.ShortDescription,
                        Specification = !string.IsNullOrEmpty(p.Specification) ? HttpUtility.HtmlDecode(p.Specification) : null,
                        Availability = p.Availability,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        PurchaseScope = p.PurchaseScope,
                        Warranty = p.Warranty,
                        IsOnline = p.IsOnline,
                        InfoNotification = p.InfoNotification,
                        PriceNotification = p.PriceNotification,
                        QtyNotification = p.QtyNotification,
                        Status = p.Status,
                        CategoryId = p.CategoryId,
                        LanguageCode = p.LanguageCode,
                        CurrencyCode = p.CurrencyCode,
                        ManufacturerId = p.ManufacturerId,
                        VendorId = p.VendorId,
                        ProductTypeId = p.ProductTypeId,
                        DiscountId = p.DiscountId,
                        SmallPhotoUrl = (p.SmallPhoto != null && p.SmallPhoto > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.SmallPhoto)).FileUrl : GlobalSettings.NotFoundFileUrl,
                        LargePhotoUrl = (p.LargePhoto != null && p.LargePhoto > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.LargePhoto)).FileUrl : GlobalSettings.NotFoundFileUrl,
                        ProductCategory = p.ProductCategory.ToDto<ProductCategory, ProductCategoryDetail>(),
                        ProductType = p.ProductType.ToDto<ProductType, ProductTypeDetail>(),
                        ProductTaxRate = p.ProductTaxRate.ToDto<ProductTaxRate, ProductTaxRateDetail>(),
                        ProductDiscount = p.ProductDiscount.ToDto<ProductDiscount, ProductDiscountDetail>(),
                        Manufacturer = p.Manufacturer.ToDto<Manufacturer, ManufacturerDetail>(),
                        Comments = comments.ToDtos<ProductComment, ProductCommentDetail>()
                    };

                    lst.Add(item);
                }
            }
            return lst;
        }

        public ProductDetail GetProductDetail(int productId)
        {
            var entity = UnitOfWork.ProductRepository.FindById(productId);
            if (entity != null && !string.IsNullOrEmpty(entity.Specification))
            {
                entity.Specification = HttpUtility.HtmlDecode(entity.Specification);
            }
            return entity.ToDto<Product, ProductDetail>();
        }
        public ProductInfoDetail GetProductDetails(int productId)
        {
            var entity = UnitOfWork.ProductRepository.GetDetailsByProductId(productId);
            if (entity == null) return null;

            decimal taxRate = 0;
            if (entity.ProductTaxRate != null && entity.ProductTaxRate.TaxRate > 0 && entity.NetPrice != null)
            {
                bool isPercent = entity.ProductTaxRate.IsPercent;
                taxRate = isPercent ? (Convert.ToDecimal(entity.NetPrice) * entity.ProductTaxRate.TaxRate) / 100 : entity.ProductTaxRate.TaxRate;
            }

            decimal discountRate = 0;
            if (entity.ProductDiscount != null && entity.ProductDiscount.DiscountRate > 0 && entity.NetPrice != null)
            {
                bool isPercent = entity.ProductDiscount.IsPercent;
                discountRate = isPercent
                    ? (Convert.ToDecimal(entity.NetPrice) * entity.ProductDiscount.DiscountRate) / 100
                    : entity.ProductDiscount.DiscountRate;
            }

            var comments = UnitOfWork.ProductCommentRepository.GetProductComments(entity.ProductId, null);

            var item = new ProductInfoDetail
            {
                ProductId = entity.ProductId,
                ProductCode = entity.ProductCode,
                ProductName = entity.ProductName,
                ProductAlias = entity.ProductAlias,
                NetPrice = entity.NetPrice ?? 0,
                GrossPrice = entity.GrossPrice ?? 0,
                TaxRate = taxRate,
                DiscountRate = discountRate,
                UnitsInStock = entity.UnitsInStock,
                UnitsOnOrder = entity.UnitsOnOrder,
                UnitsInAPackage = entity.UnitsInAPackage,
                UnitsInBox = entity.UnitsInBox,
                Unit = entity.Unit,
                Weight = entity.Weight,
                UnitOfWeightMeasure = entity.UnitOfWeightMeasure,
                Length = entity.Length,
                Width = entity.Width,
                Height = entity.Height,
                UnitOfDimensionMeasure = entity.UnitOfDimensionMeasure,
                Url = entity.Url,
                MinPurchaseQty = entity.MinPurchaseQty,
                MaxPurchaseQty = entity.MaxPurchaseQty,
                Rating = entity.Rating,
                ListOrder = entity.ListOrder,
                Views = entity.Views,
                SmallPhoto = entity.SmallPhoto,
                LargePhoto = entity.LargePhoto,
                ShortDescription = entity.ShortDescription,
                Specification = !string.IsNullOrEmpty(entity.Specification) ? HttpUtility.HtmlDecode(entity.Specification) : string.Empty,
                Availability = entity.Availability,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                PurchaseScope = entity.PurchaseScope,
                Warranty = entity.Warranty,
                IsOnline = entity.IsOnline,
                InfoNotification = entity.InfoNotification,
                PriceNotification = entity.PriceNotification,
                QtyNotification = entity.QtyNotification,
                Status = entity.Status,
                CategoryId = entity.CategoryId,
                LanguageCode = entity.LanguageCode,
                CurrencyCode = entity.CurrencyCode,
                ManufacturerId = entity.ManufacturerId,
                VendorId = entity.VendorId,
                ProductTypeId = entity.ProductTypeId,
                DiscountId = entity.DiscountId,
                SmallPhotoUrl =
                    (entity.SmallPhoto != null && entity.SmallPhoto > 0)
                        ? DocumentService.GetFileInfoDetail(Convert.ToInt32(entity.SmallPhoto)).FileUrl
                        : GlobalSettings.NotFoundFileUrl,
                LargePhotoUrl =
                    (entity.LargePhoto != null && entity.LargePhoto > 0)
                        ? DocumentService.GetFileInfoDetail(Convert.ToInt32(entity.LargePhoto)).FileUrl
                        : GlobalSettings.NotFoundFileUrl,
                ProductCategory = entity.ProductCategory.ToDto<ProductCategory, ProductCategoryDetail>(),
                ProductType = entity.ProductType.ToDto<ProductType, ProductTypeDetail>(),
                ProductTaxRate = entity.ProductTaxRate.ToDto<ProductTaxRate, ProductTaxRateDetail>(),
                ProductDiscount = entity.ProductDiscount.ToDto<ProductDiscount, ProductDiscountDetail>(),
                Manufacturer = entity.Manufacturer.ToDto<Manufacturer, ManufacturerDetail>(),
                Comments = comments.ToDtos<ProductComment, ProductCommentDetail>()
            };
            return item;
        }
        public ProductInfoDetail GetDetailsByProductCode(string productCode)
        {
            var entity = UnitOfWork.ProductRepository.GetDetailsByProductCode(productCode);
            if (entity == null) return null;

            decimal taxRate = 0;
            if (entity.ProductTaxRate != null && entity.ProductTaxRate.TaxRate > 0 && entity.NetPrice != null)
            {
                bool isPercent = entity.ProductTaxRate.IsPercent;
                taxRate = isPercent ? (Convert.ToDecimal(entity.NetPrice) * entity.ProductTaxRate.TaxRate) / 100 : entity.ProductTaxRate.TaxRate;
            }

            decimal discountRate = 0;
            if (entity.ProductDiscount != null && entity.ProductDiscount.DiscountRate > 0 && entity.NetPrice != null)
            {
                bool isPercent = entity.ProductDiscount.IsPercent;
                discountRate = isPercent
                    ? (Convert.ToDecimal(entity.NetPrice) * entity.ProductDiscount.DiscountRate) / 100
                    : entity.ProductDiscount.DiscountRate;
            }

            var comments = UnitOfWork.ProductCommentRepository.GetProductComments(entity.ProductId, null);

            var item = new ProductInfoDetail
            {
                ProductId = entity.ProductId,
                ProductCode = entity.ProductCode,
                ProductName = entity.ProductName,
                ProductAlias = entity.ProductAlias,
                NetPrice = entity.NetPrice,
                GrossPrice = entity.GrossPrice,
                UnitsInStock = entity.UnitsInStock,
                TaxRate = taxRate,
                DiscountRate = discountRate,
                UnitsOnOrder = entity.UnitsOnOrder,
                UnitsInAPackage = entity.UnitsInAPackage,
                UnitsInBox = entity.UnitsInBox,
                Unit = entity.Unit,
                Weight = entity.Weight,
                UnitOfWeightMeasure = entity.UnitOfWeightMeasure,
                Length = entity.Length,
                Width = entity.Width,
                Height = entity.Height,
                UnitOfDimensionMeasure = entity.UnitOfDimensionMeasure,
                Url = entity.Url,
                MinPurchaseQty = entity.MinPurchaseQty,
                MaxPurchaseQty = entity.MaxPurchaseQty,
                Rating = entity.Rating,
                ListOrder = entity.ListOrder,
                Views = entity.Views,
                SmallPhoto = entity.SmallPhoto,
                LargePhoto = entity.LargePhoto,
                ShortDescription = entity.ShortDescription,
                Specification = HttpUtility.HtmlDecode(entity.Specification),
                Availability = entity.Availability,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                PurchaseScope = entity.PurchaseScope,
                Warranty = entity.Warranty,
                IsOnline = entity.IsOnline,
                InfoNotification = entity.InfoNotification,
                PriceNotification = entity.PriceNotification,
                QtyNotification = entity.QtyNotification,
                Status = entity.Status,
                CategoryId = entity.CategoryId,
                LanguageCode = entity.LanguageCode,
                CurrencyCode = entity.CurrencyCode,
                ManufacturerId = entity.ManufacturerId,
                VendorId = entity.VendorId,
                ProductTypeId = entity.ProductTypeId,
                DiscountId = entity.DiscountId,
                SmallPhotoUrl =
                    (entity.SmallPhoto != null && entity.SmallPhoto > 0)
                        ? DocumentService.GetFileInfoDetail(Convert.ToInt32(entity.SmallPhoto)).FileUrl
                        : GlobalSettings.NotFoundFileUrl,
                LargePhotoUrl =
                    (entity.LargePhoto != null && entity.LargePhoto > 0)
                        ? DocumentService.GetFileInfoDetail(Convert.ToInt32(entity.LargePhoto)).FileUrl
                        : GlobalSettings.NotFoundFileUrl,
                ProductCategory = entity.ProductCategory.ToDto<ProductCategory, ProductCategoryDetail>(),
                ProductType = entity.ProductType.ToDto<ProductType, ProductTypeDetail>(),
                ProductTaxRate = entity.ProductTaxRate.ToDto<ProductTaxRate, ProductTaxRateDetail>(),
                ProductDiscount = entity.ProductDiscount.ToDto<ProductDiscount, ProductDiscountDetail>(),
                Manufacturer = entity.Manufacturer.ToDto<Manufacturer, ManufacturerDetail>(),
                Comments = comments.ToDtos<ProductComment, ProductCommentDetail>()
            };
            return item;
        }

        public string GenerateProductCode(int maxLetters)
        {
            return UnitOfWork.ProductRepository.GenerateProductCode(maxLetters);
        }
        public string GenerateProductCode(int maxLetters, string seedId)
        {
            return UnitOfWork.ProductRepository.GenerateProductCode(maxLetters, seedId);
        }

        public ProductDetail InsertProduct(Guid applicationId, Guid userId, int vendorId, string languageCode, ProductEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            ISpecification<ProductEntry> validator = new ProductEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            string currencyCode;
            if (!string.IsNullOrEmpty(entry.CurrencyCode))
            {
                var setting = ApplicationService.GetCurrencySetting(applicationId,
                    SettingKeys.CurrencySettingName);
                currencyCode = setting != null ? setting.Setting.KeyValue : GlobalSettings.DefaultCurrencyCode;
            }
            else
            {
                currencyCode = entry.CurrencyCode;
            }

            var entity = entry.ToEntity<ProductEntry, Product>();
            entity.Specification = StringUtils.UTF8_Encode(entry.Specification);
            entity.ProductAlias = StringUtils.GenerateFriendlyString(entry.ProductName);
            entity.ListOrder = UnitOfWork.ProductRepository.GetNewListOrder();
            entity.CurrencyCode = currencyCode;
            entity.LanguageCode = languageCode;
            entity.Ip = ip;
            entity.CreatedByUserId = userId;
            entity.CreatedDate = DateTime.UtcNow;
            entity.VendorId = vendorId;

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                var fileIds = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, entry.File, (int)FileLocation.Product, StorageType.Local);
                entity.LargePhoto = fileIds[0].FileId;
                entity.SmallPhoto = fileIds[1].FileId;
            }

            UnitOfWork.ProductRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.ProductGalleryFiles != null && entry.ProductGalleryFiles.Any())
            {
                var productAlbums = new List<ProductAlbum>();
                foreach (var item in entry.ProductGalleryFiles)
                {
                    var productAlbum = new ProductAlbum();
                    var fileIds = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, item, (int)FileLocation.Gallery, StorageType.Local);
                    productAlbum.ProductId = entity.ProductId;
                    productAlbum.FileId = fileIds[0].FileId;
                    productAlbums.Add(productAlbum);
                }
                UnitOfWork.ProductAlbumRepository.InsertRange(productAlbums);
                UnitOfWork.SaveChanges();
            }

            if (entry.Attributes != null && entry.Attributes.Any())
            {
                InsertProductAttributes(entity.ProductId, entry.Attributes);
            }

            return entity.ToDto<Product, ProductDetail>();
        }
        public void UpdateProduct(Guid applicationId, Guid userId, int vendorId, string languageCode, ProductEditEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductEntry, "Product Entry", null, ErrorMessage.Messages[ErrorCode.NotFoundProductEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ProductRepository.FindById(entry.ProductId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProduct, "ProductId", entry.ProductId, ErrorMessage.Messages[ErrorCode.NotFoundProduct]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.ProductCode))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductCode, "ProductCode", entry.ProductCode, ErrorMessage.Messages[ErrorCode.InvalidProductCode]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.ProductCode.Length > 50)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductCode, "ProductCode", entry.ProductCode.Length, ErrorMessage.Messages[ErrorCode.InvalidProductCode]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.ProductCode != entry.ProductCode)
                    {
                        var isProductCodeDuplicated =
                            UnitOfWork.ProductRepository.HasProductCodeExisted(entry.ProductCode);
                        if (isProductCodeDuplicated)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicatedProductCode, "ProductCode", entry.ProductCode, ErrorMessage.Messages[ErrorCode.DuplicatedProductCode]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(entry.ProductName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullProductName, "Product Name", entry.ProductName, ErrorMessage.Messages[ErrorCode.NullProductName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.ProductName.Length > 500)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductName, "Product Name", entry.ProductName.Length, ErrorMessage.Messages[ErrorCode.InvalidProductName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.ProductName != entry.ProductName)
                    {
                        var isNameDuplicated = UnitOfWork.ProductRepository.HasProductNameExisted(entry.ProductName);
                        if (isNameDuplicated)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidProductName, "ProductName", entry.ProductName, ErrorMessage.Messages[ErrorCode.InvalidProductName]));
                            throw new ValidationError(violations);
                        }

                    }
                }
            }

            if (entry.CategoryId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidCategoryId, "CategoryId", entry.CategoryId, ErrorMessage.Messages[ErrorCode.InvalidCategoryId]));
                throw new ValidationError(violations);
            }
            else
            {
                var productCategory = UnitOfWork.ProductCategoryRepository.FindById(entry.CategoryId);
                if (productCategory == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCategoryId, "CategoryId", entry.CategoryId, ErrorMessage.Messages[ErrorCode.InvalidCategoryId]));
                    throw new ValidationError(violations);
                }
            }

            if (entry.ProductTypeId != null && entry.ProductTypeId > 0)
            {
                var productType = UnitOfWork.ProductTypeRepository.FindById(entry.ProductTypeId);
                if (productType == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductTypeId, "ProductTypeId", entry.ProductTypeId, ErrorMessage.Messages[ErrorCode.InvalidProductTypeId]));
                    throw new ValidationError(violations);
                }
            }

            if (entry.TaxRateId != null && entry.TaxRateId > 0)
            {
                var taxRate = UnitOfWork.ProductTaxRateRepository.FindById(entry.TaxRateId);
                if (taxRate == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductTaxRateId, "TaxRateId", entry.TaxRateId, ErrorMessage.Messages[ErrorCode.InvalidProductTaxRateId]));
                    throw new ValidationError(violations);
                }
            }

            if (entry.Attributes != null && entry.Attributes.Any())
            {
                InsertProductAttributes(entity.ProductId, entry.Attributes);
            }

            if (entry.ExistedAttributes != null && entry.ExistedAttributes.Any())
            {
                UpdateProductAttributes(entity.ProductId, entry.ExistedAttributes);
            }

            if (entry.ProductGalleryFiles != null && entry.ProductGalleryFiles.Any())
            {
                var productAlbums = new List<ProductAlbum>();
                foreach (var item in entry.ProductGalleryFiles)
                {
                    var productAlbum = new ProductAlbum();
                    var fileIds = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, item, (int)FileLocation.Gallery, StorageType.Local);
                    productAlbum.ProductId = entity.ProductId;
                    productAlbum.FileId = fileIds[0].FileId;
                    productAlbums.Add(productAlbum);
                }
                UnitOfWork.ProductAlbumRepository.InsertRange(productAlbums);
            }

            string currencyCode;
            if (!string.IsNullOrEmpty(entry.CurrencyCode))
            {
                var setting = ApplicationService.GetCurrencySetting(applicationId, SettingKeys.CurrencySettingName);
                currencyCode = setting != null ? setting.Setting.KeyValue : GlobalSettings.DefaultCurrencyCode;
            }
            else
            {
                currencyCode = entry.CurrencyCode;
            }

            entity.CategoryId = entry.CategoryId;
            entity.ManufacturerId = entry.ManufacturerId;
            entity.ProductTypeId = entry.ProductTypeId;
            entity.ProductCode = entry.ProductCode;
            entity.ProductName = entry.ProductName;
            entity.ProductAlias = StringUtils.GenerateFriendlyString(entry.ProductName);
            entity.DiscountId = entry.DiscountId;
            entity.NetPrice = entry.NetPrice;
            entity.GrossPrice = entry.GrossPrice;
            entity.TaxRateId = entry.TaxRateId;
            entity.UnitsInStock = entry.UnitsInStock;
            entity.UnitsOnOrder = entry.UnitsOnOrder;
            entity.UnitsInAPackage = entry.UnitsInAPackage;
            entity.UnitsInBox = entry.UnitsInBox;
            entity.Unit = entry.Unit;
            entity.Weight = entry.Weight;
            entity.UnitOfWeightMeasure = entry.UnitOfWeightMeasure;
            entity.Length = entry.Length;
            entity.Width = entry.Width;
            entity.Height = entry.Height;
            entity.UnitOfDimensionMeasure = entry.UnitOfDimensionMeasure;
            entity.Url = entry.Url;
            entity.MinPurchaseQty = entry.MinPurchaseQty;
            entity.MaxPurchaseQty = entry.MaxPurchaseQty;
            entity.Views = entry.Views;
            entity.SmallPhoto = entry.SmallPhoto;
            entity.LargePhoto = entry.LargePhoto;
            entity.ShortDescription = entry.ShortDescription;
            entity.Specification = StringUtils.UTF8_Encode(entry.Specification);
            entity.Availability = entry.Availability;
            entity.StartDate = entry.StartDate;
            entity.EndDate = entry.EndDate;
            entity.PurchaseScope = entry.PurchaseScope;
            entity.Warranty = entry.Warranty;
            entity.IsOnline = entry.IsOnline;
            entity.CurrencyCode = currencyCode;
            entity.LanguageCode = languageCode;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;
            entity.LastUpdatedIp = ip;
            entity.VendorId = vendorId;
            entity.BrandId = entry.BrandId;

            UnitOfWork.ProductRepository.Update(entity);
            UnitOfWork.SaveChanges();


            if (entry.File != null && entry.File.ContentLength > 0)
            {
                if (entity.SmallPhoto != null)
                {
                    DocumentService.DeleteFile(Convert.ToInt32(entity.SmallPhoto));
                }

                if (entity.LargePhoto != null)
                {
                    DocumentService.DeleteFile(Convert.ToInt32(entity.LargePhoto));
                }

                var fileIds = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, entry.File, (int)FileLocation.Product, StorageType.Local);
                entity.LargePhoto = fileIds[0].FileId;
                entity.SmallPhoto = fileIds[1].FileId;

                UnitOfWork.ProductRepository.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdateProductStatus(int id, ProductStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProduct, "Product", id, ErrorMessage.Messages[ErrorCode.NotFoundProduct]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ProductStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            UnitOfWork.ProductRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateUnitsOnOrder(int id, int unitsOnOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProduct, "Product", id, ErrorMessage.Messages[ErrorCode.NotFoundProduct]));
                throw new ValidationError(violations);
            }
            if (entity.UnitsOnOrder == unitsOnOrder) return;

            entity.UnitsOnOrder = unitsOnOrder;
            UnitOfWork.ProductRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductTotalViews(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProduct, "Product", id, ErrorMessage.Messages[ErrorCode.NotFoundProduct]));
                throw new ValidationError(violations);
            }

            if (entity.Views == null) entity.Views = 0;
            entity.Views = entity.Views + 1;

            UnitOfWork.ProductRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProduct, "Product", id, ErrorMessage.Messages[ErrorCode.NotFoundProduct]));
                throw new ValidationError(violations);
            }
            if (UnitOfWork.OrderProductTempRepository.HasOrderProductTempExisted(id, ItemType.Product)
                || UnitOfWork.OrderProductRepository.HasOrderProductExisted(id, ItemType.Product))
            {
                violations.Add(new RuleViolation(ErrorCode.ExistedOder, "Product", id, ErrorMessage.Messages[ErrorCode.ExistedOder]));
                throw new ValidationError(violations);
            }

            var proAlbums = UnitOfWork.ProductAlbumRepository.GetProductAlbum(id);

            if (!proAlbums.IsNullOrEmpty())
            {
                proAlbums.ForEach(t => DeleteImageProductAlbum(id, t.FileId));
            }
            UnitOfWork.ProductRepository.Delete(entity);
            UnitOfWork.SaveChanges();

        }

        #endregion

        #region Product Type
        public IEnumerable<ProductTypeDetail> GeProductTypes(int vendorId, ProductTypeSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ProductTypeRepository.GetProductTypes(vendorId, filter.ProductCategoryId, filter.TypeName, filter.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ProductType, ProductTypeDetail>();
        }
        public SelectList PoplulateProductTypeSelectList(int categoryId, ProductTypeStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.ProductTypeRepository.PoplulateProductTypeSelectList(categoryId, status, selectedValue, isShowSelectText);
        }
        public ProductTypeDetail GetProductTypeDetails(int productTypeId)
        {
            var entity = UnitOfWork.ProductTypeRepository.FindById(productTypeId);
            return entity.ToDto<ProductType, ProductTypeDetail>();
        }
        public void InsertProductType(int vendorId, ProductTypeEntry entry)
        {
            ISpecification<ProductTypeEntry> validator = new ProductTypeEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<ProductTypeEntry, ProductType>();
            entity.VendorId = vendorId;
            entity.ListOrder = UnitOfWork.ProductTypeRepository.GetNewListOrder();

            UnitOfWork.ProductTypeRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductType(int vendorId, ProductTypeEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductTypeEntry, "ProductTypeEntry", null, ErrorMessage.Messages[ErrorCode.NotFoundProductTypeEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ProductTypeRepository.FindById(entry.TypeId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductType, "ProductType", entry.TypeId, ErrorMessage.Messages[ErrorCode.NotFoundProductType]));
                throw new ValidationError(violations);
            }

            if (entry.CategoryId > 0)
            {
                var productCategory = UnitOfWork.ProductCategoryRepository.FindById(entry.CategoryId);
                if (productCategory == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductCategoryId, "CategoryId", entry.CategoryId, ErrorMessage.Messages[ErrorCode.InvalidProductCategoryId]));
                    throw new ValidationError(violations);
                }
            }

            if (string.IsNullOrEmpty(entry.TypeName))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductTypeName, "TypeName", entry.TypeName, ErrorMessage.Messages[ErrorCode.InvalidProductTypeName]));
                throw new ValidationError(violations);
            }

            entity.TypeName = entry.TypeName;
            entity.CategoryId = entry.CategoryId;
            entity.Description = entry.Description;
            entity.IsActive = entry.IsActive;
            entity.VendorId = vendorId;


            UnitOfWork.ProductTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();

        }
        public void UpdateProductTypeStatus(int id, ProductTypeStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductTypeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductType, "ProductType", id, ErrorMessage.Messages[ErrorCode.NotFoundProductType]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ProductTypeStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            UnitOfWork.ProductTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductTypeListOrder(int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductTypeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductType, "ProductType", id, ErrorMessage.Messages[ErrorCode.NotFoundProductType]));
                throw new ValidationError(violations);
            }

            if (entity.ListOrder == listOrder) return;

            entity.ListOrder = listOrder;

            UnitOfWork.ProductTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductTypeOrder(int currentProductTypeId, bool isUp)
        {
            var violations = new List<RuleViolation>();
            var currentEntity = UnitOfWork.ProductTypeRepository.FindById(currentProductTypeId);
            if (currentEntity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductType, "ProductType", currentProductTypeId, ErrorMessage.Messages[ErrorCode.NotFoundProductType]));
                throw new ValidationError(violations);
            }

            if (isUp)
            {
                var nextEntity = UnitOfWork.ProductTypeRepository.GetNextItem(currentProductTypeId);
                if (nextEntity == null) return;

                nextEntity.ListOrder = currentProductTypeId;
                UnitOfWork.ProductTypeRepository.Update(nextEntity);

                currentEntity.ListOrder = nextEntity.TypeId;
                UnitOfWork.ProductTypeRepository.Update(currentEntity);
            }
            else
            {
                var prevEntity = UnitOfWork.ProductTypeRepository.GetPreviousItem(currentProductTypeId);
                if (prevEntity == null) return;

                prevEntity.ListOrder = currentProductTypeId;
                UnitOfWork.ProductTypeRepository.Update(prevEntity);

                currentEntity.ListOrder = prevEntity.TypeId;
                UnitOfWork.ProductTypeRepository.Update(currentEntity);
            }
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Product Tax Rate
        public IEnumerable<ProductTaxRateDetail> GeProductTaxRates(ProductTaxRateSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ProductTaxRateRepository.GetList(filter.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ProductTaxRate, ProductTaxRateDetail>();
        }

        public SelectList PopulateProductTaxRateSelectList(ProductTaxRateStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.ProductTaxRateRepository.PopulateProductTaxRateSelectList(status, selectedValue, isShowSelectText);
        }

        public ProductTaxRateDetail GetProductTaxRateDetails(int productTaxRateId)
        {
            var entity = UnitOfWork.ProductTaxRateRepository.FindById(productTaxRateId);
            return entity.ToDto<ProductTaxRate, ProductTaxRateDetail>();
        }
        public void InsertProductTaxRate(ProductTaxRateEntry entry)
        {
            ISpecification<ProductTaxRateEntry> validator = new ProductTaxRateEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var isExisted = UnitOfWork.ProductTaxRateRepository.HasDataExisted(entry.TaxRate, entry.IsPercent);
            if (isExisted)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.DuplicateTaxRate, "TaxRate", entry.TaxRate, ErrorMessage.Messages[ErrorCode.DuplicateTaxRate]));
                throw new ValidationError(dataViolations);
            }

            var entity = entry.ToEntity<ProductTaxRateEntry, ProductTaxRate>();
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.ProductTaxRateRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductTaxRate(ProductTaxRateEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullProductTaxRateEditEntry, "ProductTaxRateEditEntry", null, ErrorMessage.Messages[ErrorCode.NullProductTaxRateEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ProductTaxRateRepository.FindById(entry.TaxRateId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTaxRateId, "TaxRateId", entry.TaxRateId, ErrorMessage.Messages[ErrorCode.InvalidTaxRateId]));
                throw new ValidationError(violations);
            }

            entity.TaxRate = entry.TaxRate;
            entity.IsPercent = entry.IsPercent;
            entity.Description = entry.Description;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ProductTaxRateRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductTaxRateStatus(int id, ProductTaxRateStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductTaxRateRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductType, "TaxRateId", id, ErrorMessage.Messages[ErrorCode.NotFoundProductType]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ProductTaxRateStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ProductTaxRateRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Product Attribute
        public IEnumerable<ProductAttributeDetail> GeAttributes(int productId)
        {
            var lst = UnitOfWork.ProductAttributeRepository.GetProductAttributes(productId);
            return lst.ToDtos<ProductAttribute, ProductAttributeDetail>();
        }
        public ProductAttributeDetail GetProductAttributeDetails(int productAttributeId)
        {
            var entity = UnitOfWork.ProductAttributeRepository.FindById(productAttributeId);
            return entity.ToDto<ProductAttribute, ProductAttributeDetail>();
        }
        public void InsertProductAttribute(int productId, ProductAttributeEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductAttributeEntry, "Product Attribute Entry", null, ErrorMessage.Messages[ErrorCode.NotFoundProductAttributeEntry]));
                throw new ValidationError(violations);
            }

            var product = UnitOfWork.ProductRepository.FindById(productId);
            if (product == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductId, "ProductId", productId, ErrorMessage.Messages[ErrorCode.InvalidProductId]));
                throw new ValidationError(violations);
            }
            
            if (string.IsNullOrEmpty(entry.AttributeName))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeName, "Product Attribute Name", entry.AttributeName, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.AttributeName.Length > 255)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeName, "Product Attribute Name", entry.AttributeName, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool isDuplicate = UnitOfWork.ProductAttributeRepository.HasDataExisted(productId, entry.AttributeName);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateProductAttributeName, "Product Attribute Name", entry.AttributeName, ErrorMessage.Messages[ErrorCode.DuplicateProductAttributeName]));
                        throw new ValidationError(violations);
                    }
                }
            }

            var entity = entry.ToEntity<ProductAttributeEntry, ProductAttribute>();
            entity.ProductId = productId;
            entity.ListOrder = UnitOfWork.ProductAttributeRepository.GetNewListOrder();

            UnitOfWork.ProductAttributeRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.Options != null && entry.Options.Any())
            {
                InsertProductAttributeOptions(entity.AttributeId, entry.Options);
            }
        }

        public void InsertProductAttributes(int productId, List<ProductAttributeEntry> entries)
        {
            var violations = new List<RuleViolation>();
            var product = UnitOfWork.ProductRepository.FindById(productId);
            if (product == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductId, "ProductId", productId, ErrorMessage.Messages[ErrorCode.InvalidProductId]));
                throw new ValidationError(violations);
            }

            if (entries!=null && entries.Any())
            {
                foreach (var entry in entries)
                {
                    if (!string.IsNullOrEmpty(entry.AttributeName))
                    {
                        InsertProductAttribute(productId, entry);
                    }
                }
            }
            UnitOfWork.SaveChanges();
        }

        public void UpdateProductAttribute(int productId, ProductAttributeEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductAttributeEntry, "Product Attribute Edit Entry", null, ErrorMessage.Messages[ErrorCode.NotFoundProductAttributeEntry]));
                throw new ValidationError(violations);
            }
            if (string.IsNullOrEmpty(entry.AttributeName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullProductAttributeName, "Product Attribute Name", entry.AttributeName, ErrorMessage.Messages[ErrorCode.NullProductAttributeName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (!string.IsNullOrEmpty(entry.AttributeName) && entry.AttributeName.Length > 255)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeName, "Product Attribute Name", entry.AttributeName.Length, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeName]));
                    throw new ValidationError(violations);
                }
            }

            if (entry.AttributeId == null || entry.AttributeId == 0)
            {
                if (!string.IsNullOrEmpty(entry.AttributeName))
                {
                    var attributeEntry = new ProductAttributeEntry
                    {
                        AttributeName = entry.AttributeName,
                        IsActive = entry.IsActive,
                        Options = entry.Options
                    };

                    InsertProductAttribute(productId, attributeEntry);
                }
            }
            else
            {
                var entity = UnitOfWork.ProductAttributeRepository.FindById(entry.AttributeId);
                if (entity == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundProductAttribute, "AttributeId",
                        entry.AttributeId, ErrorMessage.Messages[ErrorCode.NotFoundProductAttribute]));
                    throw new ValidationError(violations);
                }

                entity.ProductId = productId;
                entity.AttributeName = entry.AttributeName;
                entity.IsActive = entry.IsActive;

                UnitOfWork.ProductAttributeRepository.Update(entity);
                UnitOfWork.SaveChanges();

                if (entry.Options != null && entry.Options.Any())
                {
                    InsertProductAttributeOptions(entity.AttributeId, entry.Options);
                }

                if (entry.ExistedOptions != null && entry.ExistedOptions.Any())
                {
                    foreach (var option in entry.ExistedOptions)
                    {
                        if (option.AttributeId != null && option.OptionId != null && option.AttributeId > 0 &&
                            option.OptionId > 0)
                        {
                            var optionEntry = new ProductAttributeOptionEditEntry
                            {
                                AttributeId = entity.AttributeId,
                                OptionId = option.OptionId,
                                OptionName = option.OptionName,
                                OptionValue = option.OptionValue,
                                IsActive = option.IsActive
                            };
                            UpdateProductAttributeOption(optionEntry);
                        }
                        else
                        {
                            var optionEntry = new ProductAttributeOptionEntry
                            {
                                OptionName = option.OptionName,
                                OptionValue = option.OptionValue,
                                IsActive = option.IsActive
                            };
                            InsertProductAttributeOption(entity.AttributeId, optionEntry);
                        }
                    }
                }
            }
        }

        public void UpdateProductAttributes(int productId, List<ProductAttributeEditEntry> entries)
        {
            if (entries.Any())
            {
                foreach (var entry in entries)
                {
                    UpdateProductAttribute(productId, entry);
                }
            }
        }

        public void UpdateProductAttributeStatus(int id, ProductAttributeStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductAttributeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductAttribute, "AttributeId", id, ErrorMessage.Messages[ErrorCode.NotFoundProductAttribute]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ProductAttributeStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            var options = UnitOfWork.ProductAttributeOptionRepository.GetProductAttributeOptions(id).ToList();
            if (options.Any())
            {
                foreach (var option in options)
                {
                    UpdateProductAttributeOptionStatus(option.OptionId, ProductAttributeOptionStatus.InActive);
                }
            }

            entity.IsActive = status;
            UnitOfWork.ProductAttributeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Product Attribute Option
        public IEnumerable<ProductAttributeOptionDetail> GeAttributeOptions(int attributeId)
        {
            var lst = UnitOfWork.ProductAttributeOptionRepository.GetProductAttributeOptions(attributeId);
            return lst.ToDtos<ProductAttributeOption, ProductAttributeOptionDetail>();
        }
        public ProductAttributeOptionDetail GetProductAttributeOptionDetails(int productAttributeOptionId)
        {
            var entity = UnitOfWork.ProductAttributeOptionRepository.FindById(productAttributeOptionId);
            return entity.ToDto<ProductAttributeOption, ProductAttributeOptionDetail>();
        }
        public void InsertProductAttributeOption(int attributeId, ProductAttributeOptionEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (attributeId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeId, "AttributeId", attributeId, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeId]));
                throw new ValidationError(violations);
            }
            else
            {
                var item = UnitOfWork.ProductAttributeRepository.FindById(attributeId);
                if (item == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeId, "AttributeId", attributeId, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeId]));
                    throw new ValidationError(violations);
                }
            }

            if (string.IsNullOrEmpty(entry.OptionName))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeOptionName, "OptionName", entry.OptionName, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeOptionName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.OptionName.Length > 200)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeOptionName, "OptionName", entry.OptionName, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeOptionName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool isDuplicate = UnitOfWork.ProductAttributeOptionRepository.HasDataExisted(attributeId, entry.OptionName);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateProductAttributeOption, "OptionName", entry.OptionName, ErrorMessage.Messages[ErrorCode.DuplicateProductAttributeOption]));
                        throw new ValidationError(violations);
                    }
                }
            }

            if (entry.OptionValue != null && entry.OptionValue <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeOptionName, "OptionValue", entry.OptionValue, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeOptionName]));
                throw new ValidationError(violations);
            }

            var entity = entry.ToEntity<ProductAttributeOptionEntry, ProductAttributeOption>();
            entity.AttributeId = attributeId;
            entity.ListOrder = UnitOfWork.ProductAttributeOptionRepository.GetNewListOrder();

            UnitOfWork.ProductAttributeOptionRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void InsertProductAttributeOptions(int attributeId, List<ProductAttributeOptionEntry> entries)
        {
            if (entries != null && entries.Any())
            {
                foreach (var entry in entries)
                {
                    if (attributeId > 0 && !string.IsNullOrEmpty(entry.OptionName) && entry.OptionValue!=null && entry.OptionValue > 0)
                    {
                        InsertProductAttributeOption(attributeId, entry);
                    }
                }
            }
        }
        public void UpdateProductAttributeOption(ProductAttributeOptionEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductAttributeOptionRepository.FindById(entry.OptionId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductAttributeOption, "ProductAttributeOption", entry.OptionId, ErrorMessage.Messages[ErrorCode.NotFoundProductAttributeOption]));
                throw new ValidationError(violations);
            }

            if (entry.AttributeId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeId, "AttributeId", entry.AttributeId, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeId]));
                throw new ValidationError(violations);
            }
            else
            {
                var item = UnitOfWork.ProductAttributeRepository.FindById(entry.AttributeId);
                if (item == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeId, "AttributeId", entry.AttributeId, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeId]));
                    throw new ValidationError(violations);
                }
            }

            if (string.IsNullOrEmpty(entry.OptionName))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeOptionName, "OptionName", null, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeOptionName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.OptionName.Length > 200)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeOptionName, "OptionName", entry.OptionName.Length, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeOptionName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.OptionName != entry.OptionName)
                    {
                        bool isDuplicate = UnitOfWork.ProductAttributeOptionRepository.HasDataExisted(Convert.ToInt32(entry.AttributeId), entry.OptionName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateProductAttributeOption, "OptionName", entry.OptionName, ErrorMessage.Messages[ErrorCode.DuplicateProductAttributeOption]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            if (entry.OptionValue != null && entry.OptionValue < 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeOptionValue, "OptionValue", entry.OptionValue, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeOptionValue]));
                throw new ValidationError(violations);
            }

            entity.AttributeId = Convert.ToInt32(entry.AttributeId);
            entity.OptionName = entry.OptionName;
            entity.OptionValue = entry.OptionValue;
            entity.IsActive = entry.IsActive;

            UnitOfWork.ProductAttributeOptionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductAttributeOption(List<ProductAttributeOptionEditEntry> entries)
        {
            if (entries != null && entries.Any())
            {
                foreach (var entry in entries)
                {
                    UpdateProductAttributeOption(entry);
                }
            }
        }
        public void UpdateProductAttributeOptionStatus(int id, ProductAttributeOptionStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductAttributeOptionRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductAttributeOption, "ProductAttributeOption", id, ErrorMessage.Messages[ErrorCode.NotFoundProductAttributeOption]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ProductAttributeOptionStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            UnitOfWork.ProductAttributeOptionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Product Discount
        public SelectList PopulateProductDiscountSelectList(DiscountType type, ProductDiscountStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.ProductDiscountRepository.PopulateProductDiscountSelectList(type, status, selectedValue, isShowSelectText);
        }
        public IEnumerable<ProductDiscountDetail> GeProductDiscounts(int vendorId, ProductDiscountSearchEntry filter, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var violations = new List<RuleViolation>();
            if (filter.FromDate != null && filter.ToDate != null)
            {
                if (DateTime.Compare(filter.FromDate.Value, filter.ToDate.Value) > 0)
                {
                    violations.Add(new RuleViolation(ErrorCode.EndDateMustBeGreaterThanStartDate, "EndDate"));
                    throw new ValidationError(violations);
                }
            }

            var lst = UnitOfWork.ProductDiscountRepository.GetProductDiscounts(vendorId, filter.FromDate, filter.ToDate, filter.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ProductDiscount, ProductDiscountDetail>();
        }
        public ProductDiscountDetail GetProductDiscountDetails(int discountId)
        {
            var entity = UnitOfWork.ProductDiscountRepository.FindById(discountId);
            return entity.ToDto<ProductDiscount, ProductDiscountDetail>();
        }
        public void InsertProductDiscount(Guid userId, int vendorId, ProductDiscountEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            ISpecification<ProductDiscountEntry> validator = new ProductDiscountEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            bool isDuplicate = UnitOfWork.ProductDiscountRepository.HasDataExisted(entry.Quantity, entry.DiscountRate, entry.IsPercent);
            if (isDuplicate)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.DuplicateProductDiscount, "ProductDiscount",
                    $"{entry.Quantity}-{entry.DiscountRate}-{entry.IsPercent}", ErrorMessage.Messages[ErrorCode.DuplicateProductDiscount]));
                throw new ValidationError(dataViolations);
            }

            var entity = entry.ToEntity<ProductDiscountEntry, ProductDiscount>();
            entity.VendorId = vendorId;
            entity.CreatedByUserId = userId;
            entity.CreatedDate = DateTime.UtcNow;
            entity.Ip = ip;

            UnitOfWork.ProductDiscountRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductDiscount(Guid userId, int vendorId, ProductDiscountEditEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullProductDiscountEditEntry, "ProductDiscountEditEntry", null, ErrorMessage.Messages[ErrorCode.NullProductDiscountEditEntry]));
                throw new ValidationError(violations);
            }
            if (entry.DiscountRate < 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidDiscountRate, "TaxRate"));
                throw new ValidationError(violations);
            }

            if (entry.StartDate.HasValue && entry.EndDate.HasValue)
            {
                if (DateTime.Compare(entry.EndDate.Value, entry.EndDate.Value) > 0)
                {
                    violations.Add(new RuleViolation(ErrorCode.EndDateMustBeGreaterThanStartDate, "EndDate"));
                    throw new ValidationError(violations);
                }
            }

            var entity = UnitOfWork.ProductDiscountRepository.FindById(entry.DiscountId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductDiscount, "ProductDiscount", entry.DiscountId, ErrorMessage.Messages[ErrorCode.NotFoundProductDiscount]));
                throw new ValidationError(violations);
            }

            entity.DiscountCode = entry.DiscountCode;
            entity.DiscountType = entry.DiscountType;
            entity.Quantity = entry.Quantity;
            entity.DiscountRate = entry.DiscountRate;
            entity.IsPercent = entry.IsPercent;
            entity.StartDate = entry.StartDate;
            entity.EndDate = entry.EndDate;
            entity.Description = entry.Description;
            entity.IsActive = entry.IsActive;
            entity.VendorId = vendorId;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;
            entity.LastUpdatedIp = ip;

            UnitOfWork.ProductDiscountRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductDiscountStatus(Guid userId, int id, ProductDiscountStatus status)
        {
            string ip = NetworkUtils.GetIP4Address();

            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductDiscountRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductDiscount, "ProductDiscount", id, ErrorMessage.Messages[ErrorCode.NotFoundProductDiscount]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ProductDiscountStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;
            entity.LastUpdatedIp = ip;

            UnitOfWork.ProductDiscountRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Product File
        public IEnumerable<ProductFileDetail> GetList(ProductFileStatus? status, ref int? recordCount,
      string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ProductFileRepository.GetList(status, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ProductFile, ProductFileDetail>();
        }
        public IEnumerable<ProductFileDetail> GetProductFiles(int productId, ProductFileStatus? status)
        {
            var lst = UnitOfWork.ProductFileRepository.GetList(productId, status);
            return lst.ToDtos<ProductFile, ProductFileDetail>();
        }
        public ProductFileDetail GetProductFileDetails(int fileId)
        {
            var entity = UnitOfWork.ProductFileRepository.FindById(fileId);
            return entity.ToDto<ProductFile, ProductFileDetail>();
        }
        public void InsertProductFile(int vendorId, ProductFileEntry entry)
        {
            ISpecification<ProductFileEntry> validator = new ProductFileEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            bool isDuplicate = UnitOfWork.ProductFileRepository.HasDataExisted(entry.ProductId, entry.FileName);
            if (isDuplicate)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.DuplicateProductFile, "ProductFile", $"{entry.ProductId}-{entry.FileName}", ErrorMessage.Messages[ErrorCode.DuplicateProductFile]));
                throw new ValidationError(dataViolations);
            }

            var entity = entry.ToEntity<ProductFileEntry, ProductFile>();

            string filePath = entry.FileUrl;
            var fileInfo = new FileInfo(filePath);
            entity.FileExtension = fileInfo.Extension;
            entity.VendorId = vendorId;

            UnitOfWork.ProductFileRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductFile(int id, ProductFileEntry entry)
        {
            ISpecification<ProductFileEntry> validator = new ProductFileEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.ProductFileRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundProductFile, "ProductFile", id, ErrorMessage.Messages[ErrorCode.NotFoundProductFile]));
                throw new ValidationError(dataViolations);
            }

            string filePath = entry.FileUrl;
            var fileInfo = new FileInfo(filePath);
            entity.FileExtension = fileInfo.Extension;

            entity.ProductId = entry.ProductId;
            entity.FileName = entry.FileName;
            entity.FileTitle = entry.FileTitle;
            entity.FileUrl = entry.FileUrl;
            entity.Description = entry.Description;
            entity.IsImage = entry.IsImage;
            entity.Description = entry.Description;

            UnitOfWork.ProductFileRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateProductFileStatus(int id, ProductFileStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductFileRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductFile, "ProductFile", id, ErrorMessage.Messages[ErrorCode.NotFoundProductFile]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ProductFileStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            UnitOfWork.ProductFileRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Product Comment

        public IEnumerable<ProductCommentDetail> GeProductComments(ProductCommentSearchEntry filter, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ProductCommentRepository.GetProductComments(filter.CommentEmail, filter.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ProductComment, ProductCommentDetail>();
        }
        public IEnumerable<ProductCommentDetail> GeProductComments(int productId, ProductCommentStatus? status)
        {
            var lst = UnitOfWork.ProductCommentRepository.GetProductComments(productId, status);
            return lst.ToDtos<ProductComment, ProductCommentDetail>();
        }
        public ProductCommentDetail GetProductCommentDetails(int commentId)
        {
            var entity = UnitOfWork.ProductCommentRepository.FindById(commentId);
            return entity.ToDto<ProductComment, ProductCommentDetail>();
        }
        public void InsertProductComment(ProductCommentEntry entry)
        {
            ISpecification<ProductCommentEntry> validator = new ProductCommentEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<ProductCommentEntry, ProductComment>();
            entity.IsActive = ProductCommentStatus.Active;
            entity.IsReplied = entry.IsReplied;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.ProductCommentRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateProductCommentStatus(int id, ProductCommentStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ProductCommentRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductComment, "ProductComment", id, ErrorMessage.Messages[ErrorCode.NotFoundProductComment]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ProductCommentStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ProductCommentRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Product Rating

        public int GetDefaultProductRating(Guid applicationId)
        {
            var rateSetting = ApplicationService.GetRateSetting(applicationId, RatingSetting.Product);
            int ratings = Convert.ToInt32(rateSetting.Setting.KeyValue);
            return ratings;
        }
        public IEnumerable<ProductRatingDetail> GetProductRatings(int productId)
        {
            var lst = UnitOfWork.ProductRatingRepository.GetProductRatings(productId);
            return lst.ToDtos<ProductRating, ProductRatingDetail>();
        }

        public decimal InsertProductRating(ProductRatingEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry.ProductId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductId, "ProductId", entry.ProductId));
                throw new ValidationError(violations);
            }

            if (entry.Rate <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidRate, "Rate", entry.Rate));
                throw new ValidationError(violations);
            }
            
            var clientIp = NetworkUtils.GetIP4Address();

            var product = UnitOfWork.ProductRepository.FindById(entry.ProductId);
            if (product == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductId, "ProductId", entry.ProductId));
                throw new ValidationError(violations);
            }

            var rating = entry.ToEntity<ProductRatingEntry, ProductRating>();
            rating.CreatedDate = DateTime.UtcNow;
            rating.Ip = clientIp;

            var ratings = UnitOfWork.ProductRatingRepository.GetProductRatings(entry.ProductId).ToList();
            if (!ratings.Any())
            {
                UnitOfWork.ProductRatingRepository.Insert(rating);
                UnitOfWork.SaveChanges();
            }
            else
            {
                var existedRating = ratings.FirstOrDefault(x => x.ProductId == entry.ProductId && x.Ip == clientIp);
                if (existedRating == null)
                {
                    UnitOfWork.ProductRatingRepository.Insert(rating);
                    UnitOfWork.SaveChanges();
                }
                else
                {
                    existedRating.Rate = entry.Rate;
                    existedRating.LastModifiedDate = DateTime.UtcNow;
                    UnitOfWork.ProductRatingRepository.Update(existedRating);
                    UnitOfWork.SaveChanges();
                }
            }

            ratings = UnitOfWork.ProductRatingRepository.GetProductRatings(entry.ProductId).ToList();
            var rateSum = ratings.Sum(d => d.Rate);
            decimal averageRates = 0;
            averageRates = rateSum / ratings.Count;

            product.Rating = averageRates;
            UnitOfWork.ProductRepository.Update(product);
            UnitOfWork.SaveChanges();

            return averageRates;
        }

        #endregion

        #region Product Album
        public IEnumerable<ProductAlbumDetail> GetProductAlbum(int productId)
        {
            var lst = UnitOfWork.ProductAlbumRepository.GetProductAlbum(productId);
            return lst.ToDtos<ProductAlbum, ProductAlbumDetail>();
        }
        public void DeleteImageProductAlbum(int productId, int fileId)
        {
            var entity = UnitOfWork.ProductAlbumRepository.FindImageByProductId(productId, fileId);
            if (entity != null)
            {
                UnitOfWork.ProductAlbumRepository.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

        #region Product Brand

        public SelectList PopulateProductBrandSelectList(BrandStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.BrandRepository.PopulateProductBrandSelectList(status, selectedValue, isShowSelectText);
        }

        #endregion Product Brand

        #region Attributes

        public IEnumerable<AttributeDetail> GetAttributes(int productId)
        {
            var lst = UnitOfWork.AttributeRepository.GetAttributes(productId);
            return lst.ToDtos<Entities.Business.Products.Attribute, AttributeDetail>();
        }

        public AttributeDetail GetAttributeDetails(int attributeId)
        {
            var entity = UnitOfWork.AttributeRepository.FindById(attributeId);
            return entity.ToDto<Entities.Business.Products.Attribute, AttributeDetail>();
        }

        public void InsertAttribute(int categoryId,AttributeEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductAttributeEntry, "Attribute Entry", null, ErrorMessage.Messages[ErrorCode.NotFoundProductAttributeEntry]));
                throw new ValidationError(violations);
            }


            if (string.IsNullOrEmpty(entry.AttributeName))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeName, "Product Attribute Name", entry.AttributeName, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeName]));
                throw new ValidationError(violations);
            }

            var entity = entry.ToEntity<AttributeEntry, Entities.Business.Products.Attribute>();
            entity.CategoryId = categoryId;
            entity.IsActive = ProductAttributeStatus.InActive;
            entity.ListOrder = UnitOfWork.ProductAttributeRepository.GetNewListOrder();

            UnitOfWork.AttributeRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.Options != null && entry.Options.Any())
            {
                InsertAttributeOptions(entity.AttributeId, entry.Options);
            }
        }
        public void InsertAttributeOptions(int attributeId, List<AttributeOptionEntry> entries)
        {
            if (entries != null && entries.Any())
            {
                foreach (var entry in entries)
                {
                    InsertAttributeOption(attributeId, entry);
                }
            }
        }
        public void InsertAttributes(int categoryId, List<AttributeEntry> entries)
        {
            var violations = new List<RuleViolation>();
            var product = UnitOfWork.ProductRepository.FindById(categoryId);
            if (product == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullCategoryId, "CagegoryId", categoryId, ErrorMessage.Messages[ErrorCode.InvalidProductId]));
                throw new ValidationError(violations);
            }

            if (entries.Any())
            {
                foreach (var entry in entries)
                {
                    InsertAttribute(categoryId, entry);
                }
            }
            UnitOfWork.SaveChanges();
        }

        public void UpdateAttribute(int categoryId, AttributeEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductAttributeEntry, "Attribute Edit Entry", null, ErrorMessage.Messages[ErrorCode.NotFoundProductAttributeEntry]));
                throw new ValidationError(violations);
            }
            if (string.IsNullOrEmpty(entry.AttributeName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullProductAttributeName, "Attribute Name", entry.AttributeName, ErrorMessage.Messages[ErrorCode.NullProductAttributeName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (!string.IsNullOrEmpty(entry.AttributeName) && entry.AttributeName.Length > 255)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeName, "Attribute Name", entry.AttributeName.Length, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeName]));
                    throw new ValidationError(violations);
                }
            }

            if (entry.AttributeId == null || entry.AttributeId == 0)
            {
                var attributeEntry = new AttributeEntry
                {
                    AttributeName = entry.AttributeName,
                    IsActive = entry.IsActive,
                    Options = entry.Options
                };

                InsertAttribute(categoryId, attributeEntry);
            }
            else
            {
                var entity = UnitOfWork.AttributeRepository.FindById(entry.AttributeId);
                if (entity == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundProductAttribute, "AttributeId",
                        entry.AttributeId, ErrorMessage.Messages[ErrorCode.NotFoundProductAttribute]));
                    throw new ValidationError(violations);
                }

                entity.CategoryId = categoryId;
                entity.AttributeName = entry.AttributeName;
                entity.IsActive = entry.IsActive;

                UnitOfWork.AttributeRepository.Update(entity);
                UnitOfWork.SaveChanges();

                if (entry.Options != null && entry.Options.Any())
                {
                    InsertAttributeOptions(entity.AttributeId, entry.Options);
                }

                if (entry.ExistedOptions != null && entry.ExistedOptions.Any())
                {
                    foreach (var option in entry.ExistedOptions)
                    {
                        if (option.AttributeId != null && option.OptionId != null && option.AttributeId > 0 &&
                            option.OptionId > 0)
                        {
                            var optionEntry = new AttributeOptionEditEntry
                            {
                                AttributeId = entity.AttributeId,
                                OptionId = option.OptionId,
                                OptionName = option.OptionName,
                                OptionValue = option.OptionValue,
                                IsActive = option.IsActive
                            };
                            UpdateAttributeOption(optionEntry);
                        }
                        else
                        {
                            var optionEntry = new AttributeOptionEntry
                            {
                                OptionName = option.OptionName,
                                OptionValue = option.OptionValue,
                                IsActive = option.IsActive
                            };
                            InsertAttributeOption(entity.AttributeId, optionEntry);
                        }
                    }
                }
            }
        }

        public void UpdateAttributes(int cagogoryId, List<AttributeEditEntry> entries)
        {
            if (entries.Any())
            {
                foreach (var entry in entries)
                {
                    UpdateAttribute(cagogoryId, entry);
                }
            }
        }

        public void UpdateAttributeStatus(int id, ProductAttributeStatus status)
        {
               var violations = new List<RuleViolation>();
            var entity = UnitOfWork.AttributeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductAttribute, "AttributeId", id, ErrorMessage.Messages[ErrorCode.NotFoundProductAttribute]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ProductAttributeStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            var options = UnitOfWork.AttributeOptionRepository.GetAttributeOptions(id).ToList();
            if (options.Any())
            {
                foreach (var option in options)
                {
                    UpdateAttributeOptionStatus(option.OptionId, ProductAttributeOptionStatus.InActive);
                }
            }

            entity.IsActive = status;
            UnitOfWork.AttributeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Attributes Option

        public IEnumerable<AttributeOptionDetail> GetAttributeOptions(int attributeId)
        {
            var lst = UnitOfWork.AttributeOptionRepository.GetAttributeOptions(attributeId);
            return lst.ToDtos<AttributeOption, AttributeOptionDetail>();
        }

        public AttributeOptionDetail GetAttributeOptionDetails(int attributeOptionId)
        {
            var entity = UnitOfWork.AttributeOptionRepository.FindById(attributeOptionId);
            return entity.ToDto<AttributeOption, AttributeOptionDetail>();
        }

        public void InsertAttributeOption(int attributeId, AttributeOptionEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (attributeId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeId, "AttributeId", attributeId, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeId]));
                throw new ValidationError(violations);
            }
            else
            {
                var item = UnitOfWork.AttributeRepository.FindById(attributeId);
                if (item == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeId, "AttributeId", attributeId, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeId]));
                    throw new ValidationError(violations);
                }
            }

            if (string.IsNullOrEmpty(entry.OptionName))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeOptionName, "OptionName", entry.OptionName, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeOptionName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.OptionName.Length > 200)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeOptionName, "OptionName", entry.OptionName, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeOptionName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool isDuplicate = UnitOfWork.AttributeOptionRepository.HasDataExisted(attributeId, entry.OptionName);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateProductAttributeOption, "OptionName", entry.OptionName, ErrorMessage.Messages[ErrorCode.DuplicateProductAttributeOption]));
                        throw new ValidationError(violations);
                    }
                }
            }

            if (entry.OptionValue != null && entry.OptionValue <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeOptionName, "OptionValue", entry.OptionValue, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeOptionName]));
                throw new ValidationError(violations);
            }

            var entity = entry.ToEntity<AttributeOptionEntry,AttributeOption>();
            entity.AttributeId = attributeId;
            entity.ListOrder = UnitOfWork.AttributeOptionRepository.GetNewListOrder();

            UnitOfWork.AttributeOptionRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateAttributeOption(AttributeOptionEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.AttributeOptionRepository.FindById(entry.OptionId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductAttributeOption, "AttributeOption", entry.OptionId, ErrorMessage.Messages[ErrorCode.NotFoundProductAttributeOption]));
                throw new ValidationError(violations);
            }

            if (entry.AttributeId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeId, "AttributeId", entry.AttributeId, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeId]));
                throw new ValidationError(violations);
            }
            else
            {
                var item = UnitOfWork.AttributeRepository.FindById(entry.AttributeId);
                if (item == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeId, "AttributeId", entry.AttributeId, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeId]));
                    throw new ValidationError(violations);
                }
            }

            if (string.IsNullOrEmpty(entry.OptionName))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeOptionName, "OptionName", null, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeOptionName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.OptionName.Length > 200)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeOptionName, "OptionName", entry.OptionName.Length, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeOptionName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.OptionName != entry.OptionName)
                    {
                        bool isDuplicate = UnitOfWork.AttributeOptionRepository.HasDataExisted(Convert.ToInt32(entry.AttributeId), entry.OptionName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateProductAttributeOption, "OptionName", entry.OptionName, ErrorMessage.Messages[ErrorCode.DuplicateProductAttributeOption]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            if (entry.OptionValue != null && entry.OptionValue < 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidProductAttributeOptionValue, "OptionValue", entry.OptionValue, ErrorMessage.Messages[ErrorCode.InvalidProductAttributeOptionValue]));
                throw new ValidationError(violations);
            }

            entity.AttributeId = Convert.ToInt32(entry.AttributeId);
            entity.OptionName = entry.OptionName;
            entity.OptionValue = entry.OptionValue;
            entity.IsActive = entry.IsActive;

            UnitOfWork.AttributeOptionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateAttributeOptionStatus(int id, ProductAttributeOptionStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.AttributeOptionRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductAttributeOption, "AttributeOption", id, ErrorMessage.Messages[ErrorCode.NotFoundProductAttributeOption]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ProductAttributeOptionStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            UnitOfWork.AttributeOptionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Dipose

        private bool _disposed;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    ApplicationService = null;
                    CurrencyService = null;
                    DocumentService = null;
                    VendorService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        public void UpdateAttribute(int productId, ProductAttributeEditEntry entry)
        {
            throw new NotImplementedException();
        }

        public void UpdateAttributes(int productId, List<ProductAttributeEditEntry> entries)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AttributeDetail> GetAttributes(AttributeDetail filter, ref int? recordCount, string orderBy, int? page, int defaultPageSize)
        {
            var lst = UnitOfWork.AttributeRepository.GetAttributes(filter.AttributeName, filter.IsActive, ref recordCount, orderBy, page, defaultPageSize);
            return lst.ToDtos<Entities.Business.Products.Attribute, AttributeDetail>();
        }
        #endregion
    }
}
