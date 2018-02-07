using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Contents.Media
{
    [Table("MediaFile", Schema = "Media")]
    public class MediaFile : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MediaId { get; set; }
        public int FileId { get; set; }
        public int TypeId { get; set; }
        public int TopicId { get; set; }
        public int? ComposerId { get; set; }
        public string Artist { get; set; }
        public bool? AutoStart { get; set; }
        public bool? MediaLoop { get; set; }
        public string Lyric { get; set; }
        public int? SmallPhoto { get; set; }
        public int? LargePhoto { get; set; }
        public int? ListOrder { get; set; }
    }
}
