using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Articles
{
    [Table("NewsCategory")]
    public class NewsCategory : BaseEntity
    {
        public NewsCategory()
        {
            ParentId = 0;
            Depth = 1;
            HasChild = false;
            Status = NewsCategoryStatus.Published;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Alias { get; set; }
        public int? ParentId { get; set; }
        public int? Depth { get; set; }
        public string Lineage { get; set; }
        public bool? HasChild { get; set; }
        public string CategoryImage { get; set; }
        public string Description { get; set; }
        public string NavigateUrl { get; set; }
        public int? ListOrder { get; set; }
        public NewsCategoryStatus? Status { get; set; }

        public string LanguageCode { get; set; }
    }
}