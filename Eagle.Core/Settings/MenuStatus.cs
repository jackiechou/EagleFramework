using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum MenuStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Active", Description = "Active", Order = 0)]
        Active = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive", Description = "InActive", Order = 1)]
        InActive = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Published", Description = "Published", Order = 2)]
        Published = 2
    }
}
