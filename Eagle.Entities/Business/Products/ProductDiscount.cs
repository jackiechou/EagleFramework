using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Products
{
    [Table("Production.ProductDiscount")]
    public class ProductDiscount : BaseEntity
    {
        [NotMapped]
        public int Id => DiscountId;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiscountId { get; set; }
        public string DiscountCode { get; set; }
        public DiscountType DiscountType { get; set; }
        public int? Quantity { get; set; }
        public decimal DiscountRate { get; set; }
        public bool IsPercent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public ProductDiscountStatus? IsActive { get; set; }

        public int VendorId { get; set; }
    }
}
