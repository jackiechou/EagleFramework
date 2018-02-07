using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.Identity;

namespace Eagle.Services.Dtos.Business.Personnel
{
    public class ContractSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Keywords")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Keywords { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContractTypeId")]
        public ContractType? ContractTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }
    }
    public class ContractInfoDetail : ContractDetail
    {
        public EmployeeDetail Employee { get; set; }
        public ContactDetail Contact { get; set; }
        public CompanyDetail Company { get; set; }
        public JobPositionDetail JobPosition { get; set; }
    }
    public class ContractDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ContractId")]
        public int ContractId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContractNo")]
        public string ContractNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContractTypeId")]
        public ContractType ContractTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CompanyId")]
        public int CompanyId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        public int PositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProbationSalary")]
        public decimal? ProbationSalary { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InsuranceSalary")]
        public decimal? InsuranceSalary { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProbationFrom")]
        public DateTime? ProbationFrom { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProbationTo")]
        public DateTime? ProbationTo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SignedDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SignedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EffectiveDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EffectiveDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ExpiredDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiredDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }
    public class ContractEntry : DtoBase
    {
        public ContractEntry()
        {
            SignedDate = DateTime.UtcNow;
            EffectiveDate = DateTime.UtcNow.AddYears(1);
            ExpiredDate = DateTime.UtcNow.AddYears(1);
            ProbationFrom = DateTime.UtcNow;
            ProbationTo = DateTime.UtcNow.AddMonths(2);
            IsActive = true;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContractNo")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 1)]
        public string ContractNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CompanyId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        //[Range(typeof(int), "1", "100", ErrorMessage = "{0} can only be between {1} and {2}")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectCompany")]
        //[RegularExpression(@"\d*", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ValidInteger")]
        public int CompanyId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeName")]
        public string EmployeeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectEmployee")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectPosition")]
        public int PositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProbationFrom")]
        public DateTime? ProbationFrom { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProbationTo")]
        public DateTime? ProbationTo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProbationSalary")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ValidDecimal")]
        public decimal? ProbationSalary { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InsuranceSalary")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ValidDecimal")]
        public decimal? InsuranceSalary { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SignedDate")]
        public DateTime? SignedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EffectiveDate")]
        public DateTime? EffectiveDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ExpiredDate")]
        public DateTime? ExpiredDate { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsActive { get; set; }
    }
    public class ContractEditEntry : ContractEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ContractId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ContractId { get; set; }
    }
}
