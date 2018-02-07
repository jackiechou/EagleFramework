using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Media
{
    [Table("MediaAlbum", Schema = "Media")]
    public class MediaAlbum: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string AlbumAlias { get; set; }
        public int? FrontImage { get; set; }
        public int? MainImage { get; set; }
        public string Description { get; set; }
        public int? TotalViews { get; set; }
        public int? ListOrder { get; set; }
        public MediaAlbumStatus Status { get; set; }


        public int TypeId { get; set; }
        public int TopicId { get; set; }
    }
}
