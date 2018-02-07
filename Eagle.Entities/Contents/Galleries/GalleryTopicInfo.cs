using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Contents.Galleries
{
    [NotMapped]
    public class GalleryTopicInfo: GalleryTopic
    {
        public GalleryTopicInfo()
        {
            Children = new List<GalleryTopicInfo>();
        }
        public List<GalleryTopicInfo> Parents { get; set; }
        public List<GalleryTopicInfo> Children { get; set; }
    }
}
