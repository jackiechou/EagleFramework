using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business.Personnel
{
    public class TerminationDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TerminationId")]
        public int TerminationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Reason")]
        public string Reason { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InformedDate")]
        public DateTime InformedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastWorkingDate")]
        public DateTime LastWorkingDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsTerminationPaid")]
        public bool? IsTerminationPaid { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SignDate")]
        public DateTime? SignDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Signer")]
        public int? Signer { get; set; }
    }
    public class TerminationEntry : DtoBase
    {
        public TerminationEntry()
        {
            IsTerminationPaid = true;
        }
        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Reason")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Reason { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InformedDate")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime InformedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastWorkingDate")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime LastWorkingDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsTerminationPaid")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsTerminationPaid { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SignDate")]
        public DateTime? SignDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Signer")]
        public int? Signer { get; set; }
    }
    public class TerminationEditEntry : TerminationEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TerminationId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TerminationId { get; set; }
    }
}
