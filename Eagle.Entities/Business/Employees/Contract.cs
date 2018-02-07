using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Employees
{
    [Table("Contract", Schema = "Personnel")]
    public class Contract : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContractId { get; set; }
        public string ContractNo { get; set; }
        public ContractType ContractTypeId { get; set; }
        public int CompanyId { get; set; }
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal? ProbationSalary { get; set; }
        public decimal? InsuranceSalary { get; set; }
        public DateTime? ProbationFrom { get; set; }
        public DateTime? ProbationTo { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
