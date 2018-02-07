using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Booking
{
    [Table("Booking.ServiceTaxRate")]
    public class ServiceTaxRate : EntityBase
    {
        public ServiceTaxRate()
        {
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaxRateId { get; set; }
        public decimal? TaxRate { get; set; }
        public bool IsPercent { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
