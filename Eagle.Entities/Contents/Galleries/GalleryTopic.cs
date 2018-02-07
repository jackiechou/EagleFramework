using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Galleries
{
    [Table("GalleryTopic")]
    public class GalleryTopic : BaseEntity
    {
        public GalleryTopic()
        {
            ParentId = 0;
            Depth = 1;
            HasChild = false;
            Status = GalleryTopicStatus.Active;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string TopicCode { get; set; }
        public string TopicAlias { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public int? Depth { get; set; }
        public string Lineage { get; set; }
        public bool? HasChild { get; set; }
        public int? ListOrder { get; set; }
        public GalleryTopicStatus Status { get; set; }

    }
}
