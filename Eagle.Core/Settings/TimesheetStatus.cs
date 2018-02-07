using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum TimesheetStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "New", Description = "New", Order = 0)]
        New = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Confirmed", Description = "Confirmed", Order = 1)]
        Confirmed = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Processed", Description = "Processed", Order = 2)]
        Processed = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Cancelled", Description = "Cancelled", Order = 3)]
        Cancelled = 3,
    }
}
