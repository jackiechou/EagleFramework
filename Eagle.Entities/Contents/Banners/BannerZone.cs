using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Resources;

namespace Eagle.Entities.Contents.Banners
{
    [Table("BannerZone")]
    public class BannerZone: EntityBase
    {
        [Key]
        public int BannerId { get; set; }

        [Key]
        public int PositionId { get; set; }
    }
}
