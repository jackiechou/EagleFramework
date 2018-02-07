using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class CapabilityEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CapabilityName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CapabilityName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CapabilityCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CapabilityCode { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }
    public class ModuleCapabilityInfoDetail : ModuleCapabilityDetail
    {
        public Guid RoleId { get; set; }
        public ModuleDetail Module { get; set; }
    }
    public class ModuleCapabilityAccess : DtoBase
    {
        public int CapabilityId { get; set; }
        public string CapabilityCode { get; set; }
        public int ModuleId { get; set; }
        public bool AllowAccess { get; set; }
        public ModuleCapabilityInfoDetail ModuleCapability { get; set; }
    }

    public class ModuleCapabilityDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CapabilityId")]
        public int CapabilityId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CapabilityName")]
        public string CapabilityName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CapabilityCode")]
        public string CapabilityCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DisplayOrder")]
        public int DisplayOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ModuleCapabilityStatus? IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleId")]
        public int ModuleId { get; set; }
    }
    public class ModuleCapabilityEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ModuleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CapabilityName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CapabilityName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CapabilityCode")]
        public string CapabilityCode { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }
    public class ModuleCapabilityEditEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CapabilityId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CapabilityId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CapabilityName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CapabilityName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CapabilityCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CapabilityCode { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ModuleId { get; set; }
    }
}
