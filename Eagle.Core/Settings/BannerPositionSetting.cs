using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
   public enum BannerPositionSetting
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Top", Description = "Top", Order = 0)]
        Top = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Middle", Description = "Middle", Order = 1)]
        Middle = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Bottom", Description = "Bottom", Order = 2)]
        Bottom = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Left", Description = "Left", Order = 3)]
        Left = 4,
        [Display(ResourceType = typeof(LanguageResource), Name = "Right", Description = "Right", Order = 4)]
        Right = 5,
    }
}
