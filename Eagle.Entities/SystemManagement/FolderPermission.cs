using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("FolderPermission")]
    public class FolderPermission: EntityBase, IEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FolderId { get; set; }
        public int PermissionId { get; set; }
        public Guid RoldeId { get; set; }
        public Guid UserId { get; set; }
        public bool AllowAccess { get; set; }
    }
}
