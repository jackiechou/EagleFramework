using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Products
{
    [Table("Production.ProductTaxRate")]
    public class ProductTaxRate : EntityBase
    {
        public ProductTaxRate()
        {
            TaxRate = 0;
            IsPercent = true;
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaxRateId { get; set; }
        public decimal TaxRate { get; set; }
        public bool IsPercent { get; set; }
        public string Description { get; set; }
        public ProductTaxRateStatus IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
