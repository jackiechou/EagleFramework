using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Banners
{
    [Table("BannerPosition")]
    public class BannerPosition : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
        public int ListOrder { get; set; }
        public BannerPositionStatus Status { get; set; }
    }
}
