using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Event
{
    public class EventTypeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeName")]
        public string TypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public int? Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Lineage")]
        public string Lineage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HasChild")]
        public bool? HasChild { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public EventTypeStatus Status { get; set; }
    }
    public class EventTypeEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [EnumDataType(typeof(EventTypeStatus))]
        public EventTypeStatus Status { get; set; }
    }
    public class EventTypeEditEntry : EventTypeEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TypeId { get; set; }
    }
}
