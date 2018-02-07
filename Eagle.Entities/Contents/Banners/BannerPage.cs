using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Contents.Banners
{
    [Table("BannerPage")]
    public class BannerPage : EntityBase
    {
        [Key]
        public int BannerId { get; set; }

        [Key]
        public int PageId { get; set; }
    }
}