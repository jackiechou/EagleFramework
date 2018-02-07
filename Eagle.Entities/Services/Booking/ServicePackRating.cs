using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Booking
{
    [Table("ServicePackRating", Schema = "Booking")]
    public class ServicePackRating : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RatingId { get; set; }
        public int PackageId { get; set; }
        public int? CustomerId { get; set; }
        public int Rate { get; set; }
        public int? TotalRates { get; set; }
        public string Ip { get; set; }
        public string LastUpdatedIp { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
