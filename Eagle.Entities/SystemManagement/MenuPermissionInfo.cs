using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class MenuPermissionInfo : EntityBase
    {
        public Guid RoleId { get; set; }
        public int MenuId { get; set; }
        public int PermissionId { get; set; }

        public string UserIds { get; set; }
        public bool? AllowAccess { get; set; }

        public virtual RoleInfo Role { get; set; }
        public virtual MenuPermissionLevelInfo MenuPermissionLevel { get; set; }
    }
}
