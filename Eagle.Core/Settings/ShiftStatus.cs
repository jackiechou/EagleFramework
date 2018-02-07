using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum ShiftStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Draft", Description = "Draft", Order = 1)]
        Draft = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Published", Description = "Published", Order = 2)]
        Published = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Confirmed", Description = "Confirmed", Order = 2)]
        Confirmed = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Rejected", Description = "Rejected", Order = 3)]
        Rejected = 3,
    }
}
