using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.Identity
{
    public class RoleUserEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RoleId")]
        public Guid RoleId { get; set; }
        public List<Guid> UserIds { get; set; }
    }
    public class RoleSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "GroupId")]
        public Guid? GroupId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RoleName")]
        public string RoleName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RoleName")]
        public bool? Status { get; set; }
    }
    public class RoleInfoDetail : RoleDetail
    {
        public IEnumerable<RoleGroupDetail> Groups { get; set; }
        public IEnumerable<UserDetail> Users { get; set; }
    }
    public class RoleDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RoleId")]
        public Guid RoleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RoleCode")]
        public string RoleCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RoleName")]
        public string RoleName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LoweredRoleName")]
        public string LoweredRoleName => RoleName.ToLower();

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }
    }
    public class RoleEditEntry : RoleEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RoleId")]
        public Guid RoleId { get; set; }
    }
    public class RoleEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RoleName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(256, MinimumLength = 2, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinMaxTitleLength")]
        public string RoleName { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "RoleCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(256, MinimumLength = 2, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinMaxTitleLength")]
        public string RoleCode { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(256, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(LanguageResource), Name = "SelectedGroups")]
        public List<Guid> SelectedGroups { get; set; }
    }
    public class RolePermissionEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RoleId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid RoleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AllowAccess")]
        public bool AllowAccess { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserIds")]
        public string UserIds { get; set; }

        public RoleDetail Role { get; set; }
    }
}
