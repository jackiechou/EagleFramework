using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Employees
{
    [Table("JobPosition", Schema= "Personnel")]
    public class JobPosition : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
