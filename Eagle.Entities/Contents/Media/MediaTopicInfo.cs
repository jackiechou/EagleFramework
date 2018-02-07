using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Contents.Media
{
    [NotMapped]
    public class MediaTopicInfo : MediaTopic
    {
        public MediaTopicInfo()
        {
            Children = new List<MediaTopicInfo>();
        }
        public List<MediaTopicInfo> Parents { get; set; }
        public List<MediaTopicInfo> Children { get; set; }
    }
}
