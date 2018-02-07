using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Employees
{
    [Table("EmployeePosition", Schema = "Personnel")]
    public class EmployeePosition : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeePositionId { get; set; }
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
    }
}
