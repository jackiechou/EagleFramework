using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business.Personnel
{
    public class JobPositionSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PositionName")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PositionName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }
    }
    public class JobPositionDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        public int PositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionName")]
        public string PositionName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }
    public class JobPositionEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PositionName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(300, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PositionName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }
    public class JobPositionEditEntry : JobPositionEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PositionId { get; set; }
    }
}
