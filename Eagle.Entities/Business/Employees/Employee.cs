using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Employees
{
    [Table("Employee", Schema = "Personnel")]
    public class Employee : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public int ContactId { get; set; }
        public int? EmergencyAddressId { get; set; }
        public int? PermanentAddressId { get; set; }
        public int? VendorId { get; set; }
        public int CompanyId { get; set; }
        public int PositionId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime? JoinedDate { get; set; }
        public EmployeeStatus Status { get; set; }
    }
}
