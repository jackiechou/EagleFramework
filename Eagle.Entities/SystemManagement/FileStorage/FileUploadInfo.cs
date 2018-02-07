using System.ComponentModel.DataAnnotations;
using System.Web;
using Eagle.Resources;

namespace Eagle.Entities.SystemManagement.FileStorage
{
    public class FileUploadInfo : EntityBase
    {
        [Key]
        public int FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileName")]
        public string FileName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileTitle")]
        public string FileTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileContent")]
        public byte[] FileContent { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileDescription")]
        public string FileDescription { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Extension")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        public string Extension { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContentType")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        public string ContentType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FolderId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int FolderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FolderKey")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string FolderKey { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LanguageResource), Name = "FolderPath")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        public string FolderPath { get; set; }

        public string FileUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Size")]
        public int? Size { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Width")]
        public int? Width { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Height")]
        public int? Height { get; set; }

        public HttpPostedFileBase FileUploadName { get; set; }
        public int ItemId { get; set; }
        public string ItemTag { get; set; }
        public string FileIds { get; set; }
    }
    
}
