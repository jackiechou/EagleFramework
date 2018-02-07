using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.SystemManagement.FileStorage;

namespace Eagle.Entities.Contents.Galleries
{
    [NotMapped]
    public class GalleryFileInfo : GalleryFile
    {
        public virtual GalleryTopic GalleryTopic { get; set; }
        public virtual GalleryCollection GalleryCollection { get; set; }
        public virtual DocumentFile DocumentFile { get; set; }
        public virtual DocumentFolder DocumentFolder { get; set; }
    }
}
