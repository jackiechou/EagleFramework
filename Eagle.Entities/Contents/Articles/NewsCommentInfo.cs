using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Contents.Articles
{
    [NotMapped]
    public class NewsCommentInfo : NewsComment
    {
        public virtual News News { get; set; }
    }
}
