using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.Identity
{
    public class AuthenticatedInfo : DtoBase
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string TrustedToken { get; set; }
    }
    public class AuthenticationResult : DtoBase
    {
        public bool Successed { get; set; }
        public string Error { get; set; }
    }

    public class VerifyCodeViewModel : DtoBase
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
  
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
    public class RecoverPasswordModel
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "UserName")]
        public string UserName { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "OldPassword")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string OldPassword { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "NewPassword")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string NewPassword { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "ConfirmPassword")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ConfirmPassword { get; set; }
    }
    public class ResetPassword
    {
        [Required]
        [EmailAddress(ErrorMessage = "Your email looks incorrect. Please check and try again.")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        public bool SendEmailSuccessed { get; set; }
    }
    public class Register
    {
        [Required]
        [Display(Name = "User name")]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Your email looks incorrect. Please check and try again.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Your password must be at least {0} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Your password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class Signin : DtoBase
    {
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [MaxLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [Display(ResourceType = typeof(LanguageResource), Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [MaxLength(100, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [Display(ResourceType = typeof(LanguageResource), Name = "Password")]
        public string Password { get; set; }
    }
    public class LoginModel : DtoBase
    {
        public LoginModel() { }
        public LoginModel(string desireUrl)
        {
            DesiredUrl = desireUrl;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RememberMe")]
        public bool RememberMe { get; set; }

        public string DesiredUrl { get; set; }
    }
    public class ChangePassword
    {
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,100})", ErrorMessage = "Password must contain: minimum 6 characters, upper case letter and numberic value")]
        public string NewPassword { get; set; }

        public bool ChangePasswordSuccess { get; set; }

        public string UserName { get; set; }

        public string TokenConfirm { get; set; }
    }
    public class ChangePasswordModel : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "UserName")]
        public string UserName { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "OldPassword")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string OldPassword { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "NewPassword")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string NewPassword { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "RetypeNewPassword")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ConfirmedPassword { get; set; }
    }

    public class ChangeProfile
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Your email looks incorrect. Please check and try again.")]
        [MaxLength(256)]
        public string Email { get; set; }
    }

    public class ConfirmEmail
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
