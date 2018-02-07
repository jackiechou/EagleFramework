using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Galleries
{
    [Table("GalleryFile")]
    public class GalleryFile : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GalleryFileId { get; set; }
        public int CollectionId { get; set; }
        public int FileId { get; set; }
        public int ListOrder { get; set; }
        public GalleryFileStatus Status { get; set; }
    }
}
