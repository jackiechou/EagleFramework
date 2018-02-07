using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.FileStorage;

namespace Eagle.Services.Dtos.Contents.Media
{
    public class MediaPlayListDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PlayListId")]
        public int PlayListId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PlayListName")]
        public string PlayListName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PlayListAlias")]
        public string PlayListAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FrontImage")]
        public int? FrontImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MainImage")]
        public int? MainImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalViews")]
        public int? TotalViews { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public MediaPlayListStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        public int TopicId { get; set; }
    }
    public class MediaPlayListEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PlayListName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PlayListName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PlayListAlias")]
        public string PlayListAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FrontImage")]
        public int? FrontImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MainImage")]
        public int? MainImage { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public MediaPlayListStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TypeId { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectTopic")]
        public int TopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUpload")]
        public HttpPostedFileBase FileUpload { get; set; }
    }
    public class MediaPlayListEditEntry : MediaPlayListEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PlayListId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PlayListId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FrontImageUrl")]
        public string FrontImageUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MainImageUrl")]
        public string MainImageUrl { get; set; }
    }


    public class MediaPlayListFileDetail : DtoBase
    {
        public int PlayListFileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PlayListId")]
        public int PlayListId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        public int FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public MediaPlayListFileStatus Status { get; set; }
    }
    public class MediaPlayListFileInfoDetail : MediaPlayListFileDetail
    {
        public MediaPlayListDetail PlayList { get; set; }
        public MediaFileInfoDetail File { get; set; }
    }
    public class MediaPlayListInfoDetail : MediaPlayListDetail
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

    public class MediaPlayListSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SearchText")]
        public string SearchText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SearchTypeId")]
        public int? SearchTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        public int? SearchTopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public MediaPlayListStatus? SearchStatus { get; set; }
    }

}
