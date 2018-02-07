using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Media
{
    [Table("MediaArtist", Schema = "Media")]
    public class MediaArtist : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string ArtistAlias { get; set; }
        public int? Photo { get; set; }
        public string Description { get; set; }
        public int? ListOrder { get; set; }
        public MediaArtistStatus Status { get; set; }
    }
}
