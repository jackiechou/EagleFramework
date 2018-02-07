using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class PaymentMethodEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PaymentMethodName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PaymentMethodName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsCreditCard")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsCreditCard { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSelected")]
        public bool IsSelected { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public PaymentMethodStatus IsActive { get; set; }
    }
    public class PaymentMethodEditEntry : PaymentMethodEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PaymentMethodId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PaymentMethodId { get; set; }
    }
    public class PaymentMethodDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PaymentMethodId")]
        public int PaymentMethodId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PaymentMethodName")]
        public string PaymentMethodName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsCreditCard")]
        public bool IsCreditCard { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSelected")]
        public bool IsSelected { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public PaymentMethodStatus IsActive { get; set; }
    }

    public class PaymentMethodSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PaymentMethodName")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PaymentMethodName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public PaymentMethodStatus? IsActive { get; set; }
    }
}
