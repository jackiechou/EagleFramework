using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
   public enum CustomerTypeSetting
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Normal", Description = "Normal", Order = 0)]
        Normal = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Discount", Description = "Discount", Order = 1)]
        Discount = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Vip", Description = "Vip", Order = 1)]
        Vip = 2,
    }
}
