using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Media
{
    [Table("MediaAlbumFile", Schema = "Media")]
    public class MediaAlbumFile: EntityBase
    {
        public MediaAlbumFile()
        {
            Status = MediaAlbumFileStatus.Active;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MediaAlbumFileId { get; set; }
        public int AlbumId { get; set; }
        public int FileId { get; set; }
        public int? ListOrder { get; set; }
        public MediaAlbumFileStatus Status { get; set; }
    }
}
