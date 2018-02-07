using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement.FileStorage
{
    [Table("DocumentFolder")]
    public class DocumentFolder : BaseEntity
    {
        public DocumentFolder()
        {
            ParentId = 0;
            Depth = 1;
            HasChild = false;
            IsActive = DocumentFolderStatus.Active;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FolderId { get; set; }
        public int? ParentId { get; set; }
        public int? Depth { get; set; }
        public string Lineage { get; set; }
        public bool? HasChild { get; set; }
        public int? ListOrder { get; set; }
        public Guid FolderCode { get; set; }
        public string FolderName { get; set; }
        public string FolderPath { get; set; }
        public string FolderIcon { get; set; }
        public string Description { get; set; }
        public DocumentFolderStatus IsActive { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
