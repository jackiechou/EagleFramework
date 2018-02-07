using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.FileStorage;

namespace Eagle.Services.Dtos.Contents.Media
{
    public class MediaFileSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SearchText")]
        public string SearchText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SearchTypeId")]
        public int? SearchTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        public int? SearchTopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public DocumentFileStatus? SearchStatus { get; set; }
    }
    public class MediaFileInfoDetail : MediaFileDetail
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SmallPhotoUrl")]
        public string SmallPhotoUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LargePhotoUrl")]
        public string LargePhotoUrl { get; set; }

        public virtual MediaTypeDetail Type { get; set; }
        public virtual MediaTopicDetail Topic { get; set; }
        public virtual MediaComposerDetail Composer { get; set; }
        public DocumentInfoDetail DocumentFileInfo { get; set; }
        public DocumentInfoDetail SmallPhotoInfo { get; set; }
        public DocumentInfoDetail LargePhotoInfo { get; set; }
    }
    public class MediaFileEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required", ErrorMessage = null)]
        public string FileTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string FileDescription { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Url")]
        public string FileUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required", ErrorMessage = null)]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required", ErrorMessage = null)]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectTopic")]
        public int TopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ComposerId")]
        public int? ComposerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ArtistId")]
        public string Artist { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AutoStart")]
        public bool? AutoStart { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MediaLoop")]
        public bool? MediaLoop { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Lyric")]
        public string Lyric { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SmallPhoto")]
        public int? SmallPhoto { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LargePhoto")]
        public int? LargePhoto { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Photo")]
        public HttpPostedFileBase Photo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Link")]
        public HttpPostedFileBase Media { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Width")]
        public int? Width { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Height")]
        public int? Height { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StorageType")]
        public StorageType StorageType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Albums")]
        public List<int> Albums { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PlayLists")]
        public List<int> PlayLists { get; set; }
    }
    public class MediaFileEditEntry : MediaFileEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MediaId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required", ErrorMessage = null)]
        public int MediaId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required", ErrorMessage = null)]
        public int FileId { get; set; }

        public DocumentInfoDetail SmallPhotoInfo { get; set; }
        public DocumentInfoDetail LargePhotoInfo { get; set; }
    }
    public class MediaFileDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MediaId")]
        public int MediaId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        public int FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        public int TopicId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ComposerId")]
        public int? ComposerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ArtistId")]
        public string Artist { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AutoStart")]
        public bool? AutoStart { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MediaLoop")]
        public bool? MediaLoop { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Lyric")]
        public string Lyric { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SmallPhoto")]
        public int? SmallPhoto { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LargePhoto")]
        public int? LargePhoto { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }
    }
}
