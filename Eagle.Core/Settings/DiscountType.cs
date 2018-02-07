using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum DiscountType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Normal", Description = "Normal", Order = 0)]
        Normal = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "VIP", Description = "VIP", Order = 1)]
        Vip = 2,
    }
}
