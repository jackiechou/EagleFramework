using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Contents.Articles
{
    [NotMapped]
    public class NewsInfo : News
    {
        public string CategoryName { get; set; }
        public string FullName { get; set; }

        public NewsCategory Category { get; set; }
    }
}
