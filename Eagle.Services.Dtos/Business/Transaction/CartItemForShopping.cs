using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.Business.Personnel;

namespace Eagle.Services.Dtos.Business.Transaction
{
    public class CartItemForShopping : CartItem
    {
        public ItemType TypeId { get; set; }
        public int? EmployeeId { get; set; }
        public int? PeriodGroupId { get; set; }
        public int? FromPeriod { get; set; }
        public int? ToPeriod { get; set; }
        public string Comment { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
        public EmployeeInfoDetail Employee { get; set; }
        public CustomerInfoDetail Customer { get; set; }
        public ItemDetail Detail { get; set; }

        // CartItem constructor just needs a productId or productNo
        public CartItemForShopping(int id) : base(id)
        {
            Id = id;
        }
    }

    public class ItemDetail: DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ItemId")]
        public int ItemId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ItemCode")]
        public string ItemCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Type")]
        public ItemType TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ItemName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ItemName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ItemAlias")]
        public string ItemAlias { get; set; }

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

        [Display(ResourceType = typeof(LanguageResource), Name = "SmallPhotoUrl")]
        public string SmallPhotoUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LargePhotoUrl")]
        public string LargePhotoUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRate")]
        public decimal? TaxRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountRate")]
        public decimal? DiscountRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PeriodGroupId")]
        public int? PeriodGroupId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "From")]
        public int? FromPeriod { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "To")]
        public int? ToPeriod { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int? EmployeeId { get; set; }

        //public virtual ProductCategoryDetail ProductCategory { get; set; }
        //public virtual ProductDiscountDetail ProductDiscount { get; set; }
        //public virtual ProductTaxRateDetail ProductTaxRate { get; set; }
        //public virtual ProductTypeDetail ProductType { get; set; }
        public virtual ManufacturerDetail Manufacturer { get; set; }
    }
    
    public class CartInfoForShopping : DtoBase
    {
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

        public decimal Weights { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Promotion { get; set; }
        public decimal ShippingCharge { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string CurrencyCode { get; set; }
        public int Count { get; set; }
        public int CustomerId { get; set; }
        public int? ShippingMethodId { get; set; }

        public CustomerInfoDetail CustomerInfo { get; set; }
        public ShipmentInfo ShipmentInfo { get; set; }
        public PromotionInfo PromotionInfo { get; set; }
        public OrderPaymentEntry PaymentInfo { get; set; }
        public List<CartItemForShopping> Items { get; set; }

        public bool IsAvailable
        {
            get
            {
                bool result = Items.All(p => p.Status == CartItemStatus.Available);
                return result;
            }
        }
    }
}
