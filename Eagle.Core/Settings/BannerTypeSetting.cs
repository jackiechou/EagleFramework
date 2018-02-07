using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum BannerTypeSetting
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Image", Description = "Image", Order = 0)]
        Image = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Flash", Description = "Flash", Order = 1)]
        Flash = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Text", Description = "Text", Order = 2)]
        Text = 3,
    }
}
