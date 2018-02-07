using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Booking
{
    public class ServicePackTypeEditEntry : ServicePackTypeEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }
    }
    public class ServicePackTypeEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsOnline")]
        public bool IsOnline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ServicePackTypeStatus IsActive { get; set; }
    }

    public class ServicePackTypeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeName")]
        public string TypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsOnline")]
        public bool IsOnline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ServicePackTypeStatus IsActive { get; set; }
    }
}
