using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Contents.Media
{
    [NotMapped]
    public class MediaAlbumFileInfo : MediaAlbumFile
    {
        public virtual MediaAlbum Album { get; set; }
        public virtual MediaFile File { get; set; }
    }
}
