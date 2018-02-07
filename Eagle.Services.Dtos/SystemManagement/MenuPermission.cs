using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.Identity;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class MenuPermissionAccessLevel : MenuPermissionLevelDetail
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool AllowAccess { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int MenuId { get; set; }

        public MenuPermissionLevelDetail MenuPermissionLevel { get; set; }
    }

    public class MenuPermissionDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MenuPermissionId")]
        public int MenuPermissionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RoleId")]
        public Guid RoleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuId")]
        public int MenuId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PermissionId")]
        public int PermissionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserIds")]
        public string UserIds { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AllowAccess")]
        public bool? AllowAccess { get; set; }

        public MenuDetail Menu { get; set; }
        public RoleDetail Role { get; set; }
    }
    public class MenuPermissionEntry : RolePermissionEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MenuId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int MenuId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PermissionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PermissionId { get; set; }
    }
    public class MenuPermissionEditEntry : MenuPermissionEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MenuPermissionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int MenuPermissionId { get; set; }
    }

    public class MenuPermissionLevelDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PermissionId")]
        public int PermissionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PermissionName")]
        public string PermissionName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DisplayOrder")]
        public int DisplayOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public MenuPermissionLevelStatus? IsActive { get; set; }
    }

    public class MenuPermissionLevelEditEntry : MenuPermissionLevelEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PermissionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PermissionId { get; set; }
    }

    public class MenuPermissionLevelEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PermissionName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string PermissionName { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }
    }

    public class MenuRolePermissionDetail : DtoBase
    {
        public Guid RoleId { get; set; }
        public List<MenuPermissionAccessLevel> MenuPermissionAccessLevels { get; set; }
        public RoleDetail Role { get; set; }
    }

    public class MenuRolePermissionEntry : DtoBase
    {
        public MenuDetail Menu { get; set; }
        public List<MenuPermissionLevelDetail> MenuPermissionLevels { get; set; }
        public List<MenuRolePermissionDetail> MenuRolePermissions { get; set; }
    }
}
