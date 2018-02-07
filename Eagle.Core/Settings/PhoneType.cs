using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum PhoneType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile", Description = "Mobile", Order = 0)]
        Mobile = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Home", Description = "Home", Order = 1)]
        Home = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Work", Description = "Work", Order = 2)]
        Work = 3
    }
}
