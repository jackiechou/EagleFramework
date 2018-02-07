using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement.FileStorage
{
    [Table("DocumentFile")]
    public class DocumentFile : BaseEntity
    {
        public DocumentFile()
        {
            FileVersion = "1";
            FileCode = Guid.NewGuid().ToString();
            IsActive = DocumentFileStatus.Published;
            StorageType = StorageType.Local;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }
        public int? FolderId { get; set; }
        public string FileCode { get; set; } //StorageKey
        public string FileTitle { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileVersion { get; set; }
        public byte[] FileContent { get; set; }
        public string FileType { get; set; } // MIME Type + content type
        public string FileDescription { get; set; }
        public string FileSource { get; set; }
        public StorageType StorageType { get; set; }


        public int? Size { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public DocumentFileStatus IsActive { get; set; }

        public Guid ApplicationId { get; set; }
        public int? ClickThroughs { get; set; }
    }
}
