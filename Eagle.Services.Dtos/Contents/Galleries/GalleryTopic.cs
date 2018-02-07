using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Galleries
{
    public class GalleryTopicTreeGridDetail : DtoBase
    {
        public int id { get; set; }
        public GalleryTopicInfoDetail data { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public GalleryTreeState state { get; set; }
        public List<GalleryTopicTreeGridDetail> children { get; set; }
    }
    public class GalleryTopicInfoDetail : GalleryTopicDetail
    {
        [NotMapped]
        public string Action { get; set; }
    }
    public class GalleryTreeState
    {
        public bool? opened { get; set; }
        public bool? disabled { get; set; }
        public bool? selected { get; set; }
    }
    public class GalleryTopicDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        public int TopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TopicName")]
        public string TopicName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Alias")]
        public string TopicAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Code")]
        public string TopicCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public string Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Lineage")]
        public string Lineage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HasChild")]
        public bool? HasChild { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public GalleryTopicStatus Status { get; set; }
    }
    public class GalleryTopicEditEntry : GalleryTopicEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TopicId { get; set; }
    }
    public class GalleryTopicEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TopicName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TopicName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TopicCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TopicCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Status")]
        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public GalleryTopicStatus Status { get; set; }
    }
}
