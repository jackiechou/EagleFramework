using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("ModuleGroup")]
    public class ModuleGroup : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
    }
}
