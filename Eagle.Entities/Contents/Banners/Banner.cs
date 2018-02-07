using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Banners
{
    [Table("Banner")]
    public class Banner : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BannerId { get; set; }
        public string BannerTitle { get; set; }
        public string BannerContent { get; set; }
        public string AltText { get; set; }
        public int? FileId { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string Advertiser { get; set; }
        public string Tags { get; set; }
        public int? ListOrder { get; set; }
        public int? ClickThroughs { get; set; }
        public int? Impressions { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Target { get; set; }
        public BannerStatus Status { get; set; }
        
        public int ScopeId { get; set; }
        public int TypeId { get; set; }
        public int VendorId { get; set; }
        public string LanguageCode { get; set; }

        //public virtual Language Language { get; set; }
        //public virtual ICollection<BannerPosition> BannerPositions { get; set; }
        //public virtual BannerScope BannerScope { get; set; }
        //public virtual BannerType BannerType { get; set; }
    }
}
