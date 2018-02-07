using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.Identity;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class ModulePermissionEntry : DtoBase
    {
        public Guid RoleId { get; set; }
        public int ModuleId { get; set; }
        public int CapabilityId { get; set; }
        public string CapabilityCode { get; set; }
        public string UserIds { get; set; }
        public bool AllowAccess { get; set; }
    }
    public class ModulePermissionDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ModulePermissionId")]
        public int ModulePermissionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RoleId")]
        public Guid RoleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleId")]
        public int ModuleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CapabilityId")]
        public int CapabilityId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserIds")]
        public string UserIds { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AllowAccess")]
        public bool AllowAccess { get; set; }
    }


    public class ModuleRolePermissionEntry : DtoBase
    {
        public ModuleDetail Module { get; set; }
        public List<ModuleCapabilityDetail> ModuleCapabilities { get; set; }
        public List<ModuleRolePermissionDetail> ModuleRolePermissions { get; set; }
    }
    public class ModuleRolePermissionDetail : DtoBase
    {
        public Guid RoleId { get; set; }
        public List<ModuleCapabilityAccess> ModuleCapabilities { get; set; }
        public RoleDetail Role { get; set; }
    }
}
