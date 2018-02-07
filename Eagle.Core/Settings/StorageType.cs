using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum StorageType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Local", Description = "Local", Order = 1)]
        Local = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "YouTube", Description = "YouTube", Order = 1)]
        YouTube = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Vimeo", Description = "Vimeo", Order = 1)]
        Vimeo = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Wistia", Description = "Wistia", Order = 1)]
        Wistia = 4,
        [Display(ResourceType = typeof(LanguageResource), Name = "SoundCloud", Description = "SoundCloud", Order = 1)]
        SoundCloud = 5,
    }
}
