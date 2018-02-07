using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Products
{
    [Table("Product",Schema = "Production")]
    public class Product : BaseEntity
    {
        public Product()
        {
            Status = ProductStatus.Active;
        }

        [NotMapped]
        public int Id => ProductId;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductAlias { get; set; }
        public decimal? NetPrice { get; set; }
        public decimal? GrossPrice { get; set; }
        public int? UnitsInStock { get; set; }
        public int? UnitsOnOrder { get; set; }
        public int? UnitsInAPackage { get; set; }
        public int? UnitsInBox { get; set; }
        public string Unit { get; set; }
        public decimal? Weight { get; set; }
        public string UnitOfWeightMeasure { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public string UnitOfDimensionMeasure { get; set; }
        public string Url { get; set; }
        public int? MinPurchaseQty { get; set; }
        public int? MaxPurchaseQty { get; set; }
        public decimal? Rating { get; set; }
        public int? ListOrder { get; set; }
        public int? Views { get; set; }
        public int? SmallPhoto { get; set; }
        public int? LargePhoto { get; set; }
        public string ShortDescription { get; set; }
        public string Specification { get; set; }
        public string Availability { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PurchaseScope { get; set; }
        public string Warranty { get; set; }
        public bool? IsOnline { get; set; }
        public bool? InfoNotification { get; set; }
        public bool? PriceNotification { get; set; }
        public bool? QtyNotification { get; set; }
        public ProductStatus Status { get; set; }


        public int CategoryId { get; set; }
        public string LanguageCode { get; set; }
        public string CurrencyCode { get; set; }
        public int? DiscountId { get; set; }
        public int? ManufacturerId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? TaxRateId { get; set; }
        public int? VendorId { get; set; }
        public int? BrandId { get; set; }
    }
}
