using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum UserType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Administrator", Description = "Administrator", Order = 0)]
        Administrator =1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Moderator", Description = "Moderator", Order = 1)]
        Moderator = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Subscriber", Description = "Subscriber", Order = 2)]
        Subscriber = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Member", Description = "Member", Order = 3)]
        Member = 4,
        [Display(ResourceType = typeof(LanguageResource), Name = "Customer", Description = "Customer", Order = 4)]
        Customer = 5,
    }
}
