using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum SexType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Male", Description = "Male", Order = 1)]
        Male = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Female", Description = "Female", Order = 2)]
        Female = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Unspecified", Description = "Unspecified", Order = 3)]
        Unspecified = 3,
    }
}
