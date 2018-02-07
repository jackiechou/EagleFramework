using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class ProductSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        public int? CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductName")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MinPrice")]
        public decimal? MinPrice { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MaxPrice")]
        public decimal? MaxPrice { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ProductStatus? Status { get; set; }
    }
    
    public class ProductEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectCategory")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ManufacturerId")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int? ManufacturerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int? VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductTypeId")]
        public int? ProductTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BrandId")]
        public int? BrandId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ProductCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 2)]
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountId")]
        public int? DiscountId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NetPrice")]
        public decimal? NetPrice { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "GrossPrice")]
        public decimal? GrossPrice { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRateId")]
        public int? TaxRateId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UnitsInStock")]
        public int? UnitsInStock { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UnitsOnOrder")]
        public int? UnitsOnOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UnitsInAPackage")]
        public int? UnitsInAPackage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UnitsInBox")]
        public int? UnitsInBox { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Weight")]
        public decimal? Weight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UnitOfWeightMeasure")]
        public string UnitOfWeightMeasure { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Length")]
        public decimal? Length { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Width")]
        public decimal? Width { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Height")]
        public decimal? Height { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UnitOfDimensionMeasure")]
        public string UnitOfDimensionMeasure { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Url")]
        public string Url { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MinPurchaseQty")]
        public int? MinPurchaseQty { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MaxPurchaseQty")]
        public int? MaxPurchaseQty { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Views")]
        public int? Views { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "ShortDescription")]
        [StringLength(600, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string ShortDescription { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Specification")]
        public string Specification { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Availability")]
        public string Availability { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PurchaseScope")]
        public string PurchaseScope { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Warranty")]
        public string Warranty { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsOnline")]
        public bool? IsOnline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ProductStatus Status { get; set; }

        public List<ProductAttributeEntry> Attributes { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AttachFile")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectFile")]
        public HttpPostedFileBase File { get; set; }

        public HttpPostedFileBase[] ProductGalleryFiles { get; set; }
    }
    public class ProductEditEntry : ProductEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ProductId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PhotoFileName")]
        public int? SmallPhoto { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ThumbnailPhotoFileName")]
        public int? LargePhoto { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InfoNotification")]
        public bool? InfoNotification { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PriceNotification")]
        public bool? PriceNotification { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "QtyNotification")]
        public bool? QtyNotification { get; set; }

        public List<ProductAttributeEditEntry> ExistedAttributes { get; set; }

        public List<ProductAlbumEditEntry> ExistedProductAlbumFiles { get; set; }

    }
    public class ProductDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ProductId")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductCode")]
        public string ProductCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductAlias")]
        public string ProductAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountId")]
        public int? DiscountId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NetPrice")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? NetPrice { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "GrossPrice")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? GrossPrice { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRateId")]
        public int? TaxRateId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UnitsInStock")]
        public int? UnitsInStock { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UnitsOnOrder")]
        public int? UnitsOnOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UnitsInAPackage")]
        public int? UnitsInAPackage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UnitsInBox")]
        public int? UnitsInBox { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Weight")]
        public decimal? Weight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UnitOfWeightMeasure")]
        public string UnitOfWeightMeasure { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Length")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? Length { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Width")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? Width { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Height")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? Height { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UnitOfDimensionMeasure")]
        public string UnitOfDimensionMeasure { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Url")]
        public string Url { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MinPurchaseQty")]
        public int? MinPurchaseQty { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MaxPurchaseQty")]
        public int? MaxPurchaseQty { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Rating")]
        public decimal? Rating { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Views")]
        public int? Views { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SmallPhoto")]
        public int? SmallPhoto { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LargePhoto")]
        public int? LargePhoto { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(LanguageResource), Name = "ShortDescription")]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string ShortDescription { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Specification")]
        public string Specification { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Availability")]
        public string Availability { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PurchaseScope")]
        public string PurchaseScope { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Warranty")]
        public string Warranty { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsOnline")]
        public bool? IsOnline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InfoNotification")]
        public bool? InfoNotification { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PriceNotification")]
        public bool? PriceNotification { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "QtyNotification")]
        public bool? QtyNotification { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ProductStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageCode")]
        public string LanguageCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ManufacturerId")]
        public int? ManufacturerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int? VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductTypeId")]
        public int? ProductTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BrandId")]
        public int? BrandId { get; set; }

    }
    public class ProductInfoDetail : ProductDetail
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SmallPhotoUrl")]
        public string SmallPhotoUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LargePhotoUrl")]
        public string LargePhotoUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRate")]
        public decimal? TaxRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountRate")]
        public decimal? DiscountRate { get; set; }

        public virtual ProductCategoryDetail ProductCategory { get; set; }
        public virtual ProductDiscountDetail ProductDiscount { get; set; }
        public virtual ProductTaxRateDetail ProductTaxRate { get; set; }
        public virtual ProductTypeDetail ProductType { get; set; }
        public virtual ManufacturerDetail Manufacturer { get; set; }
        public virtual IEnumerable<ProductCommentDetail> Comments { get; set; }
    }

    public class ProductByCategory
    {
        public int CategoryId { get; set; }
        public ProductCategoryDetail Category { get; set; }
        public IPagedList<ProductInfoDetail> Products{ get; set; }
}
}
