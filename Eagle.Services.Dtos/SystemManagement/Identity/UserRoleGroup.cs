using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.Identity
{
    public class UserRoleGroupDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "UserId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid UserId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RoleGroupId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int RoleGroupId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsDefault")]
        public bool? IsDefault { get; set; }

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
    }
    public class UserRoleGroupCreate : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RoleGroupId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int RoleGroupId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsDefault")]
        public bool? IsDefault { get; set; }

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

        public RoleGroupDetail RoleGroup { get; set; }
    }
    public class UserRoleGroupEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "UserId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid UserId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RoleGroupId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int RoleGroupId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsDefault")]
        public bool? IsDefault { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EffectiveDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? EffectiveDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ExpiryDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? ExpiryDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsAllowed")]
        public bool IsAllowed { get; set; }

        [NotMapped]
        public RoleGroupDetail RoleGroup { get; set; }

        [NotMapped]
        public RoleDetail Role { get; set; }

        [NotMapped]
        public GroupDetail Group { get; set; }
    }
    public class UserRoleGroupEdit : UserRoleGroupEntry
    {

    }

    public class UserRoleGroupInfoDetail : UserRoleGroupDetail
    {
        public UserDetail User { get; set; }
        public RoleDetail Role { get; set; }
        public GroupDetail Group { get; set; }
    }
}
