using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business.Personnel
{
    public class SalaryDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SalaryId")]
        public int SalaryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SignedDate")]
        public DateTime? SignedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EffectiveDate")]
        public DateTime? EffectiveDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BasicSalary")]
        public decimal? BasicSalary { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ActualSalary")]
        public decimal? ActualSalary { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "GrossSalary")]
        public decimal? GrossSalary { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InsuranceFee")]
        public decimal? InsuranceFee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }
    }
    public class SalaryEntry : DtoBase
    {
        public int EmployeeId { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? ActualSalary { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? InsuranceFee { get; set; }
        public string CurrencyCode { get; set; }
    }
    public class SalaryEditEntry : SalaryEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SalaryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int SalaryId { get; set; }
    }
}
