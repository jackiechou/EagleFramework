using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Common.Utilities;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Manufacturers;
using Eagle.Entities.Business.Products;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class ProductRepository: RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<Product> GetProductList(int vendorId, string languageCode, string searchText, int? categoryId, DateTime? startDate, DateTime? endDate,
         ProductStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from p in DataContext.Get<Product>()
                         where p.VendorId == vendorId && p.LanguageCode == languageCode && (status == null || p.Status == status)
                         select p;

            if (categoryId != null && categoryId > 0)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(searchText))
                query = query.Where(x => x.ProductCode.Contains(searchText)
                            || x.ProductName.Contains(searchText));

            if (startDate != null && endDate == null)
            {
                query = query.Where(x => x.StartDate >= startDate);
            }
            if (startDate == null && endDate != null)
            {
                query = query.Where(x => x.EndDate <= endDate);
            }
            if (startDate != null && endDate != null)
            {
                query = query.Where(x => x.StartDate >= startDate && x.EndDate <= endDate);
            }
            return query.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<ProductInfo> GetDiscountedProducts(int vendorId, int? categoryId, ProductStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from p in DataContext.Get<Product>()
                join c in DataContext.Get<ProductCategory>() on p.CategoryId equals c.CategoryId into pcLst
                from pc in pcLst.DefaultIfEmpty()
                join t in DataContext.Get<ProductType>() on p.ProductTypeId equals t.TypeId into ptLst
                from pt in ptLst.DefaultIfEmpty()
                join tr in DataContext.Get<ProductTaxRate>() on p.TaxRateId equals tr.TaxRateId into ptrLst
                from ptr in ptrLst.DefaultIfEmpty()
                join d in DataContext.Get<ProductDiscount>() on p.DiscountId equals d.DiscountId into pdLst
                from pd in pdLst.DefaultIfEmpty()
                join l in DataContext.Get<Language>() on p.LanguageCode equals l.LanguageCode into plLst
                from pl in plLst.DefaultIfEmpty()
                join m in DataContext.Get<Manufacturer>() on p.ManufacturerId equals m.ManufacturerId into pmLst
                from pm in pmLst.DefaultIfEmpty()
                where p.VendorId == vendorId && (status == null || p.Status == status) && p.DiscountId != null && p.DiscountId > 1
                select new ProductInfo
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
                    Specification = p.Specification,
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
                    BrandId = p.BrandId,
                    VendorId = p.VendorId,
                    ProductTypeId = p.ProductTypeId,
                    DiscountId = p.DiscountId,
                    ProductCategory = pc,
                    ProductType = pt,
                    ProductTaxRate = ptr,
                    ProductDiscount = pd,
                    Manufacturer = pm
                };

            if (categoryId != null && categoryId > 0)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }
            return query.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<ProductInfo> GetLastestProducts(int vendorId, int? categoryId, ProductStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from p in DataContext.Get<Product>()
                where p.VendorId == vendorId && (status == null || p.Status == status) && p.DiscountId != null && p.DiscountId > 0
                select new ProductInfo
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
                    Specification = p.Specification,
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
                    BrandId = p.BrandId,
                    VendorId = p.VendorId,
                    ProductTypeId = p.ProductTypeId,
                    DiscountId = p.DiscountId,
                };

            if (categoryId != null && categoryId > 0)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }
            return query.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<ProductInfo> GetProductsByManufacturer(int vendorId, int manufacturerId, ProductStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from p in DataContext.Get<Product>()
                join c in DataContext.Get<ProductCategory>() on p.CategoryId equals c.CategoryId into pcLst
                from pc in pcLst.DefaultIfEmpty()
                join t in DataContext.Get<ProductType>() on p.ProductTypeId equals t.TypeId into ptLst
                from pt in ptLst.DefaultIfEmpty()
                join tr in DataContext.Get<ProductTaxRate>() on p.TaxRateId equals tr.TaxRateId into ptrLst
                from ptr in ptrLst.DefaultIfEmpty()
                join d in DataContext.Get<ProductDiscount>() on p.DiscountId equals d.DiscountId into pdLst
                from pd in pdLst.DefaultIfEmpty()
                join l in DataContext.Get<Language>() on p.LanguageCode equals l.LanguageCode into plLst
                from pl in plLst.DefaultIfEmpty()
                join m in DataContext.Get<Manufacturer>() on p.ManufacturerId equals m.ManufacturerId into pmLst
                from pm in pmLst.DefaultIfEmpty()
                where p.VendorId == vendorId && p.ManufacturerId == manufacturerId && (status == null || p.Status == status) && p.DiscountId != null && p.DiscountId > 0
                select new ProductInfo
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
                    Specification = p.Specification,
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
                    BrandId = p.BrandId,
                    VendorId = p.VendorId,
                    ProductTypeId = p.ProductTypeId,
                    DiscountId = p.DiscountId,
                    ProductCategory = pc,
                    ProductType = pt,
                    ProductTaxRate = ptr,
                    ProductDiscount = pd,
                    Manufacturer = pm
                };
            return query.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        
        public IEnumerable<ProductInfo> GetProducts(string languageCode, int vendorId, int? categoryId, string searchText, 
            DateTime? startDate, DateTime? endDate, decimal? minPrice, decimal? maxPrice,
           ProductStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from p in DataContext.Get<Product>()
                join c in DataContext.Get<ProductCategory>() on p.CategoryId equals c.CategoryId into pcLst
                from pc in pcLst.DefaultIfEmpty()
                join t in DataContext.Get<ProductType>() on p.ProductTypeId equals t.TypeId into ptLst
                from pt in ptLst.DefaultIfEmpty()
                join tr in DataContext.Get<ProductTaxRate>() on p.TaxRateId equals tr.TaxRateId into ptrLst
                from ptr in ptrLst.DefaultIfEmpty()
                join d in DataContext.Get<ProductDiscount>() on p.DiscountId equals d.DiscountId into pdLst
                from pd in pdLst.DefaultIfEmpty()
                join m in DataContext.Get<Manufacturer>() on p.ManufacturerId equals m.ManufacturerId into pmLst
                from pm in pmLst.DefaultIfEmpty()
                where p.VendorId == vendorId && (status==null || p.Status==status)
                orderby p.ProductId descending
                select new ProductInfo
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
                    Specification = p.Specification,
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
                    BrandId = p.BrandId,
                    VendorId = p.VendorId,
                    ProductTypeId = p.ProductTypeId,
                    DiscountId = p.DiscountId,
                    ProductCategory = pc,
                    ProductType = pt,
                    ProductTaxRate = ptr,
                    ProductDiscount = pd,
                    Manufacturer = pm
                };
            
            if (!string.IsNullOrEmpty(languageCode))
            {
                query = query.Where(x => x.LanguageCode.ToLower() == languageCode.ToLower());
            }

            if (categoryId != null && categoryId > 0)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }
            
            if (!string.IsNullOrEmpty(searchText))
                query = query.Where(x => x.ProductCode.Contains(searchText)
                            || x.ProductName.Contains(searchText)
                            || x.ProductCategory.CategoryCode.Contains(searchText)
                            );

            if (startDate != null && endDate == null)
            {
                query = query.Where(x => x.StartDate >= startDate);
            }
            if (startDate == null && endDate != null)
            {
                query = query.Where(x => x.EndDate <= endDate);
            }
            if (startDate != null && endDate != null)
            {
                query = query.Where(x => x.StartDate >= startDate && x.EndDate <= endDate);
            }
            if (minPrice.HasValue)
            {
                query = query.Where(x => x.GrossPrice >= minPrice.Value);
            }
            if(maxPrice.HasValue)
            {
                query = query.Where(x => x.GrossPrice <= maxPrice.Value);
            }
            return query.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
       
        public ProductInfo GetDetailsByProductId(int productId)
        {
            var enitty = (from p in DataContext.Get<Product>()
                          join c in DataContext.Get<ProductCategory>() on p.CategoryId equals c.CategoryId into pcLst
                          from pc in pcLst.DefaultIfEmpty()
                          join t in DataContext.Get<ProductType>() on p.ProductTypeId equals t.TypeId into ptLst
                          from pt in ptLst.DefaultIfEmpty()
                          join tr in DataContext.Get<ProductTaxRate>() on p.TaxRateId equals tr.TaxRateId into ptrLst
                          from ptr in ptrLst.DefaultIfEmpty()
                          join d in DataContext.Get<ProductDiscount>() on p.DiscountId equals d.DiscountId into pdLst
                          from pd in pdLst.DefaultIfEmpty()
                          join l in DataContext.Get<Language>() on p.LanguageCode equals l.LanguageCode into plLst
                          from pl in plLst.DefaultIfEmpty()
                          join m in DataContext.Get<Manufacturer>() on p.ManufacturerId equals m.ManufacturerId into pmLst
                          from pm in pmLst.DefaultIfEmpty()
                          where p.ProductId == productId
                          select new ProductInfo
                          {
                              ProductId =p.ProductId,
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
                              ListOrder = p.ListOrder,
                              Views = p.Views,
                              SmallPhoto = p.SmallPhoto,
                              LargePhoto = p.LargePhoto,
                              ShortDescription = p.ShortDescription,
                              Specification =p.Specification,
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
                              BrandId = p.BrandId,
                              VendorId = p.VendorId,
                              ProductTypeId = p.ProductTypeId,
                              DiscountId = p.DiscountId,
                              ProductCategory = pc,
                              ProductType = pt,
                              ProductTaxRate = ptr,
                              ProductDiscount = pd,
                              Manufacturer = pm
                          }).FirstOrDefault();

            return enitty;
        }

        public ProductInfo GetDetailsByProductCode(string productCode)
        {
            var enitty = (from p in DataContext.Get<Product>()
                          join c in DataContext.Get<ProductCategory>() on p.CategoryId equals c.CategoryId into pcLst
                          from pc in pcLst.DefaultIfEmpty()
                          join t in DataContext.Get<ProductType>() on p.ProductTypeId equals t.TypeId into ptLst
                          from pt in ptLst.DefaultIfEmpty()
                          join tr in DataContext.Get<ProductTaxRate>() on p.TaxRateId equals tr.TaxRateId into ptrLst
                          from ptr in ptrLst.DefaultIfEmpty()
                          join d in DataContext.Get<ProductDiscount>() on p.DiscountId equals d.DiscountId into pdLst
                          from pd in pdLst.DefaultIfEmpty()
                          join l in DataContext.Get<Language>() on p.LanguageCode equals l.LanguageCode into plLst
                          from pl in plLst.DefaultIfEmpty()
                          join m in DataContext.Get<Manufacturer>() on p.ManufacturerId equals m.ManufacturerId into pmLst
                          from pm in pmLst.DefaultIfEmpty()
                          where p.ProductCode.ToLower().Contains(productCode.ToLower())
                          select new ProductInfo
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
                              Specification = p.Specification,
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
                              BrandId = p.BrandId,
                              VendorId = p.VendorId,
                              ProductTypeId = p.ProductTypeId,
                              DiscountId = p.DiscountId,
                              ProductCategory = pc,
                              ProductType = pt,
                              ProductTaxRate = ptr,
                              ProductDiscount = pd,
                              Manufacturer = pm
                          }).FirstOrDefault();
            return enitty;
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<Product>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
        public bool HasDataExisted(int? productTypeId, int categoryId, string productCode, string productName)
        {
            var query = (from p in DataContext.Get<Product>()
                where (productTypeId == null || p.ProductTypeId == productTypeId)
                      && (productCode == null || p.ProductCode == productCode)
                      && p.CategoryId == categoryId
                      && p.ProductName.ToUpper().Equals(productName.ToUpper())
                select p).FirstOrDefault();
            return query != null;
        }
        public bool HasProductNameExisted(string productName)
        {
            var query = (from x in DataContext.Get<Product>() where x.ProductName.ToLower().Contains(productName.ToLower()) select x).FirstOrDefault();
            return query != null;
        }
        public bool HasProductCodeExisted(string productCode)
        {
            var query = (from x in DataContext.Get<Product>() where x.ProductCode == productCode.ToLower() select x).FirstOrDefault();
            return query != null;
        }

        public string GenerateProductCode(int maxLetters)
        {
            int newId = 1;
            var query = from u in DataContext.Get<Product>() select u.ProductId;
            if (query.Any())
            {
                newId = query.Max() + 1;
            }
            return StringUtils.GenerateCode(newId.ToString(), maxLetters);
        }

        public string GenerateProductCode(int maxLetters, string seedId)
        {
            int newId = 1;
            var query = from u in DataContext.Get<Product>() select u.ProductId;
            if (query.Any())
            {
                newId = query.Max() + 1;
            }

            if (string.IsNullOrEmpty(seedId))
            {
                return StringUtils.GenerateCode(newId.ToString(), maxLetters);
            }
            else
            {
                return maxLetters > seedId.Length + 1 ? StringUtils.GenerateCode(seedId, maxLetters) : $"{seedId.ToUpper()}";
            }
           
        }

        public bool HasBrandExisted(int brandId)
        {
            var item = (from o in DataContext.Get<Product>()
                        where o.BrandId == brandId
                        select o).FirstOrDefault();
            return item != null;
        }
    }
}
