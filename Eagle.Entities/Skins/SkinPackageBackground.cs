using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Skins
{
    [Table("SkinPackageBackground")]
    public class SkinPackageBackground : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BackgroundId { get; set; }
        public string BackgroundName { get; set; }
        public int? BackgroundFile { get; set; }
        public string BackgroundLink { get; set; }
        public bool IsExternalLink { get; set; }
        public int ListOrder { get; set; }
        public bool IsActive { get; set; }

        public int PackageId { get; set; }
        public int TypeId { get; set; }
    }
}
