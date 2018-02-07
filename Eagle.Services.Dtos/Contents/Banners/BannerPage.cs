using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Banners
{
    public class BannerPageDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "BannerId")]
        public int BannerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        public int PageId { get; set; }
    }

    public class BannerPageEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "BannerId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int BannerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PageId { get; set; }
    }
}
