using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("MenuPermission")]
    public class MenuPermission : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuPermissionId { get; set; }
        public Guid RoleId { get; set; }
        public int MenuId { get; set; }
        public int PermissionId { get; set; }

        public string UserIds { get; set; }
        public bool? AllowAccess { get; set; }
    }
}
