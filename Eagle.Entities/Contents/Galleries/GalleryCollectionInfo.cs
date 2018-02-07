using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Contents.Galleries
{
    [NotMapped]
    public class GalleryCollectionInfo: GalleryCollection
    {
        public GalleryTopic GalleryTopic { get; set; }
    }
}
