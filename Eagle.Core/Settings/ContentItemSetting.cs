using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum ContentItemSetting
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Module", Description = "Module", Order = 0)]
        Module = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Page", Description = "Page", Order = 1)]
        Page = 2,
    }
}
