using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.Business.Companies;
using Eagle.Entities.SystemManagement;

namespace Eagle.Entities.Business.Employees
{
    [NotMapped]
    public class ContractInfo : Contract
    {
        public virtual Employee Employee { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Company Company { get; set; }
        public virtual JobPosition JobPosition { get; set; }
    }
}
