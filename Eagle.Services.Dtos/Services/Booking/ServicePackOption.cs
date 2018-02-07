using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Booking
{
    public class ServicePackOptionDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OptionId")]
        public int OptionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OptionName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string OptionName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OptionValue")]
        public decimal? OptionValue { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ServicePackOptionStatus IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageId")]
        public int PackageId { get; set; }
    }

    public class ServicePackOptionEditEntry : ServicePackOptionEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PackageId")]
        public int? PackageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OptionId")]
        public int? OptionId { get; set; }
    }

    public class ServicePackOptionEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OptionName")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string OptionName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OptionValue")]
        public decimal? OptionValue { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ServicePackOptionStatus IsActive { get; set; }
    }
}
