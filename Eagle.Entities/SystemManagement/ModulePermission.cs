using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("ModulePermission")]
    public class ModulePermission : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModulePermissionId { get; set; }
        public Guid RoleId { get; set; }
        public int ModuleId { get; set; }
        public int CapabilityId { get; set; }
        public string CapabilityCode { get; set; }
        public string UserIds { get; set; }
        public bool AllowAccess { get; set; }
    }
}