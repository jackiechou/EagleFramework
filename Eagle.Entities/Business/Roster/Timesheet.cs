using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Employees;

namespace Eagle.Entities.Business.Roster
{
    [Table("Timesheet", Schema = "Roster")]
    public class Timesheet : EntityBase
    {
        public Timesheet()
        {
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimesheetId { get; set; }
        public int EmployeeId { get; set; }
        public int? ShiftId { get; set; }
        public int ShiftTypeId { get; set; }
        public int? EmployeeTimeOffId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? BreakStartTime { get; set; }
        public int? BreakLength { get; set; }
        public string Location { get; set; }
        public string SubLocation { get; set; }
        public string Comments { get; set; }
        public TimesheetStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual EmployeeTimeOff EmployeeTimeOff { get; set; }
        public virtual Shift Shift { get; set; }
        public virtual ShiftType ShiftType { get; set; }
    }
}
