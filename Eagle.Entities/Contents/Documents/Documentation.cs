using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Documents
{
    [Table("Documentation")]
    public class Documentation : BaseEntity
    {
        public Documentation()
        {
            Status = DocumentationStatus.Active;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentationId { get; set; }
        public int VendorId { get; set; }
        public int FileId { get; set; }

        public DocumentationStatus Status { get; set; }

    }

    [NotMapped]
    public class DocumentationInfo : Documentation
    {
        public string FileName { get; set; }
        public string VendorName { get; set; }
    }
}
