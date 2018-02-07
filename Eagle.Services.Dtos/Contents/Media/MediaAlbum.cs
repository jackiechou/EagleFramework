using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.FileStorage;

namespace Eagle.Services.Dtos.Contents.Media
{
    public class MediaAlbumSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SearchText")]
        public string SearchText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SearchTopicId")]
        public int? SearchTopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SearchTypeId")]
        public int? SearchTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SearchStatus")]
        public MediaAlbumStatus? SearchStatus { get; set; }
    }
    public class MediaAlbumInfoDetail : MediaAlbumDetail
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FrontImageUrl")]
        public string FrontImageUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MainImageUrl")]
        public string MainImageUrl { get; set; }

        public MediaTypeDetail Type { get; set; }
        public MediaTopicDetail Topic { get; set; }

        public DocumentInfoDetail FrontImageInfo { get; set; }
        public DocumentInfoDetail MainImageInfo { get; set; }
    }
    public class MediaAlbumEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectTopic")]
        public int TopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AlbumName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string AlbumName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FrontImage")]
        public int? FrontImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MainImage")]
        public int? MainImage { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public MediaAlbumStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUpload")]
        public HttpPostedFileBase FileUpload { get; set; }
    }
    public class MediaAlbumEditEntry : MediaAlbumEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AlbumId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int AlbumId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FrontImageUrl")]
        public string FrontImageUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MainImageUrl")]
        public string MainImageUrl { get; set; }
    }
    public class MediaAlbumDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        public int TopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AlbumId")]
        public int AlbumId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AlbumName")]
        public string AlbumName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Alias")]
        public string AlbumAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FrontImage")]
        public int? FrontImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MainImage")]
        public int? MainImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalViews")]
        public int? TotalViews { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SortKey")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public MediaAlbumStatus Status { get; set; }

    }
    

    public class MediaAlbumFileInfoDetail : MediaAlbumFileDetail
    {
        public MediaAlbumDetail Album { get; set; }
        public MediaFileInfoDetail File { get; set; }
    }
    public class MediaAlbumFileDetail : DtoBase
    {
        public int MediaAlbumFileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AlbumId")]
        public int AlbumId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        public int FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public MediaAlbumFileStatus Status { get; set; }
    }

}
