using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Transactions
{
    [Table("Sales.Promotion")]
    public class Promotion : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PromotionId { get; set; }
        public PromotionType PromotionType { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionTitle { get; set; }
        public decimal PromotionValue { get; set; }
        public bool IsPercent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public PromotionStatus IsActive { get; set; }

        public int VendorId { get; set; }
    }
}
