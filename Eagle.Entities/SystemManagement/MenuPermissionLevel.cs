using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement
{
    [Table("MenuPermissionLevel")]
    public class MenuPermissionLevel: EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public MenuPermissionLevelStatus? IsActive { get; set; }
    }
}
