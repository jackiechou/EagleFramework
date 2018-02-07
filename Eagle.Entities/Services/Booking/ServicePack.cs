using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Services.Booking
{
    [Table("ServicePack",Schema = "Booking")]
    public class ServicePack: EntityBase
    {
        public ServicePack()
        {
            Capacity = 1;
            TypeId = 1;
            PackageCode = Guid.NewGuid().ToString();
            Status = ServicePackStatus.Active;
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PackageId { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public int? AvailableQuantity { get; set; }
        public int Capacity { get; set; }
        public int? DurationId { get; set; }
        public int? DiscountId { get; set; }
        public int? TaxRateId { get; set; }
        public decimal? PackageFee { get; set; }
        public decimal? TotalFee { get; set; }
        public decimal? Weight { get; set; }
        public string CurrencyCode { get; set; }
        public int? FileId { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public decimal? Rating { get; set; }
        public int? TotalViews { get; set; }
        public int? ListOrder { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public ServicePackStatus Status { get; set; }
    }
}
