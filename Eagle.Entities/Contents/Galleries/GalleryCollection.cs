using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Galleries
{
    [Table("GalleryCollection")]
    public class GalleryCollection : BaseEntity
    {
        public GalleryCollection()
        {
            Status = GalleryCollectionStatus.Active;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollectionId { get; set; }
        public string CollectionName { get; set; }
        public string Description { get; set; }
        public int? IconFile { get; set; }
        public int? ListOrder { get; set; }
        public GalleryCollectionStatus Status { get; set; }

        public int TopicId { get; set; }
    }
}
