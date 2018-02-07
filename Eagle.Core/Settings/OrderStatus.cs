using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum OrderStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Pending", Description = "Pending", Order = 0)]
        Pending = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Approved", Description = "Approved", Order = 1)]
        Approved = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Cancelled", Description = "Cancelled", Order = 2)]
        Cancelled = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Rejected", Description = "Rejected", Order = 3)]
        Rejected = 3,
    }
    public enum OrderState
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "None", Description = "None")]
        None = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderEmptyCart", Description = "OrderEmptyCart")]
        EmptyCart = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderPromoCodeUtilized", Description = "OrderPromoCodeUtilized")]
        PromoCodeUtilized = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "SessionTimeOut", Description = "SessionTimeOut")]
        SessionTimeOut = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderCannotSaveOrder", Description = "OrderCannotSaveOrder")]
        CannotSaveOrder = 4,
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderProcessPaymentError", Description = "OrderProcessPaymentError")]
        ProcessPaymentError = 5,
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderQuantityNotSufficient", Description = "OrderQuantityNotSufficient")]
        QuantityNotSufficient = 6,
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderPaymentFail", Description = "OrderQuantityNotSufficient")]
        PaymentFail = 7
    }
}
