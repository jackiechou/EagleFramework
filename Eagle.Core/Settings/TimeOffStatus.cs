using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum TimeOffStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Pending", Description = "Pending", Order = 0)]
        Pending = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Approved", Description = "Approved", Order = 1)]
        Approved = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Cancel", Description = "Cancel", Order = 2)]
        Cancel = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Rejected", Description = "Rejected", Order = 3)]
        Rejected=3
    }
}
