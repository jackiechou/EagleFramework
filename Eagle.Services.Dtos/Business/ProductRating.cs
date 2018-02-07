using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class ProductRatingDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RatingId")]
        public int RatingId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductId")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int? CustomerId { get; set; }

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
    public class ProductRatingEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ProductId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int? CustomerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Rate")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int Rate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalRates")]
        public int? TotalRates { get; set; }
    }
}
