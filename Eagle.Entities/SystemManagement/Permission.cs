using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("Permission")]
    public class Permission : EntityBase
    {
        public Permission()
        {
            IsActive = true;
        }
        [Key, Column("PermissionId"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public int DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
    }
}
