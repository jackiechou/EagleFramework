using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Media
{
    [Table("MediaPlayList", Schema = "Media")]
    public class MediaPlayList : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayListId { get; set; }
        public string PlayListName { get; set; }
        public string PlayListAlias { get; set; }
        public int? FrontImage { get; set; }
        public int? MainImage { get; set; }
        public string Description { get; set; }
        public int? TotalViews { get; set; }
        public int? ListOrder { get; set; }
        public MediaPlayListStatus Status { get; set; }

        public int TypeId { get; set; }
        public int TopicId { get; set; }
    }
}
