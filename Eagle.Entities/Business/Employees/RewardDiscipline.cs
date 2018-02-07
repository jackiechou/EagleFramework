using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Employees
{
    [Table("RewardDiscipline", Schema = "Personnel")]
    public class RewardDiscipline : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RewardDisciplineId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsReward { get; set; }
        public string Reason { get; set; }
    }
}
