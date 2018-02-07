using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum PromotionType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Coupon", Description = "Coupon", Order = 0)]
        Coupon = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Voucher", Description = "Voucher", Order = 1)]
        Voucher = 2,
    }
}
