using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Message
{
    public class NotificationSenderEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MailServerProviderId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int MailServerProviderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SenderName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string SenderName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContactName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(30, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MailAddress")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string MailAddress { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Password")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [StringLength(200, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Password { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ConfirmedPassword")]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [StringLength(200, MinimumLength = 3)]
        [Compare("Password", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ValidatePassword")]
        public string ConfirmedPassword { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Signature")]
        public string Signature { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }
    public class NotificationSenderEditEntry : NotificationSenderEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationSenderId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int NotificationSenderId { get; set; }
    }
    public class NotificationSenderDetail : DtoBase
    {
        [Key]
        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationSenderId")]
        public int NotificationSenderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MailServerProviderId")]
        public int MailServerProviderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SenderNo")]
        public string SenderNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SenderName")]
        public string SenderName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContactName")]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MailAddress")]
        public string MailAddress { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Signature")]
        public string Signature { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSelected")]
        public bool IsSelected { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }

    public class NotificationSenderInfoDetail : NotificationSenderDetail
    {
        public MailServerProviderDetail MailServerProvider { get; set; }
    }
}
