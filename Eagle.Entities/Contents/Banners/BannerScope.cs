using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Banners
{
    [Table("BannerScope")]
    public class BannerScope : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScopeId { get; set; }
        public string ScopeName { get; set; }
        public string Description { get; set; }
        public BannerScopeStatus Status { get; set; }
    }
}
