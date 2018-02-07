using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.Business.Companies;
using Eagle.Entities.SystemManagement;

namespace Eagle.Entities.Business.Employees
{
    [NotMapped]
    public class EmployeeInfo: Employee
    {
        public virtual Contact Contact { get; set; }
        public virtual Address EmergencyAddress { get; set; }
        public virtual Address PermanentAddress { get; set; }
        public virtual Company Company { get; set; }
        public virtual JobPosition JobPosition { get; set; }
    }
}
