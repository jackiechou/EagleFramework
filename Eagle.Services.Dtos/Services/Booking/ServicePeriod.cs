using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Booking
{
    public class ServicePeriodSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PeriodName")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PeriodName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public bool? Status { get; set; }
    }
    public class ServicePeriodEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PeriodName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PeriodName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PeriodValue")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PeriodValue { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool Status { get; set; }
    }
    public class ServicePeriodEditEntry : ServicePeriodEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PeriodId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PeriodId { get; set; }
    }
    public class ServicePeriodDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PeriodId")]
        public int PeriodId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PeriodName")]
        public string PeriodName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PeriodValue")]
        public int PeriodValue { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public bool Status { get; set; }
    }
}
