using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Roster
{
    [Table("ShiftPosition", Schema = "Roster")]
    public class ShiftPosition : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShiftPositionId { get; set; }
        public int ShiftId { get; set; }
        public int? PositionId { get; set; }
    }
}
