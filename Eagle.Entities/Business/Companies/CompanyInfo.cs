using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.SystemManagement;

namespace Eagle.Entities.Business.Companies
{
    [NotMapped]
    public class CompanyInfo : Company
    {
        public virtual Address Address { get; set; }
    }
}
