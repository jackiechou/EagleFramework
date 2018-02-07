using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.Identity
{
    public class UserRoleInfoDetail : DtoBase
    {
        public int UserRoleId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsTrialUsed { get; set; }
        public bool? IsDefaultRole { get; set; }

        public UserDetail User { get; set; }
        public RoleDetail Role { get; set; }
    }
    public class UserRoleDetail : DtoBase
    {
        public int UserRoleId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsTrialUsed { get; set; }
        public bool? IsDefaultRole { get; set; }

        public UserDetail User { get; set; }
        public RoleDetail Role { get; set; }
    }
    public class UserRoleCreate : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RoleId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid RoleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsDefaultRole")]
        public bool? IsDefaultRole { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsTrialUsed")]
        public bool? IsTrialUsed { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EffectiveDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? EffectiveDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ExpiryDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? ExpiryDate { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "IsAllowed")]
        public bool? IsAllowed { get; set; }
        public RoleDetail Role { get; set; }

    }
    public class UserRoleEdit : UserRoleCreate
    {
        public UserRoleEdit()
        {
            UserRoleGroups = new List<UserRoleGroupEdit>();
        }
        [Display(ResourceType = typeof(LanguageResource), Name = "UserId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid UserId { get; set; }

        public List<UserRoleGroupEdit> UserRoleGroups { get; set; }
    }
    
    public class UserRoleEntry : DtoBase
    {
        public UserRoleEntry()
        {
            IsDefaultRole = false;
            IsTrialUsed = false;
            EffectiveDate = DateTime.UtcNow;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid UserId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RoleId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid RoleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsDefaultRole")]
        public bool? IsDefaultRole { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsTrialUsed")]
        public bool? IsTrialUsed { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EffectiveDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? EffectiveDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ExpiryDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? ExpiryDate { get; set; }

        public RoleDetail Role { get; set; }
    }
}
