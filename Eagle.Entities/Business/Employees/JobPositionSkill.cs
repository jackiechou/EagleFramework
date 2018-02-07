using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Employees
{
    [Table("JobPositionSkill", Schema = "Personnel")]
    public class JobPositionSkill : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobPositionSkillId { get; set; }
        public int PositionId { get; set; }
        public int SkillId { get; set; }
    }
}
