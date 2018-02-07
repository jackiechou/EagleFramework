using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum NotificationTargetType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "System", Description = "System", Order = 1)]
        System = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "User", Description = "User", Order = 2)]
        User = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "AllCustomers", Description = "AllCustomers", Order = 3)]
        AllCustomers = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "AllEmployees", Description = "AllEmployees", Order = 4)]
        AllEmployees = 4,
        [Display(ResourceType = typeof(LanguageResource), Name = "Customer", Description = "Customer", Order = 5)]
        Customer = 5,
        [Display(ResourceType = typeof(LanguageResource), Name = "Employee", Description = "Employee", Order = 6)]
        Employee = 6,
    }
}
