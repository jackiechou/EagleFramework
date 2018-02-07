using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Employees
{
    [Table("Salary", Schema = "Personnel")]
    public class Salary : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalaryId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? ActualSalary { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? InsuranceFee { get; set; }
        public string CurrencyCode { get; set; }
    }
}
