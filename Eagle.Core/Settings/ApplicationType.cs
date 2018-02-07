using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum ApplicationType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Web", Description = "Web", Order = 0)]
        Web =1,
       [Display(ResourceType = typeof(LanguageResource), Name = "Mobile", Description = "Mobile", Order = 1)]
        Mobile =2
    }
}
