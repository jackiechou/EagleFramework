using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Media
{
    [Table("MediaTopic", Schema = "Media")]
    public class MediaTopic : BaseEntity
    {
        public MediaTopic()
        {
            ParentId = 0;
            Depth = 1;
            HasChild = false;
            Status = MediaTopicStatus.Active;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string TopicAlias { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public int? Depth { get; set; }
        public string Lineage { get; set; }
        public bool? HasChild { get; set; }
        public int? ListOrder { get; set; }
        public int? Icon { get; set; }
        public MediaTopicStatus Status { get; set; }

        public int? TypeId { get; set; }
    }
}
