using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Message
{
    public class NotificationTargetInfoDetail : NotificationTargetDefaultDetail
    {
    }
    public class NotificationTargetDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationTargetId")]
        public int NotificationTargetId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationTypeId")]
        public int NotificationTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationTargetTypeId")]
        public NotificationTargetType NotificationTargetTypeId { get; set; }

    }
    public class NotificationTargetDefaultInfoDetail : NotificationTargetDefaultDetail
    {
        public MailServerProviderDetail MailServerProvider { get; set; }
    }
    public class NotificationTargetDefaultDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationTargetDefaultId")]
        public int NotificationTargetDefaultId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MailServerProviderId")]
        public int? MailServerProviderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TargetNo")]
        public string TargetNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TargetName")]
        public string TargetName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContactName")]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PhoneNo")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MailAddress")]
        public string MailAddress { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSelected")]
        public bool IsSelected { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }
    public class NotificationTargetDefaultEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MailServerProviderId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int? MailServerProviderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TargetNo")]
        public string TargetNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TargetName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TargetName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContactName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(30, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        public string Address { get; set; }

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


        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsActive { get; set; }
    }
    public class NotificationTargetDefaultEditEntry : NotificationTargetDefaultEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationTargetDefaultId")]
        public int NotificationTargetDefaultId { get; set; }
    }
}
