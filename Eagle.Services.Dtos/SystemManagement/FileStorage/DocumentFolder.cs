using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.FileStorage
{
    public class DocumentFolderDetail : BaseDto
    {
        [Key]
        [Display(ResourceType = typeof(LanguageResource), Name = "FolderId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int FolderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FolderName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string FolderName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FolderPath")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        public string FolderPath { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FolderIcon")]
        public string FolderIcon { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public DocumentFolderStatus IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        public IEnumerable<DocumentFolderDetail> ChildFolders { get; set; }
        public IEnumerable<DocumentFileDetail> Files { get; set; }
    }
    public class DocumentFolderEditEntry : DocumentFolderEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FolderId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int FolderId { get; set; }
    }
    public class DocumentFolderEntry : DtoBase
    {
        public DocumentFolderEntry()
        {
            IsActive = DocumentFolderStatus.Published;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FolderName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string FolderName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FolderIcon")]
        public string FolderIcon { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LanguageResource), Name = "FolderPath")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        public string FolderPath { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DocumentFolderStatus IsActive { get; set; }
    }
}
