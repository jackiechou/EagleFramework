using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum EventStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Upcoming", Description = "Upcoming", Order = 0)]
        Upcoming=1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Completed", Description = "Completed", Order = 1)]
        Completed=2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Cancelled", Description = "Cancelled", Order = 2)]
        Cancelled=3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Deleted", Description = "Deleted", Order = 3)]
        Deleted=4
    }
}
