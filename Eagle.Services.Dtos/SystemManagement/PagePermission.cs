using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.Identity;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class PageRolePermissionEntry : DtoBase
    {
        public IEnumerable<PagePermissionEntry> PagePermissions { get; set; }
        public string UserIds { get; set; }
    }
    public class PagePermissionEntry : RolePermissionEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PageId { get; set; }
    }
    public class PagePermissionDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RoleId")]
        public Guid RoleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        public int PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AllowAccess")]
        public bool AllowAccess { get; set; }
    }
}
