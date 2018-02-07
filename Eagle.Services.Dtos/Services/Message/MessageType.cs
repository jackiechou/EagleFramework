using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Message
{
    public class MessageTypeEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MessageTypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string MessageTypeName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool Status { get; set; }
    }
    public class MessageTypeEditEntry : MessageTypeEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MessageTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int MessageTypeId { get; set; }
    }
    public class MessageTypeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MessageTypeId")]
        public int MessageTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MessageTypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string MessageTypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool Status { get; set; }
    }
}
