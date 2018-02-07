using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Skins
{
    [Table("SkinPackageTemplate")]
    public class SkinPackageTemplate : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateKey { get; set; }
        public string TemplateSrc { get; set; }
        public bool IsActive { get; set; }

        public int PackageId { get; set; }
        public int TypeId { get; set; }
    }
}
