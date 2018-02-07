using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Articles
{
    [Table("News")]
    public class News : BaseEntity
    {
        public News()
        {
            Status = NewsStatus.Published;
        }

        [NotMapped]
        public int Id => NewsId;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Headline { get; set; }
        public string Alias { get; set; }
        public string Summary { get; set; }
        public string Authors { get; set; }
        public string NavigateUrl { get; set; }
        public int? FrontImage { get; set; }
        public int? MainImage { get; set; }
        public string MainText { get; set; }
        public string Source { get; set; }
        public string Tags { get; set; }
        public int? ListOrder { get; set; }
        public decimal? TotalRates { get; set; }
        public int? TotalViews { get; set; }
        public DateTime PostedDate { get; set; }
        public NewsStatus Status { get; set; }

        public int CategoryId { get; set; }
        public int? VendorId { get; set; }
    }
}
