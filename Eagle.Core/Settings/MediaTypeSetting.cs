using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum MediaTypeSetting
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Video", Description = "Video", Order = 0)]
        Video = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Audio", Description = "Audio", Order = 1)]
        Audio = 2,
    }
}
