using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Galleries
{
    public class GalleryCollectionSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        public int? SearchTopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CollectionName")]
        public string SearchCollectionName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public GalleryCollectionStatus? SearchStatus { get; set; }
    }
    public class GalleryCollectionSummary : GalleryCollectionDetail
    {
        public IEnumerable<GalleryCollectionInfoDetail> GalleryCollections { get; set; }
        //public List<DocumentInfoDetail> Documents { get; set; }
    }
    public class GalleryCollectionInfoDetail : GalleryCollectionDetail
    {
        public GalleryTopicDetail GalleryTopic { get; set; }
        public IEnumerable<GalleryFileInfoDetail> GalleryFiles { get; set; }
    }
    public class GalleryCollectionDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CollectionId")]
        public int CollectionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CollectionName")]
        public string CollectionName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconFile")]
        public int? IconFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public GalleryCollectionStatus Status { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        public int TopicId { get; set; }
    }
    public class GalleryCollectionEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ThisFieldMustBeSelected")]
        public int TopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CollectionName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ThisFieldMustBeSelected")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CollectionName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconFile")]
        public int? IconFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public GalleryCollectionStatus Status { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
    public class GalleryCollectionEditEntry : GalleryCollectionEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CollectionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ThisFieldMustBeSelected")]
        public int CollectionId { get; set; }
    }

}
