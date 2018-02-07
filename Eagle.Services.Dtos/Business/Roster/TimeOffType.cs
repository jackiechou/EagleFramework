using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business.Roster
{
    public class TimeOffTypeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TimeOffTypeId")]
        public int TimeOffTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TimeOffTypeName")]
        public string TimeOffTypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }

    public class TimeOffTypeEditEntry : TimeOffTypeEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TimeOffTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TimeOffTypeId { get; set; }
    }

    public class TimeOffTypeEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TimeOffTypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TimeOffTypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsActive { get; set; }
    }
}
