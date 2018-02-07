using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Banners
{
    public class BannerZoneInfoDetail : BannerZoneDetail
    {
        public BannerPositionDetail Position { get; set; }
    }

    public class BannerZoneDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "BannerId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int BannerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PositionId { get; set; }
    }

    public class BannerZoneEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "BannerId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int BannerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PositionId { get; set; }
    }
}
