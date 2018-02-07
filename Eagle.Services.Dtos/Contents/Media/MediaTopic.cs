using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Media
{
    public class MediaTopicDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        public int TopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TopicName")]
        public string TopicName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int? TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeName")]
        public string TypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeAlias")]
        public string TypeAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Icon")]
        public int? Icon { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public MediaTopicStatus Status { get; set; }
    }
    public class MediaTopicEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int? TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TopicName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TopicName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Icon")]
        public int? Icon { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public MediaTopicStatus Status { get; set; }
    }
    public class MediaTopicEditEntry : MediaTopicEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TopicId { get; set; }
    }
}
