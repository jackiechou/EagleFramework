using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum AddressType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Emergency", Description = "Emergency", Order = 0)]
        Emergency = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Permanent", Description = "Permanent", Order = 1)]
        Permanent = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Temporary", Description = "Temporary", Order = 2)]
        Temporary = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Company", Description = "Company", Order = 3)]
        Company = 4,
        [Display(ResourceType = typeof(LanguageResource), Name = "Customer", Description = "Customer", Order = 4)]
        Customer = 5,
        [Display(ResourceType = typeof(LanguageResource), Name = "Member", Description = "Member", Order = 5)]
        Member = 6,
        [Display(ResourceType = typeof(LanguageResource), Name = "Vendor", Description = "Vendor", Order = 6)]
        Vendor = 7,
        
    }
}