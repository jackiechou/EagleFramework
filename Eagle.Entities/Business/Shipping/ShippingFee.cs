using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Shipping
{
    [Table("ShippingFee", Schema = "Sales")]
    public class ShippingFee : EntityBase
    {
        public ShippingFee()
        {
            IsActive = true;
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShippingFeeId { get; set; }
        public string ShippingFeeName { get; set; }
        public string ZipStart { get; set; }
        public string ZipEnd { get; set; }
        public decimal WeightStart { get; set; }
        public decimal WeightEnd { get; set; }
        public decimal RateFee { get; set; }
        public decimal PackageFee { get; set; }
        public decimal Vat { get; set; }
        public string CurrencyCode { get; set; }
        public int ListOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public int ShippingCarrierId { get; set; }
        public int ShippingMethodId { get; set; }
    }
}
