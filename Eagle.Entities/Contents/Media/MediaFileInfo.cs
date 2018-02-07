using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.SystemManagement.FileStorage;

namespace Eagle.Entities.Contents.Media
{
    [NotMapped]
    public class MediaFileInfo : MediaFile
    {
        public virtual MediaType Type { get; set; }
        public virtual MediaTopic Topic { get; set; }
        public virtual MediaComposer Composer { get; set; }
        public virtual DocumentFile DocumentFile { get; set; }
        public virtual DocumentFolder DocumentFolder { get; set; }
    }
}
