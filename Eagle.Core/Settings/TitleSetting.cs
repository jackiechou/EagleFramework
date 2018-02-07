using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum TitleSetting
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Mr", Description = "Mr", Order = 1)]
        Mr = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Ms", Description = "Ms", Order = 2)]
        Ms = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Mrs", Description = "Mrs", Order = 0)]
        Mrs = 3,
    }
}
