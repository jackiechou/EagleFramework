using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Contents.Galleries
{
    [NotMapped]
    public class GalleryTopicTree: GalleryTopic
    {
        public List<GalleryTopicTree> Parents { get; set; }
        public List<GalleryTopicTree> Children { get; set; }

        public GalleryTopicTree()
        {
            Children = new List<GalleryTopicTree>();
            Parents = new List<GalleryTopicTree>();
        }
    }
}
