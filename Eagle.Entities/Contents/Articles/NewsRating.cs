using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Contents.Articles
{
    [Table("NewsRating")]
    public class NewsRating : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RatingId { get; set; }
        public int NewsId { get; set; }
        public int? TargetId { get; set; }
        public int Rate { get; set; }
        public int? TotalRates { get; set; }
        public string Ip { get; set; }
        public string LastUpdatedIp { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
