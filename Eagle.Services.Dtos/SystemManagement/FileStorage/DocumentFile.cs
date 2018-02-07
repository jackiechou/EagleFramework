using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.FileStorage
{
    public class DocumentFileDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        public int FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FolderId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int FolderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileCode")]
        public string FileCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileName")]
        public string FileName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileTitle")]
        public string FileTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Extension")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        public string FileExtension { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileVersion")]
        public string FileVersion { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileContent")]
        public byte[] FileContent { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContentType")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        public string FileType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileDescription")]
        public string FileDescription { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileSource")]
        public string FileSource { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StorageType")]
        public StorageType StorageType { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "Size")]
        public int? Size { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Width")]
        public int? Width { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Height")]
        public int? Height { get; set; }

        public DocumentFileStatus IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ClickThroughs")]
        public int? ClickThroughs { get; set; }
    }
    public class DocumentFileEditEntry : DocumentFileEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int FileId { get; set; }
    }
    public class DocumentFileEntry : DtoBase
    {
        public DocumentFileEntry()
        {
            StorageType = StorageType.Local;
            FileCode = Guid.NewGuid().ToString();
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileCode")]
        public string FileCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string FileName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileTitle")]
        public string FileTitle { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "FileDescription")]
        public string FileDescription { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Source")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string FileSource { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FolderId")]
        public int? FolderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Width")]
        public int? Width { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Height")]
        public int? Height { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StorageType")]
        public StorageType StorageType { get; set; }
    }
}
