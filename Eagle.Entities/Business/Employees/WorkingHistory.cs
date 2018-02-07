using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Employees
{
    [Table("WorkingHistory", Schema = "Personnel")]
    public class WorkingHistory : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkingHistoryId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? JoinedDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? ManagerId { get; set; }
        public int PositionId { get; set; }
        public string Duty { get; set; }
        public string Note { get; set; }

        public Employee Employee { get; set; }
        public Employee Manager { get; set; }
        public JobPosition Position { get; set; }
    }
}
