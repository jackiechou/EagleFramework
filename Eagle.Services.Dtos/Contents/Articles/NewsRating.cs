using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Articles
{
    public class NewsRatingEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "NewsId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int NewsId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TargetId")]
        public int? TargetId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Rate")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int Rate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalRates")]
        public int? TotalRates { get; set; }
    }
    public class NewsRatingDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RatingId")]
        public int RatingId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NewsId")]
        public int NewsId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TargetId")]
        public int? TargetId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Rate")]
        public int Rate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalRates")]
        public int? TotalRates { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Ip")]
        public string Ip { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastUpdatedIp")]
        public string LastUpdatedIp { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }
    }
}
