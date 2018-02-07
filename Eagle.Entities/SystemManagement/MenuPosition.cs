using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("MenuPosition")]
    public class MenuPosition : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public bool IsActive { get; set; }

        public int TypeId { get; set; }
    }
}
