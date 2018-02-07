using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Services.Booking
{
    [Table("ServiceDiscount", Schema = "Booking")]
    public class ServiceDiscount : EntityBase
    {
        public ServiceDiscount()
        {
            StartDate = DateTime.UtcNow;
            IsActive = ServiceDiscountStatus.Active;
        }

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
        public ServiceDiscountStatus IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
