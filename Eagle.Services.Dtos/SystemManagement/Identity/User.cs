using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.Identity
{
    public class UserViewModel
    {
        [Key]
        public Guid UserId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmpId")]
        public int? EmpId { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Password")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [StringLength(6, MinimumLength = 3)]
        public string Password { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RetypePassword")]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [StringLength(6, MinimumLength = 3)]
        [Compare("Password", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ValidatePassword")]
        public string RetypePassword { get; set; }

        public string InitialUserName { get { return UserName; } }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        // [Remote("ValidationUserName", "Validation", AdditionalFields = "InitialUserName")]        
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [Editable(true)]
        public string UserName { get; set; }
        public string FullName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsLDAP")]
        public int IsLDAP { get; set; }

        public bool? IsSuperUser { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FAdm")]
        public int? FAdm { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FLock")]
        public int? FLock { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "JoinDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? JoinDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FromDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ToDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LockDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? LockDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmpName")]
        public string strEmpName { get { return LastName + " " + FirstName; } }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Creater { get; set; }
        public string Editer { get; set; }
        public string strIsLDAP { get; set; }
        public string strFAdm { get; set; }
        public string strFLock { get; set; }
        public string strFromDate { get; set; }
        public string strToDate { get; set; }
        public string strLockDate { get; set; }
        public string strJoinDate { get; set; }

        //   public EmployeeViewModel EmployeeInfo { get; set; }
    }
    public class UserReturn
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool ChangePassword { get; set; }
        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
    }

    public class UserSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RoleId")]
        public Guid? RoleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "GroupId")]
        public Guid? GroupId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "KeyName")]
        public string KeyName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsApproved")]
        public bool? IsApproved { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsLockedOut")]
        public bool? IsLockedOut { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSuperUser")]
        public bool? IsSuperUser { get; set; }
    }
    public class UserInfoDetail : DtoBase
    {
        public ApplicationDetail Application { get; set; }
        public UserDetail User { get; set; }
        public UserProfileInfoDetail Profile { get; set; }
        public IEnumerable<UserRoleInfoDetail> Roles { get; set; }
    }
    public class UserDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserId")]
        public Guid UserId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SeqNo")]
        public int SeqNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LoweredUserName")]
        public string LoweredUserName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordSalt")]
        public string PasswordSalt { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordQuestion")]
        public string PasswordQuestion { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordAnswer")]
        public string PasswordAnswer { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordQuestion")]
        public bool? UpdatePassword { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmailConfirmed")]
        public bool? EmailConfirmed { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastPasswordChangedDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? LastPasswordChangedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FailedPasswordAttemptCount")]
        public int? FailedPasswordAttemptCount { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FailedPasswordAttemptTime")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? FailedPasswordAttemptTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FailedPasswordAnswerAttemptCount")]
        public int? FailedPasswordAnswerAttemptCount { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FailedPasswordAnswerAttemptTime")]
        public DateTime? FailedPasswordAnswerAttemptTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ExpiredDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ExpiredDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastLoginDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? LastLoginDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsApproved")]
        public bool? IsApproved { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsLockedOut")]
        public bool? IsLockedOut { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSuperUser")]
        public bool? IsSuperUser { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastLockoutDate")]
        public DateTime? LastLockoutDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastActivityDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? LastActivityDate { get; set; }
    }
    public class UserEntry : DtoBase
    {
        public UserEntry()
        {
            Contact = new ContactEntry();
            EmergencyAddress = new EmergencyAddressEntry();
            PermanentAddress = new PermanentAddressEntry();
            IsSuperUser = false;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        // [Remote("ValidationUserName", "Validation", AdditionalFields = "InitialUserName")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        //[MaxLength(256, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength"), MinLength(2)]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 2)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Password")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [StringLength(128, MinimumLength = 3)]
        public string PasswordSalt { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ConfirmedPassword")]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [StringLength(128, MinimumLength = 3)]
        [Compare("PasswordSalt", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ValidatePassword")]
        public string ConfirmedPassword { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordQuestion")]
        public string PasswordQuestion { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordAnswer")]
        [StringLength(128, MinimumLength = 2)]
        public string PasswordAnswer { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FromDate")]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? ExpiredDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSuperUser")]
        public bool? IsSuperUser { get; set; }

        public ContactEntry Contact { get; set; }
        public EmergencyAddressEntry EmergencyAddress { get; set; }
        public PermanentAddressEntry PermanentAddress { get; set; }

        public List<UserRoleCreate> UserRoles { get; set; }
        public List<UserRoleGroupCreate> UserRoleGroups { get; set; }
    }
    public class UserEditEntry : DtoBase
    {
        public UserEditEntry()
        {
            Contact = new ContactEditEntry();
            EmergencyAddress = new EmergencyAddressEditEntry();
            PermanentAddress = new PermanentAddressEditEntry();
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserId", Description = "UserId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid UserId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserName", Description = "UserName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        // [Remote("ValidationUserName", "Validation", AdditionalFields = "InitialUserName")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 2)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Password", Description = "Password")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [StringLength(128, MinimumLength = 3)]
        public string PasswordSalt { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ConfirmedPassword", Description = "ConfirmedPassword")]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [StringLength(128, MinimumLength = 3)]
        [System.ComponentModel.DataAnnotations.Compare("PasswordSalt", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ValidatePassword")]
        public string ConfirmedPassword { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordQuestion", Description = "PasswordQuestion")]
        public string PasswordQuestion { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordAnswer", Description = "PasswordAnswer")]
        [StringLength(128, MinimumLength = 2)]
        public string PasswordAnswer { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FromDate", Description = "FromDate")]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate", Description = "EndDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? ExpiredDate { get; set; }
        public bool? IsSuperUser { get; set; }

        public ContactEditEntry Contact { get; set; }
        public EmergencyAddressEditEntry EmergencyAddress { get; set; }
        public PermanentAddressEditEntry PermanentAddress { get; set; }

        public List<UserRoleEdit> UserRoles { get; set; }
        //public List<UserRoleGroupEdit> UserRoleGroups { get; set; }
    }
}
