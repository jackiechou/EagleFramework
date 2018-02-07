using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Net.Mail;

namespace Eagle.Entities.SystemManagement.FileStorage
{
    [Table("FileStore")]
    public class FileStore : CleanEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileStoreId { get; set; }
        public string FileName { get; set; }
        public long? FileSize { get; set; }
        public string FileVersion { get; set; }
        public string StorageKey { get; set; }
        public int NetworkId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsProcessing { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? MediaResolutionType { get; set; }
        public string StorageType { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }

        public virtual ICollection<DownloadTracking> DownloadTrackings { get; set; }
        public virtual ICollection<LibraryProcessedInfo> LibraryProcessedInfos { get; set; }

        [NotMapped]
        public string FileExtension => Path.GetExtension(FileName);

        public void InitializeDefaultValues()
        {
            FileVersion = "1";
            StorageKey = Guid.NewGuid().ToString("D");
            IsProcessing = false;
            Deleted = false;

            //To do: This should be at generic repository, when you insert or update.
            CreatedDate = DateTime.UtcNow;
            LastUpdatedDate = DateTime.UtcNow;
        }

    }
}
