using System.ComponentModel.DataAnnotations;
using System.Web;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.FileStorage;

namespace Eagle.Services.Dtos.Contents.Galleries
{
    public class GalleryFileSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        public int? SearchTopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CollectionId")]
        public int? SearchCollectionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public GalleryFileStatus? SearchStatus { get; set; }
    }
    public class GalleryFileInfoDetail : GalleryFileDetail
    {
        public GalleryTopicDetail GalleryTopic { get; set; }
        public GalleryCollectionDetail GalleryCollection { get; set; }
        public DocumentInfoDetail File { get; set; }
    }
    public class GalleryFileDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CollectionId")]
        public int GalleryFileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CollectionId")]
        public int CollectionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        public int FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public GalleryFileStatus Status { get; set; }
    }

    public class GalleryFileEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectTopic")]
        public int TopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CollectionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectCollection")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectCollection")]
        public int CollectionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public GalleryFileStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AttachFile")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ThisFieldMustBeSelected")]
        public HttpPostedFileBase File { get; set; }
    }
    public class GalleryFileEditEntry : GalleryFileEntry
    {
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int GalleryFileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ThisFieldMustBeSelected")]
        public int FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string FileUrl { get; set; }
    }
}
