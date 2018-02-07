using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Roster;

namespace Eagle.Entities.Business.Employees
{
    [Table("EmployeeTimeOff", Schema = "Personnel")]
    public class EmployeeTimeOff : EntityBase
    {
        public EmployeeTimeOff()
        {
            CreatedDate = DateTime.UtcNow;
            Status = TimeOffStatus.Pending;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeTimeOffId { get; set; }
        public string TimeZoneId { get; set; }
        public int TimeOffTypeId { get; set; }
        public TimeOffStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }


        public virtual Employee Employee { get; set; }
        public virtual TimeOffType TimeOffType { get; set; }
    }
}
