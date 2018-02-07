using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Contents.Media
{
    [NotMapped]
    public class MediaAlbumInfo : MediaAlbum
    {
        public virtual MediaType Type { get; set; }
        public virtual MediaTopic Topic { get; set; }
    }
}
