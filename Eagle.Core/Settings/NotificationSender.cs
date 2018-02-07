using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum NotificationSenderType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "System", Description = "System", Order = 1)]
        System = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "User", Description = "User", Order = 2)]
        User = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Customer", Description = "Customer", Order = 3)]
        Customer = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Employee", Description = "Employee", Order = 4)]
        Employee = 4,
    }
}
