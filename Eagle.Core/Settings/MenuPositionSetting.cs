using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum MenuPositionSetting
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Top", Description = "Top", Order = 0)]
        Top = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Bottom", Description = "Bottom", Order = 1)]
        Bottom = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Left", Description = "Left", Order = 2)]
        Left = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Right", Description = "Right", Order = 3)]
        Right = 4,
        [Display(ResourceType = typeof(LanguageResource), Name = "Middle", Description = "Middle", Order = 4)]
        Middle = 5,
        [Display(ResourceType = typeof(LanguageResource), Name = "Top", Description = "Top", Order = 5)]
        TopSite = 6,
        [Display(ResourceType = typeof(LanguageResource), Name = "Bottom", Description = "Bottom", Order = 6)]
        BottomSite = 7,
        [Display(ResourceType = typeof(LanguageResource), Name = "Left", Description = "Left", Order = 7)]
        LeftSite = 8,
        [Display(ResourceType = typeof(LanguageResource), Name = "Right", Description = "Right", Order = 8)]
        RightSite = 9,
        [Display(ResourceType = typeof(LanguageResource), Name = "Middle", Description = "Middle", Order = 9)]
        MiddleSite = 10,
    }
}
