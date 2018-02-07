using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Employees;

namespace Eagle.Entities.Business.Roster
{
    [Table("Shift", Schema = "Roster")]
    public class Shift : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShiftId { get; set; }
        public string Location { get; set; }
        public string SubLocation { get; set; }
        public string Comments { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? BreakLength { get; set; }
        public DateTime? BreakStartTime { get; set; }
        public ShiftStatus Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? ShiftPositionId { get; set; }
        public int? EmployeeId { get; set; }
        public virtual ShiftPosition ShiftPosition { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
