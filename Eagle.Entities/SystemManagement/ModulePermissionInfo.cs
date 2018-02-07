using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class ModulePermissionInfo: EntityBase
    {
        public int ModulePermissionId { get; set; }
        public Guid RoleId { get; set; }
        public int ModuleId { get; set; }
        public int CapabilityId { get; set; }
        public string CapabilityCode { get; set; }
        public string UserIds { get; set; }
        public bool AllowAccess { get; set; }

        public virtual RoleInfo Role { get; set; }
        public virtual ModuleCapabilityInfo ModuleCapability { get; set; }
    }
}
