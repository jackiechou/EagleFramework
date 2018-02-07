using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
     public enum MessageTypeSetting
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Email", Description = "Email", Order = 0)]
        Email = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Sms", Description = "Sms", Order = 1)]
        Sms = 2
    }
}
