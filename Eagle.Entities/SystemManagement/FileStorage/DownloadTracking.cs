using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement.FileStorage
{
    [Table("DownloadTracking")]
    public class DownloadTracking : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DownloadId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string Code { get; set; }
        public DownloadStatus Status { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int PercentCompleted { get; set; }

        public int? FileId { get; set; }
    }
}
