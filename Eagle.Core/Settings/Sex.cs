using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum Sex
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "NoneSpecified", Description = "NoneSpecified", Order = 0)]
        NoneSpecified = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Male", Description = "Male", Order = 1)]
        Male = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Female", Description = "Female", Order = 2)]
        Female = 2,
       
    }
}
