using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum BookingPeriodGroup
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Anytime", Description = "Anytime", Order = 0)]
        Anytime = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "MorningPeriod", Description = "MorningPeriod", Order = 1)]
        Morning = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "AfternoonPeriod", Description = "AfternoonPeriod", Order = 2)]
        Afternoon = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "EveningPeriod", Description = "EveningPeriod", Order = 3)]
        Evening = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "SpecificTime", Description = "SpecificTime", Order = 4)]
        SpecificTime = 4
    }
}
