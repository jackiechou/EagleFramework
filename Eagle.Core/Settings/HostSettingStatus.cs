using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum HostSettingStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Secure", Description = "Secure", Order = 0)]
        Secure = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "InSecure", Description = "InSecure", Order = 1)]
        InSecure = 1
    }
}
